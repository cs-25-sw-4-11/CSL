using CSL.Exceptions;

namespace CSL;

using Antlr4.Runtime.Misc;

public class ClockVisitor : CSLBaseVisitor<Clock>
{
    public override Clock VisitClock(CSLParser.ClockContext context)
    {
        if (context.children.Count != 3)
        {
            throw new InvalidLiteralCompilerException("Clock: Invalid syntax, expected format is 'hour:minute'");
        }

        var hourRaw = context.children[0].GetText();
        var colon = context.children[1].GetText();
        var minuteRaw = context.children[2].GetText();

        if (colon != ":")
        {
            throw new InvalidLiteralCompilerException("Clock: Invalid separator, expected ':'");
        }

        if (!int.TryParse(hourRaw, out int hour))
        {
            throw new InvalidLiteralCompilerException("Clock: Not a number given as hour");
        }

        if (!int.TryParse(minuteRaw, out int minute))
        {
            throw new InvalidLiteralCompilerException("Clock: Not a number given as minute");
        }

        // Validate hour and minute values
        if (hour < 0 || hour > 23)
        {
            throw new InvalidLiteralCompilerException($"Clock: Hour value {hour} outside valid range (0-23)");
        }

        if (minute < 0 || minute > 59)
        {
            throw new InvalidLiteralCompilerException($"Clock: Minute value {minute} outside valid range (0-59)");
        }

        return new Clock(hour, minute);
    }
}