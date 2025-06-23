using Antlr4.Runtime;

namespace CDL.Exceptions;

using TypeChecker;

public class InvalidTypeCompilerException : CompilerException
{
    public EventTypes[] Expected { init; get; }
    public EventTypes Actual { init; get; }
    
    public InvalidTypeCompilerException()
    {
    }

    public InvalidTypeCompilerException(ParserRuleContext context) : base(context)
    {
    }

    public InvalidTypeCompilerException(string message) : base(message)
    {
    }

    public InvalidTypeCompilerException(string message, Exception inner) : base(message, inner)
    {
    }

    public InvalidTypeCompilerException(EventTypes[] expected, EventTypes actual, ParserRuleContext context) : base(context)
    {
        Expected = expected;
        Actual = actual;
    }

    public override string ToString()
    {
        return $"Expected: {string.Join(", ", Expected)}, found: {Actual}";
    }
}