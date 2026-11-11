using System.Diagnostics.CodeAnalysis;
using VMTranslator;

class Program
{

    static void Main(string[] args)
    {
        //Check command-line arguments
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: program <source>");
            return;
        }

        string source = args[0];
        string outputFile = source + ".asm";

        if (Path.GetExtension(source).Equals(".vm"))//one file
        {
            string fileName = Path.GetFileNameWithoutExtension(source);
            CodeWriter codeWriter = new(fileName, outputFile);

            TranslateFile(source, codeWriter);
            codeWriter.Close();
        }
        else //folder
        {
            CodeWriter codeWriter = new("Bootstrap", outputFile);

            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {
                codeWriter.SetFileName(Path.GetFileNameWithoutExtension(file));
                TranslateFile(file, codeWriter);
            }

            codeWriter.Close();
        }
    }

    private static void TranslateFile(string inputFile, CodeWriter codeWriter)
    {
        Parser parser = new(inputFile);
        
        while (parser.HasMoreLine())
        {
            var command = parser.GetCommandType();
            switch (command)
            {
                case CommandType.C_PUSH:
                case CommandType.C_POP:
                    codeWriter.WritePushPop(command, parser.GetArg1(), parser.GetArg2());
                    break;
                case CommandType.C_ARITHMETIC:
                    codeWriter.WriteArithmetic(parser.GetArg1());
                    break;
                case CommandType.C_LABEL:
                    codeWriter.WriteLabel(parser.GetArg1());
                    break;
                case CommandType.C_GOTO:
                    codeWriter.WriteGoto(parser.GetArg1());
                    break;
                case CommandType.C_IF:
                    codeWriter.WriteIf(parser.GetArg1());
                    break;
                case CommandType.C_CALL:
                    codeWriter.WriteCall(parser.GetArg1(), parser.GetArg2());
                    break;
                case CommandType.C_FUNCTION:
                    codeWriter.WriteFunction(parser.GetArg1(), parser.GetArg2());
                    break;
                case CommandType.C_RETURN:
                    codeWriter.WriteReturn();
                    break;
            }

            parser.Advance();
        }
    }
        
}