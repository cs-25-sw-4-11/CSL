namespace CSL.Exceptions;

public class InvalidTypeCompilerException : CompilerException
{
    public InvalidTypeCompilerException()
    {
    }

    public InvalidTypeCompilerException(string message) : base(message)
    {
    }

    public InvalidTypeCompilerException(string message, Exception inner) : base(message, inner)
    {
    }

    public InvalidTypeCompilerException(EventTypes expected, EventTypes actual) : base($"Expected: {expected}, found: {actual}")
    {
    }
}