using System.ComponentModel.DataAnnotations;
using Antlr4.Runtime.Misc;

public class CSLCustomVisitor : CSLBaseVisitor<string>
{
    public override string VisitAddOp([NotNull] CSLParser.AddOpContext context)
    {
        return base.VisitAddOp(context);
    }
}