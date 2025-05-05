namespace CSL.Tests;

using CSL;
using NUnit.Framework;
using Antlr4.Runtime;
using System;
using System.IO;

[TestFixture]

public class TestClock
{
    [TestCase("15:30", 15, 30)]


    public void TestLiteralClock(string input, int hour, int minute)
    {
         var stream = CharStreams.fromString(input);

        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);

        var tree = parser.prog();

        // Create and use the visitor
        var visitor = new ClockVisitor();
        var result = visitor.Visit(tree);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Hours, Is.EqualTo(hour));
        Assert.That(result.Minutes, Is.EqualTo(minute));
    }
}