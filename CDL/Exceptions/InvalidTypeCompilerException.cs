namespace CDL.Exceptions;

using CDL.TypeChecker;

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

    public InvalidTypeCompilerException(TypeChecker.EventTypes[] expected, TypeChecker.EventTypes actual) : base($"Expected: {string.Join(", ", expected)}, found: {actual}")
    {
    }
}