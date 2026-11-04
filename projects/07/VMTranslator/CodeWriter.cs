using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator
{
    internal class CodeWriter
    {
        private string fileName;
        private readonly StreamWriter streamWriter;
        private int boolean_counter;
        private int return_counter;
        public CodeWriter(string inputFileName, string outputFile) 
        {
            fileName = inputFileName;
            streamWriter = new(outputFile);
            boolean_counter = 0;
            return_counter = 0;
            //TODO: write bootstrap code
        }

        public void SetFileName(string newFileName)
        {
            fileName = newFileName;
        }
        public void WriteArithmetic(string command)
        {
            if(command != "neg" & command != "not")
                //pop y to D register
                WritePopInstructionsIntoDRegister();
            //pop x
            //perform arithmetic command with the last element saved in D register
            //save result in D Register
            WritePopInstructionsIntoDRegister(command);

            //push value of D register to stack
            WritePushInstructionsFromDRegister();
        }
        public void WritePushPop(CommandType command, string segment, int index)
        {
            if (command == CommandType.C_PUSH)
                WritePushInstructions(segment, index);
            else if (command == CommandType.C_POP)
                WritePopInstructions(segment, index);
        }
        public void WriteLabel(string label) 
        {
            WriteInstruction('(' + fileName + '.' + label + ')');//(FileName.label)
        }
        public void WriteGoto(string label) 
        {
            WriteAInstruction(fileName + '.' + label);//@FileName.Label
            WriteInstruction("0;JMP");//0;JMP

        }
        public void WriteIf(string label) 
        {
            WritePopInstructionsIntoDRegister();
            WriteAInstruction(fileName + '.' + label);//@FileName.Label
            WriteInstruction("D;JNE");//D;JNE
        }
        public void WriteFunction(string functionName, int nVars)
        {
            WriteInstruction('(' + functionName + ')');
            for (int i = 0; i < nVars; i++)
            {
                WritePushInstructions("constant", 0);
            }
        }
        public void WriteCall(string functionName, int nArgs)
        {
            string returnAddress = fileName + '.' + functionName + "$ret." + return_counter;
            //save frame of caller
            WritePushInstructions(returnAddress);
            WritePushInstructions("LCL");
            WritePushInstructions("ARG");
            WritePushInstructions("THIS");
            WritePushInstructions("THAT");

            //reposition ARG and LCL
            WritePushInstructions("SP");
            WritePushInstructions("constant", 5);
            WritePushInstructions("constant", nArgs);
            WriteArithmetic("sub");
            WriteArithmetic("sub");
            WritePopInstructions("ARG");

            WritePushInstructions("SP");
            WritePopInstructions("LCL");

            //goto f
            WriteAInstruction(functionName);//@FunctionName
            WriteInstruction("0;JMP");

            //define returnAddress
            WriteInstruction('(' + returnAddress + ')');

            return_counter++;
        }
        public void WriteReturn()
        {
            //copy return value into argument 0
            WritePopInstructions("argument", 0);

            //FRAME = LCL - 5
            WritePushInstructions("LCL");
            WritePushInstructions("constant", 5);
            WriteArithmetic("sub");
            WritePopInstructions("FRAME");

            //Save value of new SP = ARG + 1
            WritePushInstructions("ARG");
            WritePushInstructions("constant", 1);
            WriteArithmetic("add");

            //returnAddress = LCL-5 = frame 0
            WritePushInstructions("frame", 0);
            WritePopInstructions("returnAddress");
            //LCL = LCL - 4 = frame 1
            WritePushInstructions("frame", 1);
            WritePopInstructions("LCL");
            //ARG = LCL - 4 = frame 2
            WritePushInstructions("frame", 2);
            WritePopInstructions("ARG");
            //THIS = LCL - 2 = frame 3
            WritePushInstructions("frame", 3);
            WritePopInstructions("THIS");
            //THAT = LCL - 1 = frame 4
            WritePushInstructions("frame", 4);
            WritePopInstructions("THAT");

            //reposition SP : SP=ARG+1
            WritePopInstructions("SP");//from value pushed above 

            //goto returnAddressAddress
            WriteAInstruction("returnAddress");
            WriteInstruction("A=M");
            WriteInstruction("0;JMP");
        }
        public void Close()
        {
            //write infinite loop
            WriteInstruction("(END)");
            WriteInstruction("@END");
            WriteInstruction("0;JMP");

            streamWriter.Close();
        }

        #region private
        private void WritePushInstructions(string address)
        {
            WriteAInstruction(address);//@value
            WriteDRegisterAssignmentInstruction("M");//D=M
            WritePushInstructionsFromDRegister();//D->Stack
        }
        private void WritePushInstructions(string segment, int index)
        {
            if (segment == "constant")
            {
                WriteAInstruction(index.ToString());//@index
                WriteDRegisterAssignmentInstruction("A");//D=A
            }
            else if (segment == "static")
            {
                WriteAInstruction(fileName + '.' + index);//@FileName.i
                WriteDRegisterAssignmentInstruction("M");//D=M
            }
            else if (segment == "pointer")
            {
                switch (index)
                {
                    case 0:
                        WriteAInstruction("THIS");//@THIS
                        break;
                    case 1:
                        WriteAInstruction("THAT");//@THAT
                        break;
                }
                WriteDRegisterAssignmentInstruction("M");//D=M
            }
            else
            {
                WriteAInstruction(index.ToString());//@index
                WriteDRegisterAssignmentInstruction("A");//D=A
                WriteAInstructionForSegment(segment);//@SEGMENT
                if(segment == "temp")
                    WriteCInstruction("A", "D+A");//A=D+A=TEMP+index
                else
                    WriteCInstruction("A", "D+M");//A=D+M=base_address+index
                WriteDRegisterAssignmentInstruction("M");
            }

            WritePushInstructionsFromDRegister();
        }
        private void WritePopInstructions(string address)
        {
            WritePopInstructionsIntoDRegister();
            WriteAInstruction(address);//@address
            WriteCInstruction("M", "D");//M=D
        }
        private void WritePopInstructions(string segment, int index)
        {
            if (segment == "static")
            {
                WritePopInstructionsIntoDRegister();
                WriteAInstruction(fileName + '.' + index);//@FileName.i
                WriteCInstruction("M", "D");//M=D
            }
            else if(segment == "pointer")
            {
                WritePopInstructionsIntoDRegister();
                switch (index)
                {
                    case 0:
                        WriteAInstruction("THIS");//@THIS
                        break;
                    case 1:
                        WriteAInstruction("THAT");//@THAT
                        break;
                }
                WriteCInstruction("M", "D");//M=D
            }
            else
            {
                WriteAInstruction(index.ToString());//@index
                WriteDRegisterAssignmentInstruction("A");//D=A
                WriteAInstructionForSegment(segment);//@SEGMENT
                if (segment == "temp")
                    WriteDRegisterAssignmentInstruction("D+A");//D=D+A
                else
                    WriteDRegisterAssignmentInstruction("D+M");//D=D+M
                WriteAInstruction("R13");//@R13
                WriteCInstruction("M", "D");//M=D

                WritePopInstructionsIntoDRegister();//D=popped value

                WriteAInstruction("R13");//@R13
                WriteCInstruction("A", "M");//A=M
                WriteCInstruction("M", "D");//M=D
            }
            
        }
        private void WriteInstruction(string instruction)
        {
            streamWriter.WriteLine("//" + instruction);
            streamWriter.WriteLine(instruction);
        }
        private void WriteAInstruction(string address)
        {
            WriteInstruction("@" + address);//@address
        }
        private void WriteCInstruction(string dest, string comp)
        {
            WriteInstruction(dest + '=' + comp);//dest=comp
        }
        private void WriteDRegisterAssignmentInstruction(string value)
        {
            WriteCInstruction("D", value);//D=value
        }
        private void WriteAInstructionForSegment(string segment)
        {
            switch (segment)
            {
                case "local":
                    WriteAInstruction("LCL");//@LCL
                    break;
                case "argument":
                    WriteAInstruction("ARG");//@ARG
                    break;
                case "this":
                    WriteAInstruction("THIS");//@THIS
                    break;
                case "that":
                    WriteAInstruction("THAT");//@THAT
                    break;
                case "temp":
                    WriteAInstruction("5");//@5
                    break;
                case "frame":
                    WriteAInstruction("FRAME");//@FRAME
                    break;
            }
        }
        private void WritePushInstructionsFromDRegister()//From value of D Register
        {
            //Add value of D register to stack
            WriteAInstruction("SP");//@SP
            WriteCInstruction("A", "M");//A=M
            WriteCInstruction("M", "D");//M=D

            //Increment Stack Pointer
            WriteAInstruction("SP");//@SP
            WriteCInstruction("M", "M+1");//M=M+1
        }
        //Write instructions that pops the last element of stack, or the result of an arithmetic operation on the last element(s)
        //into the D register, 
        private void WritePopInstructionsIntoDRegister(string? operation = null)
        {
            WriteAInstruction("SP");//@SP
            WriteCInstruction("M", "M-1");//M=M-1
            WriteCInstruction("A", "M");//A=M
            if(operation != null)
                WriteDRegisterArithmeticInstruction(operation);
            else
                WriteDRegisterAssignmentInstruction("M");//D=M
        }
        private void WriteDRegisterArithmeticInstruction(string? operation = null)
        {
            switch(operation)//D=DopM or D=opD
            {
                case "add":
                    WriteDRegisterAssignmentInstruction("D+M");//D=D+M
                    break;
                case "sub":
                    WriteDRegisterAssignmentInstruction("M-D");//D=M-D
                    break;
                case "neg":
                    WriteDRegisterAssignmentInstruction("-D");//D=-D
                    break;
                case "and":
                    WriteDRegisterAssignmentInstruction("D&M");//D=D&M
                    break;
                case "or":
                    WriteDRegisterAssignmentInstruction("D|M");//D=D|M
                    break;
                case "not":
                    WriteDRegisterAssignmentInstruction("!D");//D=!D
                    break;
                case "eq":
                case "lt":
                case "gt":
                    WriteDRegisterTrueOrFalseAssignementInstruction(operation);
                    break;
            }
        }
        private void WriteDRegisterTrueOrFalseAssignementInstruction(string operation)
        {
            WriteDRegisterAssignmentInstruction("D-M");//D=D-M=y-x
            WriteAInstruction("TRUE_" + boolean_counter);//@TRUE_counter
            switch (operation)
            {
                case "eq":
                    WriteInstruction("D;JEQ");//D;JEQ
                    break;
                case "lt":
                    WriteInstruction("D;JGT");//D;JGT
                    break;
                case "gt":
                    WriteInstruction("D;JLT");//D;JLT
                    break;
            }
            WriteDRegisterAssignmentInstruction("0");//D=0
            WriteAInstruction("END_" + boolean_counter);//@END_counter
            WriteInstruction("0;JMP");//0;JMP
            WriteInstruction("(TRUE_" + boolean_counter + ")");//(TRUE_counter)
            WriteDRegisterAssignmentInstruction("-1");//D=-1
            WriteInstruction("(END_" + boolean_counter + ")");//(END_counter)
            boolean_counter++;
        }
        #endregion private
    }
}
