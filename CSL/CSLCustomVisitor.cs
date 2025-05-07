namespace CSL;

using System.ComponentModel.DataAnnotations;
using Antlr4.Runtime.Misc;

public class CSLCustomVisitor : CSLBaseVisitor<object>
{

    public override object VisitProg(CSLParser.ProgContext context)
    {
        object lastResult = null;
        foreach (var child in context.children)
        {
            lastResult = Visit(child);
        }

        return lastResult;
    }
    
    public override object VisitLiteral(CSLParser.LiteralContext context)
    {
        if (context.subject() != null)
        {
            SubjectVisitor visitor = new SubjectVisitor();
            return visitor.VisitSubject(context.subject());
        }
        else if (context.description() != null)
        {
            DescriptionVisitor visitor = new DescriptionVisitor();
            return visitor.VisitDescription(context.description());
        }
        else if (context.date() != null)
        {
            DateVisitor visitor = new DateVisitor();
            return visitor.VisitDate(context.date());
        }
        else if (context.clock() != null)
        {
            ClockVisitor visitor = new ClockVisitor();
            return visitor.VisitClock(context.clock());
        }
        else if (context.duration() != null)
        {
            DurationVisitor visitor = new DurationVisitor();
            return visitor.VisitDuration(context.duration());
        }
        throw new NotImplementedException("Unknown literal type");
    }

    public override object VisitAddOp(CSLParser.AddOpContext context)
    {
        object left = Visit(context.expr(0));
        object right = Visit(context.expr(1));
        
        if (left is Duration leftDuration && right is Duration rightDuration)
        {
            return new Duration(
                leftDuration.Minutes + rightDuration.Minutes,
                leftDuration.Months + rightDuration.Months
            );
        }
        
        throw new Exception($"Cannot add values of types {left.GetType().Name} and {right.GetType().Name}");
    }
}