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
    [TestCase("'abc' * 1d")] // RecurrenceOp
    [TestCase("10:00~11:00")] // TildeOp
    [TestCase("01/01/2001~01/02/2001")] // TildeOp
    [TestCase("01/01/2001~10:00")] // TildeOp
    [TestCase("2h - 1h")] //SubtractOp
    [TestCase("02/01/2001 - 1 d")] //SubtractOp
    [TestCase("3h ++ 'abc'")] // DoublePlusOp

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