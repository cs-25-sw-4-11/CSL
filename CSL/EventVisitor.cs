using Antlr4.Runtime.Tree;

namespace CSL;

using Antlr4.Runtime.Misc;

public class EventVisitor : CSLBaseVisitor<Event>
{
    public override Event VisitSubject(CSLParser.SubjectContext context) =>
        new(Subject: new SubjectVisitor().VisitSubject(context));

    public override Event VisitDate(CSLParser.DateContext context) =>
        new(Date: new DateVisitor().VisitDate(context));

    public override Event VisitClock(CSLParser.ClockContext context) =>
        new(Clock: new ClockVisitor().VisitClock(context));

    public override Event VisitDuration(CSLParser.DurationContext context) =>
        new(Duration: new DurationVisitor().VisitDuration(context));

    public override Event VisitDescription(CSLParser.DescriptionContext context) =>
        new(Description: new DescriptionVisitor().VisitDescription(context));
}