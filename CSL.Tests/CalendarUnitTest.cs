using System.Collections;
using Antlr4.Runtime;
using CSL.TypeChecker;
using CSL.Exceptions;

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

                 yield return new TestCaseData(
                "1y ++ \"abc\"",
                new Event(Description: new Description("abc"), Duration: new Duration(0, 12)));    

                yield return new TestCaseData(
                "'Workshop' ++ 06/12/2023 ++ 09:30 ++ 3h ++ \"Annual planning session\"",
                new Event(
                    Subject: new Subject("Workshop"),
                    Date: new Date(6, 12, 2023),
                    Clock: new Clock(9, 30),
                    Duration: new Duration(180, 0),
                    Description: new Description("Annual planning session")));
        }
    }
    
    public static IEnumerable CalendarTestCases
    {
        get
        {
            yield return new TestCaseData(
                "'abc' || 'def'",
                new Calendar ([
                    new (Subject: new Subject("abc")),
                    new (Subject: new Subject("def"))
                ]));
            yield return new TestCaseData(
                "'abc' || 16:00",
                new Calendar ([
                    new (Subject: new Subject("abc")), new (Clock: new Clock(16,00))
                ]));
            yield return new TestCaseData(
                "'abc' || 'def' ++ 16:00",
                new Calendar ([
                    new (Subject: new Subject("abc")),
                    new (Subject: new Subject("def"), Clock: new Clock(16,00))
                ]));   
        }
    }

    public static IEnumerable InvalidEventTestCases
    {
        get
        {
            yield return new TestCaseData("16:00 ++ 12:00");
            yield return new TestCaseData("'abc' ++ 16:00 ++ 12:00");
            yield return new TestCaseData("'abc' ++ 'def'");
            yield return new TestCaseData("'abc' ++ 01/01/2001 ++ 01/01/2001");
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

    [TestCaseSource(nameof(InvalidEventTestCases))]
    public void TestInvalidEventOperations(string input)
    {
        var calendarVisitor = new CalendarVisitor();
    
        Assert.Throws<ArgumentException>(() => {
            var expr = calendarVisitor.Visit(Parse(input));
        });
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