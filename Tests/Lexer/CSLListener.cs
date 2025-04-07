//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from CSL.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="CSLParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ICSLListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="CSLParser.prog"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProg([NotNull] CSLParser.ProgContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CSLParser.prog"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProg([NotNull] CSLParser.ProgContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CSLParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStat([NotNull] CSLParser.StatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CSLParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStat([NotNull] CSLParser.StatContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>HideExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterHideExpr([NotNull] CSLParser.HideExprContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>HideExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitHideExpr([NotNull] CSLParser.HideExprContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TildeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTildeOp([NotNull] CSLParser.TildeOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TildeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTildeOp([NotNull] CSLParser.TildeOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>AddOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAddOp([NotNull] CSLParser.AddOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>AddOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAddOp([NotNull] CSLParser.AddOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>IntersectOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIntersectOp([NotNull] CSLParser.IntersectOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>IntersectOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIntersectOp([NotNull] CSLParser.IntersectOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>BeforeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBeforeOp([NotNull] CSLParser.BeforeOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>BeforeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBeforeOp([NotNull] CSLParser.BeforeOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>InOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInOp([NotNull] CSLParser.InOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>InOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInOp([NotNull] CSLParser.InOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>AfterOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAfterOp([NotNull] CSLParser.AfterOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>AfterOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAfterOp([NotNull] CSLParser.AfterOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>RecursiveOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRecursiveOp([NotNull] CSLParser.RecursiveOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>RecursiveOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRecursiveOp([NotNull] CSLParser.RecursiveOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>DoublePlusOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDoublePlusOp([NotNull] CSLParser.DoublePlusOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>DoublePlusOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDoublePlusOp([NotNull] CSLParser.DoublePlusOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ComplementOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComplementOp([NotNull] CSLParser.ComplementOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ComplementOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComplementOp([NotNull] CSLParser.ComplementOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>UnionOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnionOp([NotNull] CSLParser.UnionOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>UnionOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnionOp([NotNull] CSLParser.UnionOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>StrictlyAfterOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStrictlyAfterOp([NotNull] CSLParser.StrictlyAfterOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>StrictlyAfterOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStrictlyAfterOp([NotNull] CSLParser.StrictlyAfterOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>StrictlyBeforeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStrictlyBeforeOp([NotNull] CSLParser.StrictlyBeforeOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>StrictlyBeforeOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStrictlyBeforeOp([NotNull] CSLParser.StrictlyBeforeOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>IdentifierExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifierExpr([NotNull] CSLParser.IdentifierExprContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>IdentifierExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifierExpr([NotNull] CSLParser.IdentifierExprContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>LiteralExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralExpr([NotNull] CSLParser.LiteralExprContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>LiteralExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralExpr([NotNull] CSLParser.LiteralExprContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ParenExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenExpr([NotNull] CSLParser.ParenExprContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ParenExpr</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenExpr([NotNull] CSLParser.ParenExprContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>SubtractOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSubtractOp([NotNull] CSLParser.SubtractOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>SubtractOp</c>
	/// labeled alternative in <see cref="CSLParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSubtractOp([NotNull] CSLParser.SubtractOpContext context);
}
