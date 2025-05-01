namespace CSL.Tests;

using NUnit.Framework;
using CSL;

[TestFixture]
public class TestClass
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        CSLCustomVisitor visitor = new CSLCustomVisitor();
        
        Assert.That("abc", Is.SameAs("abc"), "abc is equal to abc");
    }
}