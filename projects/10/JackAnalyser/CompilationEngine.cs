namespace JackAnalyser
{
    /*
     * This module emits a structured representation of 
     * the input source code wrapped in XML tags.
     * */
    class CompilationEngine
    {
        public CompilationEngine(StreamReader streamReader, StreamWriter streamWriter)
        {

        }

        //Compiles a complete class
        public void CompileClass()
        {

        }
        //Compiles a static variable declaration, or a field declaration
        public void CompileClassVarDec()
        {

        }
        //Compiles a complete method, function, or constructor
        public void CompileSubroutine()
        {

        }
        //Compiles a (possibly empty) parameter list.
        //Does not handle the enclosing parentheses tokens ( and ).
        public void CompileParameterList() { }
        //Compiles a subroutine's body
        public void CompileSubroutineBody() { }
        //Compiles a var declaration
        public void CompileVarDec() { }
        //Compiles a sequence of statements.
        //Does not handle the enclosing curly bracket tokens { and }.
        public void CompileStatements() { }
        //Compiles a let statement
        public void CompileLet() { }
        //Compiles an if statement, possibly with a trailing else clause.
        public void CompileIf() { }
        //Compiles a while statement
        public void CompileWhile() { }
        //Compiles a do statement
        public void CompileDo() { }
        //Compiles a return statement
        public void CompileReturn() { }
        //Compiles an expression
        public void CompileExpression() { }
        //Compiles a term. If the current token is identifier, the routine must resolve
        //it into a variable, an array element or a subroutine call.
        public void CompileTerm() { }
        //Compiles a (possibly empty) comma-seperated list of expressions.
        //Returns the number of expressions in the list.
        public int CompileExpressionList()
        {
            return 0;
        }
    }
}
