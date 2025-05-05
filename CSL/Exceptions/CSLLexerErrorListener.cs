using Antlr4.Runtime;
using CSL.Exceptions;
using System.IO;

namespace CSL
{
    public class CSLLexerErrorListener : IAntlrErrorListener<int>
    {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            // Immediately throw our custom exception for any lexer error
            throw new InvalidLiteralCompilerException($"Lexer error at line {line}:{charPositionInLine} - {msg}");
        }
    }
}