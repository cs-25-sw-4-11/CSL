using Antlr4.Runtime.Misc;
using CSL.Exceptions;

namespace CSL;

public class CalendarVisitor : CSLBaseVisitor<Calendar>
{
    public Dictionary<string, Calendar> Variables = new Dictionary<string, Calendar>();

    public override Calendar VisitSubject(CSLParser.SubjectContext context) =>
        new Event(Subject: new SubjectVisitor().VisitSubject(context));

    public override Calendar VisitDate(CSLParser.DateContext context) =>
        new Event(Date: new DateVisitor().VisitDate(context));

    public override Calendar VisitClock(CSLParser.ClockContext context) =>
        new Event(Clock: new ClockVisitor().VisitClock(context));

    public override Calendar VisitDuration(CSLParser.DurationContext context) =>
        new Event(Duration: new DurationVisitor().VisitDuration(context));

    public override Calendar VisitDescription(CSLParser.DescriptionContext context) =>
        new Event(Description: new DescriptionVisitor().VisitDescription(context));

    public override Calendar VisitStat(CSLParser.StatContext context)
    {
        var value = Visit(context.expr());
        var key = context.IDENTIFIER().GetText();

        if (!Variables.TryAdd(key, value))
        {
            throw new InvalidIdentifierCompilerException($"Identifier '{key}' not found");
        }

        return new Calendar([]);
    }

    public override Calendar VisitIdentifierExpr(CSLParser.IdentifierExprContext context)
    {
        var key = context.IDENTIFIER().GetText();

        if (!Variables.TryGetValue(key, out var expr))
        {
            throw new InvalidIdentifierCompilerException($"Identifier '{key}' not found");
        }

        return expr;
    }

    public override Calendar VisitDoublePlusOp(CSLParser.DoublePlusOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (!left.IsEvent() && !right.IsEvent())
        {
            throw new ArgumentException($"PlusPlus called on two Calendars: {left} and {right}");
        }

        if (!left.IsEvent())
        {
            return Calendar.ConcatOperator(left, (Event)right);
        }

        if (!right.IsEvent())
        {
            return Calendar.ConcatOperator(right, (Event)left);
        }

        return Event.ConcatOperator((Event)left, (Event)right);
    }

    public override Calendar VisitUnionOp(CSLParser.UnionOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        return Calendar.UnionOp(left, right);
    }

    public override Calendar VisitAddOp(CSLParser.AddOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left.IsEvent() && right.IsEvent())
        {
            return Event.AddOperator((Event)left, (Event)right);
        }
        else
        {
            return Calendar.AddOperator(left, (Event)right);
        }
    }

    public override Calendar VisitSubtractOp([NotNull] CSLParser.SubtractOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));

        if (left.IsEvent() && right.IsEvent())
        {
            return Event.SubOperator((Event)left, (Event)right);
        }
        else
        {
            return Calendar.SubOperator(left, (Event)right);
        }
    }

    public override Calendar VisitHideExpr([NotNull] CSLParser.HideExprContext context)
    {
        var visit = Visit(context.expr());
        if (visit.IsEvent())
        {
            return Event.HideOperator((Event)visit);
        }
        else
        {
            return Calendar.HideOperator(visit);
        }
    }


}