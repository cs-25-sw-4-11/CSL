namespace CDL.Tests.Literals;

using CDL;
using CDL.Exceptions;
using NUnit.Framework;

[TestFixture]
public class SubjectTests
{
    [TestCase("'Simple subject'", "Simple subject")]
    [TestCase("'This is a test'", "This is a test")]
    [TestCase("'Subject with numbers 123'", "Subject with numbers 123")]
    [TestCase("'Special characters: !@#$%^&*()'", "Special characters: !@#$%^&*()")]
    public void TestLiteralSubject(string input, string expectedText)
    {
        var subject = SubjectParser.ParseSubject(input);

        Assert.That(subject, Is.Not.Null);
        Assert.That(subject.Value.Text, Is.EqualTo(expectedText));
    }

    [TestCase("Subject without quotes")]
    [TestCase("'Unclosed quote")]
    [TestCase("'Contains \\'backslash'")]
    [TestCase("\"Wrong quote type\"")]
    [TestCase("")]
    [TestCase("''")]
    public void TestInvalidSubjectLiterals(string input)
    {
        Assert.Throws<InvalidLiteralCompilerException>(() => SubjectParser.ParseSubject(input));
    }
}