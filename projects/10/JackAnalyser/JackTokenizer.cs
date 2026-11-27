using System.Text.RegularExpressions;

namespace JackAnalyser
{
    /*
     * This module ignores all comments and white space in the input stream and
     * enables accessing the input one token at a time. Also, it parses and provides
     * the type of each token, as defined by the Jack grammar.
     */
    class JackTokenizer
    {
        private readonly static string RE_SYMBOL = @"\{|\}|\(|\)|\[|\]|\.|,|;|\+|-|\*|/|&|\||\<|\>|=|~";
        private readonly static string RE_STRING = @"""[^""]*""";
        private readonly static string RE_SPLIT = "(" + RE_SYMBOL + "|" + RE_STRING + @")|\s+";
        private string file;
        private List<string> tokens;
        string? currentToken;
        //Receives the input .jack file as a string and gets ready to tokenize it.
        public JackTokenizer(string inputFile)
        {
            file = inputFile;
            Advance();
        }

        //Are there more tokens in the input?
        public bool HasMoreTokens()
        {
            return currentToken != null;
        }

        //Gets the next token from the input and makes it the current token.
        //Called only if HasMoreToken is true.
        public void Advance()
        {
            
        }

        //Returns the type of the current token
        public TokenType GetTokenType()
        {
            return TokenType.KEYWORD;
        }

        //Returns the keyword which is the current token
        //TokenType=KEYWORD
        public Keyword GetKeyword()
        {
            return Keyword.BOOLEAN;
        }

        //Returns the character which is the current token
        //TokenType=SYMBOL
        public char GetSymbol()
        {
            return '{';
        }

        //Returns the string which is the current token
        //TokenType=IDENTIFIER
        public string GetIdentifier()
        {
            return "hi";
        }

        //Returns the integer value of current token
        //TokenType=INT_CONST
        public int GetIntVal()
        {
            return 0;
        }

        //Returns the string value of the current token
        //TokenType=STRING_CONST
        public string GetStringVal()
        {
            return "hi";
        }

        #region Private
        private List<string> TokenizeFile(string file)
        {
            var split_code = Regex.Split(file, RE_SPLIT);
            var tokens = new List<string>();

            foreach(var expression in split_code)
            {
                if(expression != null || !Regex.IsMatch(expression, @"^\s*$"))
                {
                    tokens.Add(expression);
                }
            }
            return tokens;
        }
        #endregion
    }

    enum TokenType
    {
        KEYWORD,
        SYMBOL,
        IDENTIFIER,
        INT_CONST,
        STRING_CONST
    }

    enum Keyword
    {
        CLASS,
        METHOD,
        FUNCTION,
        CONSTRUCTOR,
        INT,
        BOOLEAN,
        CHAR,
        VOID,
        VAR,
        STATIC,
        FIELD,
        LET,
        DO,
        IF,
        ELSE,
        WHILE,
        RETURN,
        TRUE,
        FALSE,
        NULL,
        THIS
    }
}
