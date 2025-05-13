using CSL.Exceptions;

namespace CSL.SubTypes;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime;

public class ClockVisitor : CSLBaseVisitor<Clock?>
{
    public override Clock? VisitClock(CSLParser.ClockContext context)
    {
        try 
        {
            if (context.children.Count != 3)
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Invalid syntax, expected format is 'hour:minute'");
            }

            var hourRaw = context.children[0].GetText();
            var colon = context.children[1].GetText();
            var minuteRaw = context.children[2].GetText();

            if (colon != ":")
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Invalid separator, expected ':'");
            }

            // Try to parse with more specific error handling
            if (!int.TryParse(hourRaw, out int hour))
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Not a number given as hour: '{hourRaw}'");
            }

            if (!int.TryParse(minuteRaw, out int minute))
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Not a number given as minute: '{minuteRaw}'");
            }

            // Validate hour and minute values
            if (hour < 0 || hour > 23)
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Hour value {hour} outside valid range (0-23)");
            }

            if (minute < 0 || minute > 59)
            {
                throw new InvalidLiteralCompilerException($"{nameof(Clock)}: Minute value {minute} outside valid range (0-59)");
            }

            return new Clock(hour, minute);
        }
        catch (CompilerException)
        {
            // Re-throw our custom exceptions
            throw;
        }
    }
    
    // Override the default visit behavior to catch any parsing errors
    public override Clock? Visit(Antlr4.Runtime.Tree.IParseTree tree)
    {
        try
        {
            return base.Visit(tree);
        }
        catch (CompilerException)
        {
            throw;
        }
    }
}