using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    internal class Code
    {
        //Returns the binary code of the dest mnemonic
        public string Dest(string mnemonic)
        {
            if (mnemonic == string.Empty)
                return "000";
            char[] dest = {'0','0','0'};
            if (mnemonic.Contains('M'))
                dest[0] = '1';
            if(mnemonic.Contains('D'))
                dest[1] = '1';
            if(mnemonic.Contains('A'))
                dest[2] = '1';
            return new string(dest);
        }

        //Returns the binary code of the comp mnemonic
        public string Comp(string mnemonic)
        {
            string a = mnemonic.Contains('M')? "1" : "0";

            switch (mnemonic){
                case "0":   
                    return a + "101010";
                case "1":   
                    return a + "111111";
                case "-1":  
                    return a + "111010";
                case "D":   
                    return a + "001100";
                case "A":
                case "M":
                    return a + "110000";
                case "!D":   
                    return a + "001101";
                case "!A":
                case "!M":
                    return a + "110001";
                case "-D":   
                    return a + "001111";
                case "-A":
                case "-M":
                    return a + "110011";
                case "D+1":   
                    return a + "011111";
                case "A+1":
                case "M+1":
                    return a + "110111";
                case "D-1":   
                    return a + "001110";
                case "A-1":
                case "M-1":
                    return a + "110010";
                case "D+A":
                case "D+M":
                    return a + "000010";
                case "D-A":
                case "D-M":
                    return a + "010011";
                case "A-D":
                case "M-D":
                    return a + "000111";
                case "D&A":
                case "D&M": 
                    return a + "000000";
                case "D|A":
                case "D|M":
                    return a + "010101";
                default: return a + "101010";
            }
        }

        //Returns the binary code of the jump mnemonic
        public string Jump(string mnemonic)
        {
            return mnemonic switch
            {
                "" => "000",
                "JGT" => "001",
                "JEQ" => "010",
                "JGE" => "011",
                "JLT" => "100",
                "JNE" => "101",
                "JLE" => "110",
                "JMP" => "111",
                _ => "000",
            };
        }
    }
}
