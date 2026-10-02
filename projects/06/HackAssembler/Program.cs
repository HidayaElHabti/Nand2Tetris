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

        var outputLines = new List<string>();

        using(StreamReader sr = new(inputProgram))
        {
            Parser parser = new(sr);
            while (parser.HasMoreLine())
            {
                if(parser.GetInstructionType() == InstructionType.C_INSTRUCTION)
                {
                    string instruction;
                    outputLines.Add(instruction);

                }

                else if (parser.GetInstructionType() == InstructionType.A_INSTRUCTION)
                {

                }
                
                parser.Advance(sr);
            }
        }
    }
}
