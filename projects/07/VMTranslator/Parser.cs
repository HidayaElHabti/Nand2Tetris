using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator
{
    internal class Parser
    {
        private string? currentLine;
        private readonly StreamReader streamReader;
        private readonly static List<string> operations = ["add", "sub",  "eq", "gt", "lt", "and", "or", "neg", "not"];
        public Parser(string inputFilePath)
        {
            streamReader = new(inputFilePath);
            Advance();
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
        public void Advance()
        {
            currentLine = streamReader.ReadLine();
            if (currentLine != null)
            {
                currentLine = currentLine.Trim();
                if (currentLine == "")
                    Advance();
                if (currentLine.StartsWith("//"))
                    Advance();
            }
        }

        public CommandType GetCommandType()
        {
            if (currentLine!.StartsWith("push"))
                return CommandType.C_PUSH;
            if (currentLine!.StartsWith("pop"))
                return CommandType.C_POP;
            if (operations.Contains(currentLine!))
                return CommandType.C_ARITHMETIC;
            else //TODO: complete implementation
                return CommandType.C_ARITHMETIC;
        }

        public string GetArg1()
        {
            if (GetCommandType() == CommandType.C_ARITHMETIC)
                return currentLine!;
            else //Pop or push commands
                return currentLine!.Split(' ')[1];
        }

        public int GetArg2()
        {
            return int.Parse(currentLine!.Split(' ')[2]);//pop and push commands
        }
    }
    enum CommandType
    {
        C_ARITHMETIC,
        C_PUSH,
        C_POP,
        C_LABEL,
        C_GOTO,
        C_FUNCTION,
        C_RETURN,
        C_CALL
    }
}
