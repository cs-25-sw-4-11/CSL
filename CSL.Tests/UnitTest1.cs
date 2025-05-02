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
    [TestCase("5 hours", 5 * Duration.MinuteFactor)]
    [TestCase("2 days", 2 * Duration.DayFactor)]
    [TestCase("3 weeks", 10 * Duration.WeekFactor)]
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

    [Test]
    public void TestLiteralMinutesSimple()
    {
    }
}