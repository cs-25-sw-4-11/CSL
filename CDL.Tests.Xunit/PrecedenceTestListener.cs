namespace CDL.Tests.Xunit;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Xunit;
using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

public class PrecedenceTestListener : CSLBaseListener
{

    public List<string> Operations { get; } = new List<string>();

    public override void EnterParenExpr([NotNull] CSLParser.ParenExprContext context)
    {
        Operations.Add("ParenExpr");
    }

    public override void EnterHideExpr([NotNull] CSLParser.HideExprContext context)
    {
        Operations.Add("HideExpr");
    }

    public override void EnterTildeOp([NotNull] CSLParser.TildeOpContext context)
    {
        Operations.Add("TildeOp");
    }

    public override void EnterDoublePlusOp([NotNull] CSLParser.DoublePlusOpContext context)
    {
        Operations.Add("DoublePlusOp");
    }

    public override void EnterAddOp([NotNull] CSLParser.AddOpContext context)
    {
        Operations.Add("AddOp");
    }

    public override void EnterSubtractOp([NotNull] CSLParser.SubtractOpContext context)
    {
        Operations.Add("SubstractOp");
    }

    public override void EnterStrictlyBeforeOp([NotNull] CSLParser.StrictlyBeforeOpContext context)
    {
        Operations.Add("StrictlyBeforeOp");
    }

    public override void EnterStrictlyAfterOp([NotNull] CSLParser.StrictlyAfterOpContext context)
    {
        Operations.Add("StrictlyAfterOp");
    }

    public override void EnterRecurrenceOp([NotNull] CSLParser.RecurrenceOpContext context)
    {
        Operations.Add("RecursiveOp");
    }

    public override void EnterUnionOp([NotNull] CSLParser.UnionOpContext context)
    {
        Operations.Add("UnionOp");
    }
}