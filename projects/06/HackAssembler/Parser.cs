using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    internal class Parser
    {
        private string? currentLine;
        private int? indexEqual;
        private int? indexSemicolon;
        public Parser(StreamReader streamReader)
        {
            currentLine = streamReader.ReadLine();
            indexEqual = currentLine?.IndexOf('=');
            indexSemicolon = currentLine?.IndexOf(';');
        }
        

        //Are there more lines in the input file?
        public bool HasMoreLine()
        {
            return currentLine != null;
        }

        /*
        Reads the current line from the input, and makes it the current instruction.
        Skips over white space and comments.
         */
        public void Advance(StreamReader streamReader)
        {
            currentLine = streamReader.ReadLine();
            if(currentLine != null)
            {
                currentLine = currentLine.Trim();
                if(currentLine == "")
                    Advance(streamReader);
                if(currentLine.StartsWith("//"))
                    Advance(streamReader);
            }
        }

        //Returns the type of the current instruction
        public InstructionType GetInstructionType()
        {
            if (currentLine!.StartsWith('@'))
                return InstructionType.A_INSTRUCTION;
            else if (currentLine!.StartsWith('('))
                return InstructionType.L_INSTRUCTION;
            else
                return InstructionType.C_INSTRUCTION;
        }

        //Returns the symbol xxx if the current instruction is (xxx) or @xxx
        public string Symbol()
        {
            return "xxx";
        }

        //Returns the symbolic dest part of the current C-instruction
        public string Dest()
        {
            if (indexEqual == -1)
                return string.Empty;
            return currentLine!.Substring(0, indexEqual!.Value);
        }

        //Returns the symbolic comp part of the current C-instruction
        public string Comp()
        {
            if(Dest() == string.Empty && Jump() == string.Empty)
                return currentLine!;
            if(Jump() == string.Empty)
                return currentLine!.Substring(indexEqual!.Value + 1);
            if(Dest() == string.Empty)
                return currentLine!.Substring(0, indexSemicolon!.Value);

            return currentLine!.Substring(indexEqual!.Value + 1, indexSemicolon!.Value);
        }

        //Returns the symbolic jump part of the current C-instruction
        public string Jump()
        {
            if(indexSemicolon == -1)
                return string.Empty;
            return currentLine!.Substring(indexSemicolon!.Value + 1);
        }
    }
    enum InstructionType
    {
        A_INSTRUCTION,
        C_INSTRUCTION,
        L_INSTRUCTION
    }
}
