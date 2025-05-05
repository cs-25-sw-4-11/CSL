namespace CSL.Tests;

using CSL;
using CSL.Exceptions;
using NUnit.Framework;
using Antlr4.Runtime;
using System;
using System.IO;

[TestFixture]
public class ClockTests
{
    [TestCase("15:30", 15, 30)]
    [TestCase("00:00", 0, 0)]
    [TestCase("23:59", 23, 59)]
    public void TestLiteralClock(string input, int hours, int minutes)
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
        Assert.That(result.Hours, Is.EqualTo(hours));
        Assert.That(result.Minutes, Is.EqualTo(minutes));
    }
    
    [TestCase("24:00")] // Hour too large
    [TestCase("-1:30")] // Negative hour
    [TestCase("12:60")] // Minute too large
    [TestCase("12:-5")] // Negative minute
    [TestCase("a:30")]  // Non-numeric hour
    [TestCase("12:b")]  // Non-numeric minute
    [TestCase("12-30")] // Wrong separator
    public void TestInvalidClockLiterals(string input)
    {
        var stream = CharStreams.fromString(input);
        var lexer = new CSLLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new CSLParser(tokens);
        var tree = parser.prog();
        
        // Create and use the visitor
        var visitor = new ClockVisitor();
        
        Assert.Throws<InvalidLiteralCompilerException>(() => visitor.Visit(tree));
    }
}   