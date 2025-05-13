namespace CSL;

public class CalendarVisitor : CSLBaseVisitor<Calendar>
{
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

    public override Calendar VisitDoublePlusOp(CSLParser.DoublePlusOpContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        
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
}