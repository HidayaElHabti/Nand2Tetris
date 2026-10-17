using VMTranslator;

class Program
{
    static void Main(string[] args)
    {
        //Check command-line arguments
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: program <filename>");
            return;
        }

        string filename = args[0];
        string inputProgram = filename + ".vm";
        string outputProgram = filename + ".asm";

        //Check if input file exist
        if (!File.Exists(inputProgram))
        {
            Console.WriteLine($"Assembly program not found: {inputProgram}");
            return;
        }

        Parser parser = new(inputProgram);
        CodeWriter codeWriter = new(outputProgram);
        while (parser.HasMoreLine())
        {
            var command = parser.GetCommandType();
            switch (parser.GetCommandType())
            {
                case CommandType.C_PUSH:
                case CommandType.C_POP:
                    codeWriter.WritePushPop(command, parser.GetArg1(), parser.GetArg2());
                    break;
                case CommandType.C_ARITHMETIC:
                    codeWriter.WriteArithmetic(parser.GetArg1());
                    break;
            }

            parser.Advance();
        }

        codeWriter.Close();
    }
}