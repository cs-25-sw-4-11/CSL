using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;

namespace CSL.Tests;

using CSL;

[TestFixture]
public class CompilerTests
{
    [TestCase("Compiler/TestCases/casper.txt", "Compiler/TestCases/casper.result.ics")]
    [TestCase("Compiler/TestCases/casper.hide.txt", "Compiler/TestCases/casper.result.ics")]
    
    public void TestSourceCodeExamples(string inputFile, string expectedFile)
    {
        var input = File.ReadAllText(inputFile);

        var compiler = new Compiler();
        var output = compiler.Compile(input);

        var expectedLines = File.ReadAllLines(expectedFile);
        var actualLines = output.Split(Environment.NewLine);

        for (int i = 0; i < actualLines.Length; i++)
        {
            if (actualLines[i].StartsWith("DTSTAMP") ||
                actualLines[i].StartsWith("UID") ||
                (actualLines[i] == ""))
            {
                continue;
            }

            Assert.That(actualLines[i].Trim(), Is.EqualTo(expectedLines[i].Trim()));
        }
    }
}