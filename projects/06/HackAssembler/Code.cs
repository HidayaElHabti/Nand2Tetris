using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    internal static class Code
    {
        //Returns the binary code of the dest mnemonic
        public static string Dest(string mnemonic)
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
        public static string Comp(string mnemonic)
        {
            string a = mnemonic.Contains('M')? "1" : "0";

            return a + mnemonic switch
            {
                "0" => "101010",
                "1" => "111111",
                "-1" => "111010",
                "D" => "001100",
                "A" or "M" => "110000",
                "!D" => "001101",
                "!A" or "!M" => "110001",
                "-D" => "001111",
                "-A" or "-M" => "110011",
                "D+1" => "011111",
                "A+1" or "M+1" => "110111",
                "D-1" => "001110",
                "A-1" or "M-1" => "110010",
                "D+A" or "D+M" => "000010",
                "D-A" or "D-M" => "010011",
                "A-D" or "M-D" => "000111",
                "D&A" or "D&M" => "000000",
                "D|A" or "D|M" => "010101",
                _ => "101010",
            };
        }

        //Returns the binary code of the jump mnemonic
        public static string Jump(string mnemonic)
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
