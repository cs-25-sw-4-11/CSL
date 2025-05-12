using System.Collections;
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
    
    public static IEnumerable TestPlusCases
    {
        get
        {
            yield return new TestCaseData("1mth + 2mth", new Event(Duration: new Duration(0, 3)));
            yield return new TestCaseData("10min + 20min", new Event(Duration: new Duration(30, 0)));
            yield return new TestCaseData("1y + 3h", new Event(Duration: new Duration(180, 12)));
            
            // Add more cases as needed
        }
    }
    
    [TestCaseSource(nameof(TestPlusCases))]
    public void TestPlusOp(string input, Event expectedResult)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var calendarVisitor = new CalendarVisitor();
        
        var expr = calendarVisitor.Visit(tree);
        
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.EqualTo(expectedResult));
    }
}