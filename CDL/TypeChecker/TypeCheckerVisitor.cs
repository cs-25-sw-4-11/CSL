using System.Data;
using System.Globalization;
using Antlr4.Runtime.Misc;
using CDL.Exceptions;

namespace CDL.TypeChecker;

public class TypeCheckerVisitor : CSLBaseVisitor<EventTypes>
{
    public Dictionary<string, EventTypes> Variables = new Dictionary<string, EventTypes>();

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
            throw new InvalidTypeCompilerException([EventTypes.DateTime], left, context);
        }

        if (right != EventTypes.DateTime)
        {
            throw new InvalidTypeCompilerException([EventTypes.DateTime], right, context);
        }

        return EventTypes.DateTime;
    }

    public override EventTypes VisitAddOp(CSLParser.AddOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left == EventTypes.Calendar || right == EventTypes.Calendar)
        {
            if (left == right)
            {
                throw new InvalidTypeCompilerException([
                    EventTypes.Subject |
                    EventTypes.DateTime |
                    EventTypes.Description |
                    EventTypes.Duration
                ], left, context); // NOTE: Expected types are not exhaustive
            }

            if (left == EventTypes.Duration || right == EventTypes.Duration)
            {
                return EventTypes.Calendar;
            }

            throw new InvalidTypeCompilerException(
                [EventTypes.Duration],
                left == EventTypes.Calendar ? right : left, context);
        }

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
            throw new InvalidTypeCompilerException([EventTypes.Duration], left, context);
        }

        // Check for valid other operand
        if (otherOperand.HasFlag(EventTypes.Duration) && !otherOperand.HasFlag(EventTypes.DateTime))
        {
            return otherOperand;
        }

        if (otherOperand.HasFlag(EventTypes.DateTime) && !otherOperand.HasFlag(EventTypes.Duration))
        {
            return otherOperand;
        }

        throw new InvalidTypeCompilerException([EventTypes.DateTime, EventTypes.Duration], otherOperand, context);
    }

    public override EventTypes VisitSubtractOp(CSLParser.SubtractOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (right == EventTypes.Calendar)
        {
            throw new InvalidTypeCompilerException([
                EventTypes.Subject |
                EventTypes.DateTime |
                EventTypes.Description |
                EventTypes.Duration
            ], right, context); // NOTE: Expected types are not exhaustive
        }

        if (left == EventTypes.Calendar)
        {
            return EventTypes.Calendar;
        }

        // Check for duration operand

        if (right != EventTypes.Duration)
        {
            throw new InvalidTypeCompilerException([EventTypes.Duration], right, context);
        }

        EventTypes otherOperand = left;

        //Check for valid other operand

        if (otherOperand.HasFlag(EventTypes.Duration) && !otherOperand.HasFlag(EventTypes.DateTime))
        {
            return EventTypes.Duration;
        }


        if (otherOperand.HasFlag(EventTypes.DateTime) && !otherOperand.HasFlag(EventTypes.Duration))
        {
            return EventTypes.DateTime;
        }

        throw new InvalidTypeCompilerException([EventTypes.DateTime, EventTypes.Duration], otherOperand, context);
    }

    public override EventTypes VisitDoublePlusOp([NotNull] CSLParser.DoublePlusOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));


        if (left == EventTypes.Calendar || right == EventTypes.Calendar)
        {
            if (left == right)
            {
                throw new InvalidTypeCompilerException([
                    EventTypes.Subject |
                    EventTypes.DateTime |
                    EventTypes.Description |
                    EventTypes.Duration
                ], left, context); // NOTE: Expected types are not exhaustive
            }
            else
            {
                return EventTypes.Calendar;
            }
        }

        if (((left & right) != 0)
            && left != EventTypes.DateTime // Exempt DateTime, no questions asked.
            )
        {
            throw new InvalidTypeCompilerException([~left], left, context); // NOTE: Expected types are not exhaustive
        }

        return left | right;
    }

    public override EventTypes VisitUnionOp([NotNull] CSLParser.UnionOpContext context)
    {
        return EventTypes.Calendar;
    }

    public override EventTypes VisitStrictlyAfterOp([NotNull] CSLParser.StrictlyAfterOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left.HasFlag(EventTypes.DateTime))
        {
            throw new InvalidTypeCompilerException([
                EventTypes.Subject |
                EventTypes.Calendar |
                EventTypes.Description |
                EventTypes.Duration
            ], left);
        }

        if (right == EventTypes.Calendar)
        {
            return EventTypes.Calendar;
        }

        if (!right.HasFlag(EventTypes.DateTime) || !right.HasFlag(EventTypes.Duration))
        {
            throw new InvalidTypeCompilerException([EventTypes.DateTime | EventTypes.Duration], right, context);
        }

        return EventTypes.Calendar;
    }

    public override EventTypes VisitStrictlyBeforeOp([NotNull] CSLParser.StrictlyBeforeOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left.HasFlag(EventTypes.DateTime))
        {
            throw new InvalidTypeCompilerException([
                EventTypes.Subject |
                EventTypes.Calendar |
                EventTypes.Description |
                EventTypes.Duration
            ], left, context);
        }

        if (left == EventTypes.Calendar)
        {
            if (right != EventTypes.Calendar && !right.HasFlag(EventTypes.DateTime))
            {
                throw new InvalidTypeCompilerException([EventTypes.Calendar | EventTypes.DateTime], right, context);
            }

            return EventTypes.Calendar;
        }

        if (!left.HasFlag(EventTypes.Duration))
        {
            throw new InvalidTypeCompilerException([EventTypes.Duration], left, context);
        }

        return EventTypes.Calendar;
    }

    public override EventTypes VisitSplitOp([NotNull] CSLParser.SplitOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left == EventTypes.Calendar && right == EventTypes.Calendar)
        {
            return EventTypes.Calendar;
        }

        throw new InvalidTypeCompilerException(
            [EventTypes.Calendar],
            left == EventTypes.Calendar ? right : left, context);
    }

    public override EventTypes VisitRecurrenceOp([NotNull] CSLParser.RecurrenceOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left == EventTypes.Calendar || right == EventTypes.Calendar)
        {
            if (left == right)
            {
                throw new InvalidTypeCompilerException([
                    EventTypes.Subject |
                    EventTypes.DateTime |
                    EventTypes.Description |
                    EventTypes.Duration
                ], left, context); // NOTE: Expected types are not exhaustive
            }

            if (left == EventTypes.Duration || right == EventTypes.Duration)
            {
                return EventTypes.Calendar;
            }
            else
            {
                throw new InvalidTypeCompilerException(
                    [EventTypes.Duration],
                    left == EventTypes.Calendar ? right : left, context);
            }
        }

        if (left == EventTypes.Duration && !right.HasFlag(EventTypes.DateTime))
        {
            throw new InvalidTypeCompilerException(
                [EventTypes.DateTime],
                right, context);
        }

        if (!left.HasFlag(EventTypes.DateTime) && right == EventTypes.Duration)
        {
            throw new InvalidTypeCompilerException(
                [EventTypes.DateTime],
                left, context);
        }

        return EventTypes.Calendar;
    }

    public override EventTypes VisitSetdiffOp([NotNull] CSLParser.SetdiffOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left == EventTypes.Calendar && right == EventTypes.Calendar)
        {
            return EventTypes.Calendar;
        }

        throw new InvalidTypeCompilerException(
            [EventTypes.Calendar],
            left == EventTypes.Calendar ? right : left, context);
    }

    public override EventTypes VisitLiteral(CSLParser.LiteralContext context)
    {
        var child = context.children.FirstOrDefault();
        var res = Visit(context.children.First());

        return res;
    }

    public override EventTypes VisitHideExpr(CSLParser.HideExprContext context)
    {
        return Visit(context.expr());
    }

    public override EventTypes VisitParenExpr(CSLParser.ParenExprContext context)
    {
        return Visit(context.expr());
    }
    
    public override EventTypes VisitStat(CSLParser.StatContext context)
    {
        var value = Visit(context.expr());
        var key = context.IDENTIFIER().GetText();

        if (!Variables.TryAdd(key, value))
        {
            throw new InvalidIdentifierCompilerException($"Identifier '{key}' not found");
        }

        return EventTypes.Calendar;
    }

    public override EventTypes VisitIdentifierExpr(CSLParser.IdentifierExprContext context)
    {
        var key = context.IDENTIFIER().GetText();

        if (!Variables.TryGetValue(key, out var expr))
        {
            throw new InvalidIdentifierCompilerException($"Identifier '{key}' not found");
        }

        return expr;
    }
}