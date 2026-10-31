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

        if(source.EndsWith(".vm"))//one file
        {
            TranslateFile(source);
        }
        else //folder
        {
            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {
                if (!file.EndsWith(".vm"))
                    continue;
                TranslateFile(file);
            }
        }
        //does this work with paths?
    }

    private static void TranslateFile(string file)
    {
        string fileName = file[..file.IndexOf('.')];
        string outputFile = fileName + ".asm";

        Parser parser = new(file);
        CodeWriter codeWriter = new(fileName, outputFile);
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