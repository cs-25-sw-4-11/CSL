using Antlr4.Runtime;
using CSL.TypeChecker;

namespace CSL.Tests;

[TestFixture]
public class CalendarUnitTest
{
    public static Event testResult = new Event(Subject: new Subject("abc"), Duration: new Duration(0, 1));
    
    [TestCase("1mth ++ 'abc'")]
    public void TestEventConcat(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var calendarVisitor = new CalendarVisitor();
        
        var expr = calendarVisitor.Visit(tree);
        
        Assert.That(expr, Is.Not.Null);
        Assert.That((Event)expr, Is.EqualTo(testResult));
    }
    
    [TestCase("'abc' || 'def'")]
    public void TestUnionOp(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var calendarVisitor = new CalendarVisitor();
        
        var expr = calendarVisitor.Visit(tree);

        Calendar expectedResult = new ([new (Subject: new Subject("abc")), new (Subject: new Subject("def"))]);
        
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.EqualTo(expectedResult.Events[0]));
        Assert.That(expr.Events[1], Is.EqualTo(expectedResult.Events[1]));
    }
}