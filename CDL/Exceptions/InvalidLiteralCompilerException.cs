using Antlr4.Runtime;

namespace CDL.Exceptions;

public class InvalidLiteralCompilerException : CompilerException
{
    public InvalidLiteralCompilerException()
    {
    }

    public InvalidLiteralCompilerException(ParserRuleContext context) : base(context)
    {
    }

    public InvalidLiteralCompilerException(string message) : base(message)
    {
    }

    public InvalidLiteralCompilerException(string message, Exception inner) : base(message, inner)
    {
    }
}
