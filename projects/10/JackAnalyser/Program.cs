using JackAnalyser;

class Program
{
    /*The syntax analyzer accepts a single command-line argument: source
     * where source is either a filename (.jack) or a folder.
     * Fore ach .jack file, the JackAnalyser a .xml file
     */
    static void Main(string[] args)
    {
        //Check command-line arguments
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: program <source>");
            return;
        }

        string source = args[0];

        if (Path.GetExtension(source).Equals(".jack"))//one file
        {
            AnalyseFile(source, Path.GetFileNameWithoutExtension(source) + ".xml");
        }
        else //folder
        {
            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {

            }
        }
    }
    private static void AnalyseFile(string inputFile, string outputFile)
    {
        var streamReader = new StreamReader(inputFile);
        var streamWriter = new StreamWriter(outputFile);

        var jackTokenizer = new JackTokenizer(streamReader);
        var compilationEngine = new CompilationEngine(streamReader, streamWriter);

        while (jackTokenizer.HasMoreTokens())
        {

            jackTokenizer.Advance();
        }

        streamReader.Close();
        streamWriter.Close();
    }
}
