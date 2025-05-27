using Antlr4.Runtime;
using System.IO;
using CDL.Exceptions;

namespace CDL
{
    public class CSLParserErrorListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            // Immediately throw our custom exception for any parser error
            throw new InvalidLiteralCompilerException($"Parser error at line {line}:{charPositionInLine} - {msg}");
        }
    }
}