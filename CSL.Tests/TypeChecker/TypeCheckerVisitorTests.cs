using Antlr4.Runtime;
using CSL.Exceptions;
using CSL.TypeChecker;

namespace CSL.Tests.TypeChecker;


[TestFixture]
public class TypeCheckerVisitorTests
{
    [TestCase("1d")]
    [TestCase("1d + 1d")]
    [TestCase("1d + 1min")]
    [TestCase("1d + 01/01/2001")]
    [TestCase("1d + 10:00")]
    [TestCase("1h + 10:00")]
    [TestCase("10:00~11:00")] // TildeOp
    [TestCase("01/01/2001~01/02/2001")] // TildeOp
    [TestCase("01/01/2001~10:00")] // TildeOp
    [TestCase("2h - 1h")] //SubtractOp
    [TestCase("02/01/2001 - 1 d")] //SubtractOp
    [TestCase("3h ++ 'abc'")] // DoublePlusOp
    [TestCase("(\"abc\" || \"cba\") + 1h")] // AddOp calendar + duration
    [TestCase("(\"abc\" || \"cba\") - 1h")] // SubtractOp calendar - duration
    [TestCase("(\"abc\" || \"cba\") * 1h ")]
    [TestCase("1h * (\"abc\" || \"cba\")")]
    [TestCase("08/08/2002 * 1 y")]
    [TestCase("1 y * 08/08/2002")]
    

    public void TestTypeExpressionsValid(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var typeVisitor = new TypeCheckerVisitor();
        
        Assert.DoesNotThrow(() => typeVisitor.Visit(tree));
    }

    [TestCase("'abc' + 1d")] // AddOp
    [TestCase("01/01/2001 + 01/01/2001")] // AddOp
    [TestCase("01/01/2001~'abc'")] // TildeOp
    [TestCase("3d - 01/01/2001")] // SubtractOp
    [TestCase("'abc' - 3h")] // SubtractOp
    [TestCase("3h ++ 3h")] // DoublePlusOp
    [TestCase("(\"abc\" || \"cba\") + 12:30")] // AddOp calendar + event without duration
    [TestCase("(\"abc\" || \"cba\") + (\"abc\" || \"cba\")")] // AddOp celendar + calendar
    [TestCase("(\"abc\" || \"cba\") - (\"abc\" || \"cba\")")]
    [TestCase("1 h - (\"abc\" || \"cba\")")]
    [TestCase("(\"abc\" || \"cba\") * (\"abc\" || \"cba\")")]
    [TestCase("(\"abc\" || \"cba\") * 13:30")]
    [TestCase("13:30 * (\"abc\" || \"cba\")")]
    [TestCase("1h * \"abc\" ++ 3h")]


 
    public void TestTypeExpressionsInvalid(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var typeVisitor = new TypeCheckerVisitor();
        
        Assert.Throws<InvalidTypeCompilerException>(() => typeVisitor.Visit(tree));
    }
}