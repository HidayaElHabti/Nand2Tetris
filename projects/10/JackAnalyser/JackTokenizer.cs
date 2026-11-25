namespace JackAnalyser
{
    /*
     * This module ignores all comments and white space in the input stream and
     * enables accessing the input one token at a time. Also, it parses and provides
     * the type of each token, as defined by the Jack grammar.
     */
    class JackTokenizer
    {
        //Opens the input .jack file/stream and gets ready to tokenize it.
        public JackTokenizer(string inputFile)
        {

        }

        //Are there more tokens in the input?
        public bool HasMoreTokens()
        {
            return false;
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
