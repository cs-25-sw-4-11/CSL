using System.Collections;
using Antlr4.Runtime;
using CSL.TypeChecker;
using CSL.Exceptions;

namespace CSL.Tests;

[TestFixture]
public class CalendarVisitorTest
{
    public static IEnumerable CalendarTestCases
    {
        get
        {

            yield return new TestCaseData(
                "1mth ++ 'abc'",
                new Calendar([
                    new Event(Subject: new Subject("abc"), Duration: new Duration(0, 1))
                ])
            );
            yield return new TestCaseData(
                "mikkel = 'fødsel'; mikkel 'er dum'",
                new Calendar([
                    new Event(Subject: "er dum")
                ])
            );
            yield return new TestCaseData(
                "trussel = 'Rådhus💥💥'; trussel ++ 25/06/2025",
                new Calendar([
                    new Event(Subject: "Rådhus💥💥", Date: new Date(25, 6, 2025))
                ])
            );
            yield return new TestCaseData(
                "mikkel = 'fødsel'; mikkel",
                new Calendar([
                    new Event(Subject: "fødsel")
                ])
            );
            yield return new TestCaseData(
                "'abc' || 'def'",
                new Calendar([
                    new (Subject: new Subject("abc")),
                    new (Subject: new Subject("def"))
                ]));
            yield return new TestCaseData(
                "'abc' || 16:00",
                new Calendar([
                    new (Subject: new Subject("abc")), new (Clock: new Clock(16,00))
                ]));
            yield return new TestCaseData(
                "'abc' || 'def' ++ 16:00",
                new Calendar([
                    new (Subject: new Subject("abc")),
                    new (Subject: new Subject("def"), Clock: new Clock(16,00))
                ]));
            yield return new TestCaseData(
                "calendar1 = 'abc' || 'def'; calendar1 ++ 16:00",
                new Calendar([
                    new (Subject: new Subject("abc"), Clock: new Clock(16,00)),
                    new (Subject: new Subject("def"), Clock: new Clock(16,00))
                ])
            );
            yield return new TestCaseData(
                "calendar1 = 'abc' || 'def'; calendar1 ++ 16:00",
                new Calendar([
                    new (Subject: new Subject("abc"), Clock: new Clock(16,00)),
                    new (Subject: new Subject("def"), Clock: new Clock(16,00))
                ])
            );
            yield return new TestCaseData(
                "('abc' || 'def') ++ 16:00",
                new Calendar([
                    new (Subject: new Subject("abc"), Clock: new Clock(16,00)),
                    new (Subject: new Subject("def"), Clock: new Clock(16,00))
                ])
            );
            yield return new TestCaseData(
                "('abc' || 'def') ++ 16:00 + 3h",
                new Calendar([
                    new (Subject: new Subject("abc"), Clock: new Clock(19,00)),
                    new (Subject: new Subject("def"), Clock: new Clock(19,00))
                ])
            );
            yield return new TestCaseData(
                "(10:00 || 13/3/2032) ++ 3h",
                new Calendar([
                    new (Clock: new Clock(10,0), Duration: new Duration(180, 0)),
                    new (Date: new Date(13, 3, 2032), Duration: new Duration(180, 0))
                ])
            );
            yield return new TestCaseData(
                "(10:00 || 13/3/2032) + 3d",
                new Calendar([
                    new (Clock: new Clock(10,0)),
                    new (Date: new Date(16,3,2032))
                ])
            );
            yield return new TestCaseData(
                "(10:00 || 13/3/2032) - 3h",
                new Calendar([
                    new (Clock: new Clock(7,0)),
                    new (dateClock: new DateClock(new(12,3,2032), new(21,0)))
                ])
            );
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
            yield return new TestCaseData("\"abc\" + 16:00");
            yield return new TestCaseData("\"abc\" + 01/01/2001");
        }
    }

    [TestCaseSource(nameof(CalendarTestCases))]
    public void TestCalendarOperations(string input, Calendar expectedResult)
    {
        var calendarVisitor = new CalendarVisitor();
        var expr = calendarVisitor.Visit(StringParser.ParseString(input));

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

        Assert.Throws<ArgumentException>(() =>
        {
            var expr = calendarVisitor.Visit(StringParser.ParseString(input));
        });
    }


    public static IEnumerable TestPlusCases
    {
        get
        {
            yield return new TestCaseData("1mth + 2mth", new Event(Duration: new Duration(0, 3)));
            yield return new TestCaseData("10min + 20min", new Event(Duration: new Duration(30, 0)));
            yield return new TestCaseData("1y + 3h", new Event(Duration: new Duration(180, 12)));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(16, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(13, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 12h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(1, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 + 3h + 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(16, 0))));
            yield return new TestCaseData("13:00 + 3h", new Event(Clock: new Clock(16, 0)));
            yield return new TestCaseData("13:00 + 3d", new Event(Clock: new Clock(13, 0)));
            yield return new TestCaseData("13:00 + 12h", new Event(Clock: new Clock(1, 0)));
            yield return new TestCaseData("13:00 + 3d + 3h", new Event(Clock: new Clock(16, 0)));
            yield return new TestCaseData("01/01/2000 + 3d", new Event(Date: new Date(4, 1, 2000)));
            yield return new TestCaseData("01/01/2000 + 24h", new Event(Date: new Date(2, 1, 2000)));
            yield return new TestCaseData("01/01/2000 + 3h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(3, 0))));
            yield return new TestCaseData("01/01/2000 + 25h",
                new Event(new DateClock(new Date(2, 1, 2000), new Clock(1, 0))));
            yield return new TestCaseData("01/01/2000 + 3h + 3d",
                new Event(new DateClock(new Date(4, 1, 2000), new Clock(3, 0))));
            // Add more cases as needed
        }
    }

    [TestCaseSource(nameof(TestPlusCases))]
    public void TestPlusOp(string input, Event expectedResult)
    {

        var calendarVisitor = new CalendarVisitor();

        var expr = calendarVisitor.Visit(StringParser.ParseString(input));

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.EqualTo(expectedResult));
    }


    public static IEnumerable TestNotPlusCases
    {
        get
        {
            yield return new TestCaseData("13:00 + 3d", new Event(new DateClock(new Date(0, 0, 0), new Clock(13, 0))));
            yield return new TestCaseData("01/01/2000 + 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(0, 0))));
        }
    }

    [TestCaseSource(nameof(TestNotPlusCases))]
    public void TestNotPlusOp(string input, Event expectedResult)
    {

        var calendarVisitor = new CalendarVisitor();

        var expr = calendarVisitor.Visit(StringParser.ParseString(input));

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.Not.EqualTo(expectedResult));
    }


    public static IEnumerable TestMinusCases
    {
        get
        {
            yield return new TestCaseData("2mth - 1mth", new Event(Duration: new Duration(0, 1)));
            yield return new TestCaseData("20min - 10min", new Event(Duration: new Duration(10, 0)));
            yield return new TestCaseData("1y - 3mth", new Event(Duration: new Duration(0, 9)));
            yield return new TestCaseData("01/01/2000 ++ 13:00 - 3h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(10, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 - 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(13, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 - 12h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(1, 0))));
            yield return new TestCaseData("01/01/2000 ++ 13:00 - 3h - 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(10, 0))));
            yield return new TestCaseData("19:00 - 3h", new Event(Clock: new Clock(16, 0)));
            yield return new TestCaseData("13:00 - 3d", new Event(Clock: new Clock(13, 0)));
            yield return new TestCaseData("13:00 - 12h", new Event(Clock: new Clock(1, 0)));
            yield return new TestCaseData("01:00 - 3d - 3h", new Event(Clock: new Clock(22, 0)));
            yield return new TestCaseData("01/01/2000 - 3d", new Event(Date: new Date(29, 12, 1999)));
            yield return new TestCaseData("01/01/2000 - 24h", new Event(Date: new Date(31, 12, 1999)));
            yield return new TestCaseData("02/01/2000 - 3h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(21, 0))));
            yield return new TestCaseData("03/01/2000 - 25h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(23, 0))));
            yield return new TestCaseData("05/01/2000 - 3h - 3d",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(21, 0))));
            yield return new TestCaseData("1/3/2313 - 17mth", new Event(Date: new Date(1, 10, 2311)));
            yield return new TestCaseData("1/3/2313 - 36500d", new Event(Date: new Date(25, 3, 2213)));
            yield return new TestCaseData("13:00 + 3h", new Event(Clock: new Clock(16, 0)));
            yield return new TestCaseData("13:00 + 3h", new Event(Clock: new Clock(16, 0)));
            yield return new TestCaseData("1mth - 3d", new Event(Duration: new Duration(-4320, 1)));
        }
    }
    [TestCaseSource(nameof(TestMinusCases))]
    public void TestMinusOp(string input, Event expectedResult)
    {
        var calendarVisitor = new CalendarVisitor();
        var expr = calendarVisitor.Visit(StringParser.ParseString(input));

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.EqualTo(expectedResult));
    }


    public static IEnumerable TestNotMinusCases
    {
        get
        {
            yield return new TestCaseData("1d - 3d");
            yield return new TestCaseData("1min - 3d");
            yield return new TestCaseData("1d - 3w");
            yield return new TestCaseData("2w - 3w");
            yield return new TestCaseData("1mth - 3mth");
            yield return new TestCaseData("1y - 3y");
        }
    }
    [TestCaseSource(nameof(TestNotMinusCases))]
    public void TestExpectErrorMinusOp(string input)
    {
        var calendarVisitor = new CalendarVisitor();

        Assert.Throws<ArgumentException>(() => calendarVisitor.Visit(StringParser.ParseString(input)));
    }

    public static IEnumerable TestParentersesCases
    {
        get
        {
            yield return new TestCaseData("(01/01/2000 ++ 13:00) - 3h",
                new Event(new DateClock(new Date(1, 1, 2000), new Clock(10, 0))));
            yield return new TestCaseData("(01/01/2000 ++ 13:00) - 3d",
                new Event(new DateClock(new Date(30, 12, 1999), new Clock(13, 0))));
            yield return new TestCaseData("(03/01/2000 ++ 12:00) - 12h",
                new Event(new DateClock(new Date(3, 1, 2000), new Clock(0, 0))));
            yield return new TestCaseData("(02/07/2000 ++ 13:00) - 3h - 3d",
                new Event(new DateClock(new Date(30, 6, 2000), new Clock(10, 0))));
            yield return new TestCaseData("19:00 - (3h + 3h)", new Event(Clock: new Clock(13, 0)));
            yield return new TestCaseData("05/01/2000 - (3d - 3h)",
                new Event(new DateClock(new Date(2, 1, 2000), new Clock(3, 0))));
            yield return new TestCaseData("12:00 + (3d - 3d)", new Event(Clock: new Clock(12, 0)));
            
            }
    }
    [TestCaseSource(nameof(TestParentersesCases))]
    public void TestParenterses(string input, Event expectedResult)
    {
        var calendarVisitor = new CalendarVisitor();
        var expr = calendarVisitor.Visit(StringParser.ParseString(input));

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Events[0], Is.EqualTo(expectedResult));
    }
}