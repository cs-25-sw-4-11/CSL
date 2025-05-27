namespace CDL.Tests.Literals;

using CDL;
using CDL.Exceptions;
using NUnit.Framework;
using Antlr4.Runtime;
using System;
using System.IO;

[TestFixture]
public class DescriptionTests
{
    [TestCase("\"Simple description\"", "Simple description")]
    [TestCase("\"This is a test\"", "This is a test")]
    [TestCase("\"Description with numbers 123\"", "Description with numbers 123")]
    [TestCase("\"Special characters: !@#$%^&*()\"", "Special characters: !@#$%^&*()")]
    public void TestLiteralDescription(string input, string expectedText)
    {
        var description = DescriptionParser.ParseDescription(input);

        Assert.That(description, Is.Not.Null);
        Assert.That(description.Value.Text, Is.EqualTo(expectedText));
    }

    [TestCase("Description without quotes")]
    [TestCase("\"Unclosed quote")]
    [TestCase("\"Contains \\backslash\"")]
    [TestCase("\'Wrong quote type\'")]
    [TestCase("")]
    public void TestInvalidDescriptionLiterals(string input)
    {
        Assert.Throws<InvalidLiteralCompilerException>(() => DescriptionParser.ParseDescription(input));
    }
}