namespace CSL.Tests;

using CSL;
using NUnit.Framework;
using Antlr4.Runtime;
using System;
using System.IO;

[TestFixture]
public class TestDuration
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("10 min", 10 * Duration.MinuteFactor)]
    [TestCase("5 hr", 5 * Duration.MinuteFactor)]
    [TestCase("2 d", 2 * Duration.DayFactor)]
    [TestCase("3 w", 10 * Duration.WeekFactor)]
    public void TestLiteralsMinuteParam(string text, int minutes)
    {
        var stream = CharStreams.fromString(text);

        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();

        // Create and use the visitor
        var visitor = new DurationVisitor();
        var result = visitor.Visit(tree);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Minutes, Is.EqualTo(minutes));
    }

    [TestCase("2 mth", 2 * Duration.MonthFactor)]
    [TestCase("1 y", 1 * Duration.YearFactor)]
    public void TestLiteralMonthParam()
    {
        var stream = CharStreams.fromString(text);

        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();

        // Create and use the visitor
        var visitor = new DurationVisitor();
        var result = visitor.Visit(tree);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Months, Is.EqualTo(months));
    }
}