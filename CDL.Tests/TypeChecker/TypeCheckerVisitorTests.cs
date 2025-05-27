using Antlr4.Runtime;
using CDL.Exceptions;
using CDL.TypeChecker;

namespace CDL.Tests.TypeChecker;

[TestFixture]
public class TypeCheckerVisitorTests
{
    [TestCase("1d")]
    [TestCase("1d + 1d")]
    [TestCase("1d + 1min")]
    [TestCase("1d + 01/01/2001")]
    [TestCase("1d + 10:00")]
    [TestCase("1h + 10:00")]
    [TestCase("10:00~11:00")] // TildeOp
    [TestCase("01/01/2001~01/02/2001")] // TildeOp
    [TestCase("01/01/2001~10:00")] // TildeOp
    [TestCase("2h - 1h")] //SubtractOp
    [TestCase("02/01/2001 - 1 d")] //SubtractOp
    [TestCase("3h ++ 'abc'")] // DoublePlusOp
    [TestCase("(\"abc\" || \"cba\") + 1h")] // AddOp calendar + duration
    [TestCase("(\"abc\" || \"cba\") - 1h")] // SubtractOp calendar - duration
    [TestCase("(\"abc\" || 02/06/2004) << (\"abc\" || 02/06/2004)")] // StrictlyBeforeOp 2 calendars
    [TestCase("(\"abc\" ++ 3h) << (12:30 ++ \"def\")")] // StrictlyBeforeOp contains duration << datetime
    [TestCase("1h << (01/01/2001 || 12:30)")] // StrictlyBeforeOp datetime << calendar
    [TestCase("(\"abc\" || 1h) << (01/01/2001 ++ \"def\")")] // StrictlyBeforeOp calender << contains datetime
    [TestCase("\"abc\" >> (12:30 || 01/01/2001)")] // StrictlyAfterOp event >> calendar
    [TestCase("1h ++ \"abc\") >> (01/01/2001 ++ 12h)")] // StrictlyAfterOp event >> event
    [TestCase("(1h || \"abc\") >> (01/01/2001 ++ 12h)")] // StrictlyAfterOp calendar >> event
    [TestCase("(\"abc\" || \"cba\") * 1h ")]
    [TestCase("1h * (\"abc\" || \"cba\")")]
    [TestCase("08/08/2002 * 1 y")]
    [TestCase("1 y * 08/08/2002")]
    [TestCase("(\"abc\" || 12:30) \\ (\"abc\" || 01/01/2001)")]
    [TestCase("(\"abc\" || 12:30) / (\"abc\" || 01/01/2001)")]
    public void TestTypeExpressionsValid(string input)
    {

        var tree = StringParser.ParseString(input);
        var typeVisitor = new TypeCheckerVisitor();

        Assert.DoesNotThrow(() => typeVisitor.Visit(tree));
    }

    [TestCase("'abc' + 1d")] // AddOp
    [TestCase("01/01/2001 + 01/01/2001")] // AddOp
    [TestCase("01/01/2001~'abc'")] // TildeOp
    [TestCase("3d - 01/01/2001")] // SubtractOp
    [TestCase("'abc' - 3h")] // SubtractOp
    [TestCase("3h ++ 3h")] // DoublePlusOp
    [TestCase("(\"abc\" || \"cba\") + 12:30")] // AddOp calendar + event without duration
    [TestCase("(\"abc\" || \"cba\") + (\"abc\" || \"cba\")")] // AddOp celendar + calendar
    [TestCase("12:30 << (\"abc\" || 02/06/2004)")] // StrictlyBeforeOp datetime << calendar
    [TestCase("02/06/2004 << 12:30")] // StrictlyBeforeOp datetime << datetime
    [TestCase("(\"abc\" || 3h) << \"abc\"")] // StrictlyBeforeOp calender << missing datetime
    [TestCase("\"abc\" << 12:30")] // StrictlyBeforeOp missing duration << datetime
    [TestCase("01/01/2001 >> (1h ++ 12:30)")] // StrictlyAfterOp contains datetime >> event
    [TestCase("(\"abc\" ++ 1h) >> 01/01/2001")] //StrictlyAfterOp event >> missing duration
    [TestCase("(\"abc\" || 1h) >> \"abc\" ++ 1h")] //StrictlyAfterOp calendar >> missing datetime
    [TestCase("(\"abc\" || \"cba\") - (\"abc\" || \"cba\")")]
    [TestCase("1 h - (\"abc\" || \"cba\")")]
    [TestCase("(\"abc\" || \"cba\") * (\"abc\" || \"cba\")")]
    [TestCase("(\"abc\" || \"cba\") * 13:30")]
    [TestCase("13:30 * (\"abc\" || \"cba\")")]
    [TestCase("1h * \"abc\" ++ 3h")]
    [TestCase("\"abc\" ++ 12:30 / \"abc\" ++ 01/01/2001")]
    [TestCase("\"abc\" ++ 12:30 \\ \"abc\" ++ 01/01/2001")]
    public void TestTypeExpressionsInvalid(string input)
    {

        var tree = StringParser.ParseString(input);
        var typeVisitor = new TypeCheckerVisitor();

        Assert.Throws<InvalidTypeCompilerException>(() => typeVisitor.Visit(tree));
    }
}