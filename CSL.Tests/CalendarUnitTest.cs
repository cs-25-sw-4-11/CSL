using System.Collections;
using Antlr4.Runtime;
using CSL.TypeChecker;

namespace CSL.Tests;

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
    
    public static IEnumerable TestPlusCases
    {
        get
        {
            yield return new TestCaseData("1mth + 2mth", new Event(Duration: new Duration(0, 3)));
            yield return new TestCaseData("10min + 20min", new Event(Duration: new Duration(30, 0)));
            yield return new TestCaseData("1y + 3h", new Event(Duration: new Duration(180, 12)));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3h", new Event(new DateClock(date: new Date(1,1,2000),clock: new Clock(16,0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3d", new Event(new DateClock(date: new Date(4,1,2000),clock: new Clock(13,0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 12h", new Event(new DateClock(date: new Date(2,1,2000),clock: new Clock(1,0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3h + 3d", new Event(new DateClock(date: new Date(4,1,2000),clock: new Clock(16,0))));
            yield return new TestCaseData("13:00 + 3h", new Event(Clock: new Clock(16,0)));
            yield return new TestCaseData("13:00 + 3d", new Event(Clock: new Clock(13,0)));
            yield return new TestCaseData("13:00 + 12h", new Event(Clock: new Clock(1,0)));
            yield return new TestCaseData("13:00 + 3d + 3h", new Event(Clock: new Clock(16,0)));
            yield return new TestCaseData("01/01/2000 + 3d", new Event(Date: new Date(4,1,2000)));
            yield return new TestCaseData("01/01/2000 + 24h", new Event(Date: new Date(2,1,2000)));
            yield return new TestCaseData("01/01/2000 + 3h", new Event(new DateClock(date: new Date(1,1,2000),clock: new Clock(3,0))));
            yield return new TestCaseData("01/01/2000 + 25h", new Event(new DateClock(date: new Date(2,1,2000),clock: new Clock(1,0))));
            yield return new TestCaseData("01/01/2000 + 3h + 3d", new Event(new DateClock(date: new Date(4,1,2000),clock: new Clock(3,0))));
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


    public static IEnumerable TestNotPlusCases
    {
        get
        {
            yield return new TestCaseData("13:00 + 3d", new Event(new DateClock(date: new Date(0,0,0),clock: new Clock(13,0))));
            yield return new TestCaseData("01/01/2000 + 3d", new Event(new DateClock(date: new Date(1,1,2000),clock: new Clock(0,0))));
        }
    }

    [TestCaseSource(nameof(TestNotPlusCases))]
    public void TestNotPlusOp(string input, Event expectedResult)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();
        var calendarVisitor = new CalendarVisitor();
        
        var expr = calendarVisitor.Visit(tree);
        
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.Not.EqualTo(expectedResult));
    }
}