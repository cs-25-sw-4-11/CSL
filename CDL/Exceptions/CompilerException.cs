using Antlr4.Runtime;

namespace CDL.Exceptions;

public class CompilerException : Exception
{
    public ParserRuleContext? Context { init; get; }

    public CompilerException()
    {
    }
    
    public CompilerException(ParserRuleContext context)
    {
        Context = context;
    }

    public CompilerException(string message) : base(message)
    {
    }
    
    public CompilerException(string message, Exception inner) : base(message, inner)
    {
    }
}