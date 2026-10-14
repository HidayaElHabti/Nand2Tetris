using HackAssembler;

class Program
{
    static void Main(string[] args)
    {
        //Check command-line arguments
        if(args.Length != 1)
        {
            Console.WriteLine("Usage: program <filename>");
            return;
        }

        string filename = args[0];
        string inputProgram = filename + ".asm";
        string outputProgram = filename + ".hack";

        //Check if input file exist
        if (!File.Exists(inputProgram))
        {
            Console.WriteLine($"Assembly program not found: {inputProgram}");
            return;
        }

        SymbolTable symbolTable = new();
        AddLabelSymbols(inputProgram, symbolTable);
        TranslateCode(inputProgram, outputProgram, symbolTable);
    }

    private static void AddLabelSymbols(string inputFilePath, SymbolTable symbolTable)
    {
        symbolTable.InitializeTable();

        int lineNumber = 0;
        
        Parser parser = new(inputFilePath);
        
        while (parser.HasMoreLine())
        {
            if(parser.GetInstructionType() == InstructionType.C_INSTRUCTION)
            {
                lineNumber++;
            }
            else if (parser.GetInstructionType() == InstructionType.A_INSTRUCTION)
            {
                lineNumber++;
            }
            else if (parser.GetInstructionType() == InstructionType.L_INSTRUCTION)
            {
                string symbol = parser.Symbol();
                if (!symbolTable.Contains(symbol))
                {
                    symbolTable.AddEntry(symbol, lineNumber);
                }

            }

            parser.Advance();
        }
    }

    private static void TranslateCode(string inputFilePath, string outputFilePath, SymbolTable symbolTable)
    {
        int variableCounter = 16;
        bool firstLine = true;

        using StreamWriter sw = new(outputFilePath);
        Parser parser = new(inputFilePath);
        while (parser.HasMoreLine())
        {
            string instruction = "";
            if(parser.GetInstructionType() == InstructionType.L_INSTRUCTION)
            {
                parser.Advance();
                continue;
            }
            else if (parser.GetInstructionType() == InstructionType.C_INSTRUCTION)
            {
                instruction = "111" + Code.Comp(parser.Comp()) + Code.Dest(parser.Dest()) + Code.Jump(parser.Jump());
            }
            else if (parser.GetInstructionType() == InstructionType.A_INSTRUCTION)
            {
                string symbol = parser.Symbol();
                if (int.TryParse(symbol, out int address))
                {
                    instruction = Convert.ToString(address, 2).PadLeft(16, '0');
                }
                else if (symbolTable.Contains(symbol))
                {
                    instruction = Convert.ToString(symbolTable.GetAddress(symbol), 2).PadLeft(16, '0');
                }
                else
                {
                    symbolTable.AddEntry(symbol, variableCounter);
                    instruction = Convert.ToString(variableCounter, 2).PadLeft(16, '0');
                    variableCounter++;
                }
            }

            if (!firstLine)
                sw.Write(Environment.NewLine);

            sw.Write(instruction);

            firstLine = false;

            parser.Advance();
        }
    }
}
