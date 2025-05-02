namespace CSL;

using Antlr4.Runtime.Misc;

public class DurationVisitor : CSLBaseVisitor<Duration>
{
    public override Duration VisitDuration(CSLParser.DurationContext context)
    {
        var exprCtx = context.children.First();
        var text = exprCtx.GetText();

        return base.VisitDuration(context);
    }
}