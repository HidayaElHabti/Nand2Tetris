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
                string instruction = "";
                if(parser.GetInstructionType() == InstructionType.C_INSTRUCTION)
                {
                    instruction = "111" + Code.Comp(parser.Comp()) + Code.Dest(parser.Dest()) + Code.Jump(parser.Jump());
                }

                else if (parser.GetInstructionType() == InstructionType.A_INSTRUCTION)
                {
                    int address = int.Parse(parser.Symbol());
                    instruction = Convert.ToString(address, 2).PadLeft(16, '0');
                }
                outputLines.Add(instruction);

                parser.Advance(sr);
            }
        }

        File.WriteAllLines(outputProgram, outputLines);
    }
}
