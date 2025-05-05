using CSL.Exceptions;

namespace CSL;

using Antlr4.Runtime.Misc;

public class DurationVisitor : CSLBaseVisitor<Duration>
{
    public override Duration VisitDuration(CSLParser.DurationContext context)
    {
        if (context.children.Count != 2)
        {
            throw new InvalidLiteralCompilerException("Duration: Invalid syntax, wrong amount of children");
        }

        var valueRaw = context.children[0].GetText();

        int value;
        if (!int.TryParse(valueRaw, out value))
        {
            throw new InvalidLiteralCompilerException("Duration: Not a number given as value");
        }

        var unit = context.children[1].GetText();

        return unit switch
        {
            "min" => Duration.FromMinutes(value),
            "h" => Duration.FromHours(value),
            "d" => Duration.FromDays(value),
            "w" => Duration.FromWeeks(value),
            "mth" => Duration.FromMonths(value),
            "y" => Duration.FromYears(value),
            _ => throw new InvalidLiteralCompilerException($"Duration: Invalid unit used {unit}")
        };
    }
}