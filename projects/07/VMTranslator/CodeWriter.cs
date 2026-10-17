using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator
{
    internal class CodeWriter
    {
        private readonly string fileName;
        private readonly StreamWriter streamWriter;
        private int counter;
        public CodeWriter(string outputFileName) 
        {
            fileName = outputFileName;
            streamWriter = new(outputFileName);
            counter = 0;
        }

        public void WriteArithmetic(string command)
        {
            //pop 1st last element of stack (y) to D register
            WritePopInstructions();
            //pop 2nd last element of stack (x)
            //perform arithmetic command with the last element saved in D register
            //save result in D Register
            WritePopInstructions(command);

            //push value of D register to stack
            WritePushInstructions();
        }

        public void WritePushPop(CommandType command, string segment, int index)
        {
            if(command == CommandType.C_PUSH)
            {
                if(segment == "constant")
                {
                    WriteAInstruction(index.ToString());//@index
                    WriteDRegisterAssignmentInstruction("A");//D=A
                }
                else if(segment == "static")
                {
                    WriteAInstruction(fileName + '.' + index);//@FileName.i
                    WriteDRegisterAssignmentInstruction("M");//D=M
                }
                else
                {
                    WriteAInstructionForSegment(segment);//@SEGMENT
                    WriteCInstruction("A", "M+" + index);//A=M+i
                    WriteDRegisterAssignmentInstruction("M");
                }

                WritePushInstructions();
            }
            else if (command == CommandType.C_POP)
            {
                WritePopInstructions();
                if(segment == "static")
                {
                    WriteAInstruction(fileName + '.' + index);//@FileName.i
                    WriteCInstruction("M", "D");//M=D
                }
                else
                {
                    WriteAInstructionForSegment(segment);//@SEGMENT
                    WriteCInstruction("A", "M+" + index);//A=M+i
                    WriteCInstruction("M", "D");
                }
            }
        }//temp and pointer segment implementations are missing

        public void Close()
        {
            //write infinite loop
            WriteInstruction("(END)");
            WriteInstruction("@END");
            WriteInstruction("0;JMP");

            streamWriter.Close();
        }

        #region private
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
            }
        }
        private void WritePushInstructions()//From value of D Register
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
        private void WritePopInstructions(string? operation = null)
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
                    WriteDRegisterAssignmentInstruction("D-M");//D=D-M
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
            WriteAInstruction("TRUE_" + counter);//@TRUE_counter
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
            WriteAInstruction("END_" + counter);//@END_counter
            WriteInstruction("(TRUE_" + counter + ")");//(TRUE_counter)
            WriteDRegisterAssignmentInstruction("-1");//D=-1
            WriteInstruction("(END_" + counter + ")");//(END_counter)
            counter++;
        }
        #endregion private
    }
}
