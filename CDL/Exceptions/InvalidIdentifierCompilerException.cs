namespace CDL.Exceptions;

public class InvalidIdentifierCompilerException : CompilerException
{
    public InvalidIdentifierCompilerException()
    {
    }

    public InvalidIdentifierCompilerException(string message) : base(message)
    {
    }

    public InvalidIdentifierCompilerException(string message, Exception inner) : base(message, inner)
    {
    }
}