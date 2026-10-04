using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    internal class SymbolTable
    {
        private Dictionary<string, int> symbols;

        public SymbolTable()
        {
            symbols = [];
        }

        public void InitializeTable()
        {
            for (int i = 0; i <= 15; i++)
            {
                AddEntry("R" + i, i);
            }

            AddEntry("SP", 0);
            AddEntry("LCL", 1);
            AddEntry("ARG", 2);
            AddEntry("THIS", 3);
            AddEntry("THAT", 4);
            AddEntry("SCREEN", 16384);
            AddEntry("KBD", 24576);
        }

        public void AddEntry(string symbol, int address)
        {
            symbols[symbol] = address;
        }

        public bool Contains(string symbol)
        {
            return symbols.ContainsKey(symbol);
        }

        public int GetAddress(string symbol)
        {
            return symbols.GetValueOrDefault(symbol);
        }
    }
}
