using System.Data;
using CSL.Exceptions;

namespace CSL.TypeChecker;

public class TypeCheckerVisitor : CSLBaseVisitor<EventTypes>
{
    public override EventTypes VisitClock(CSLParser.ClockContext context)
    {
        return EventTypes.DateTime;
    }

    public override EventTypes VisitDescription(CSLParser.DescriptionContext context)
    {
        return EventTypes.Description;
    }

    public override EventTypes VisitDuration(CSLParser.DurationContext context)
    {
        return EventTypes.Duration;
    }

    public override EventTypes VisitDate(CSLParser.DateContext context)
    {
        return EventTypes.DateTime;
    }

    public override EventTypes VisitSubject(CSLParser.SubjectContext context)
    {
        return EventTypes.Subject;
    }
    
    public override EventTypes VisitTildeOp(CSLParser.TildeOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        
        if (left != EventTypes.DateTime)
        {
            throw new InvalidTypeCompilerException([EventTypes.DateTime], left);
        }

        if (right != EventTypes.DateTime)
        {
            throw new InvalidTypeCompilerException([EventTypes.DateTime], right);
        }

        return EventTypes.DateTime;
    }

    public override EventTypes VisitAddOp(CSLParser.AddOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        // Check for duration operand
        EventTypes otherOperand;
        if (left == EventTypes.Duration)
        {
            otherOperand = right;
        }
        else if (right == EventTypes.Duration)
        {
            otherOperand = left;
        }
        else
        {
            throw new InvalidTypeCompilerException([EventTypes.Duration], left);
        }
        
        // Check for valid other operand
        if (otherOperand.HasFlag(EventTypes.Duration) && !otherOperand.HasFlag(EventTypes.DateTime))
            return EventTypes.Duration;

        if (otherOperand.HasFlag(EventTypes.DateTime) && !otherOperand.HasFlag(EventTypes.Duration))
            return EventTypes.DateTime;
        
        if (otherOperand.HasFlag(EventTypes.DateTime) && !otherOperand.HasFlag(EventTypes.Duration))
            return EventTypes.DateTime;

        if (otherOperand.HasFlag(EventTypes.DateTime) && !otherOperand.HasFlag(EventTypes.Duration))
            return EventTypes.DateTime;
        
        throw new InvalidTypeCompilerException([EventTypes.DateTime, EventTypes.Duration], otherOperand);
    }
}