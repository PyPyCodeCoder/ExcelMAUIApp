﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Valera\122_22_2\OOP\ExcelMAUIApp\Controllers\CalculationController.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Controller {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="CalculationControllerParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public interface ICalculationControllerVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by the <c>ParenthesizedExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesizedExpr([NotNull] CalculationControllerParser.ParenthesizedExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>ExponentialExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExponentialExpr([NotNull] CalculationControllerParser.ExponentialExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>MultiplicativeExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplicativeExpr([NotNull] CalculationControllerParser.MultiplicativeExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>AdditiveExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdditiveExpr([NotNull] CalculationControllerParser.AdditiveExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>ComparisonExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonExpr([NotNull] CalculationControllerParser.ComparisonExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>NotExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotExpr([NotNull] CalculationControllerParser.NotExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>MaxExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMaxExpr([NotNull] CalculationControllerParser.MaxExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>MinExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinExpr([NotNull] CalculationControllerParser.MinExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>NumberExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberExpr([NotNull] CalculationControllerParser.NumberExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>IdentifierExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifierExpr([NotNull] CalculationControllerParser.IdentifierExprContext context);

	/// <summary>
	/// Visit a parse tree produced by the <c>BoolExpr</c>
	/// labeled alternative in <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolExpr([NotNull] CalculationControllerParser.BoolExprContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="CalculationControllerParser.compileUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompileUnit([NotNull] CalculationControllerParser.CompileUnitContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="CalculationControllerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] CalculationControllerParser.ExpressionContext context);
}
} // namespace Controller