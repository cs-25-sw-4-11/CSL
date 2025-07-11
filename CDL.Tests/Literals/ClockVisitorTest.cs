namespace CDL.Tests.Literals;

using CDL;
using CDL.Exceptions;
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
        var clock = ClockParser.ParseClock(input);
        
        Assert.That(clock, Is.Not.Null);
        Assert.That(clock.Value.Hours, Is.EqualTo(hours));
        Assert.That(clock.Value.Minutes, Is.EqualTo(minutes));
    }
    
    [TestCase("24:00")] // Hour too large
    [TestCase("-1:30")] // Negative hour
    [TestCase("12:60")] // Minute too large
    [TestCase("12:-5")] // Negative minute
    [TestCase("aa:30")]  // Non-numeric hour
    [TestCase("12:bb")]  // Non-numeric minute
    [TestCase("12+30")] // Wrong separator
    [TestCase("50/30")] 
    [TestCase("!12::30")] 
    [TestCase("what the sigma")]
    [TestCase("12-30")]
    [TestCase(":30")]  // Missing hour
    [TestCase("12:")]  // Missing minutes
    public void TestInvalidClockLiterals(string input)
    {
        Assert.Throws<InvalidLiteralCompilerException>(() => ClockParser.ParseClock(input));
    }
}