using Antlr4.Runtime;

namespace CDL.Exceptions;

public class InvalidIdentifierCompilerException : CompilerException
{
    public InvalidIdentifierCompilerException()
    {
    }

    public InvalidIdentifierCompilerException(ParserRuleContext context) : base(context)
    {
    }

    public InvalidIdentifierCompilerException(string message) : base(message)
    {
    }

    public InvalidIdentifierCompilerException(string message, Exception inner) : base(message, inner)
    {
    }
}