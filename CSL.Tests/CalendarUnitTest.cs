using System.Collections;
using Antlr4.Runtime;
using CSL.TypeChecker;

namespace CSL.Tests;

using CSL.SubTypes;


[TestFixture]
public class CalendarUnitTest
{
    public static IEnumerable EventTestCases
    {
        get
        {
            yield return new TestCaseData(
                "1mth ++ 'abc'",
                new Event(Subject: new Subject("abc"), Duration: new Duration(0, 1)));
        }
    }
    
    public static IEnumerable CalendarTestCases
    {
        get
        {
            yield return new TestCaseData(
                "'abc' || 'def'",
                new Calendar ([
                    new (Subject: new Subject("abc")), new (Subject: new Subject("def"))
                ]));
        }
    }
    
    [TestCaseSource(nameof(EventTestCases))]
    public void TestEventOperations(string input, Event expectedResult)
    {
        var calendarVisitor = new CalendarVisitor();
        var expr = calendarVisitor.Visit(Parse(input));
        
        Assert.That(expr, Is.Not.Null);
        Assert.That((Event)expr, Is.EqualTo(expectedResult));
    }
    
    [TestCaseSource(nameof(CalendarTestCases))]
    public void TestCalendarOperations(string input, Calendar expectedResult)
    {
        var calendarVisitor = new CalendarVisitor();
        var expr = calendarVisitor.Visit(Parse(input));

        Assert.That(expr, Is.Not.Null);
        for (int i = 0; i < expectedResult.Events.Length; i++)
        {
            Assert.That(expr.Events[i], Is.EqualTo(expectedResult.Events[i]));
        }
    }

    private static CSLParser.ProgContext Parse(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        return parser.prog();
    }
}