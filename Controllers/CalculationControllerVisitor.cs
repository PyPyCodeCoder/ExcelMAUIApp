using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Antlr4.Runtime.Tree;
using Controller;

namespace Controllers;

internal class CalculationControllerVisitor : CalculationControllerBaseVisitor<double>
{ 
    public override double VisitCompileUnit(CalculationControllerParser.CompileUnitContext context)
    {
        return Visit(context.expression());
    }
    
    public override double VisitNumberExpr(CalculationControllerParser.NumberExprContext context)
    {
        var result = double.Parse(context.GetText());
        Debug.WriteLine(result);
        return result;
    }

    //IdentifierExpr
    public override double VisitIdentifierExpr(CalculationControllerParser.IdentifierExprContext context)
    {
        var result = context.GetText();
        if (CellsController.IsCyclicDependency(CalculationController.ProcessingCellName, result))
        {
            throw new Exception("Invalid input");
        }
        var evaluatingCell = CalculationController.CellsController.Cells[CalculationController.ProcessingCellName];
        var visitedCell = CalculationController.CellsController.Cells[result];
        if (!evaluatingCell.DependsOnCells.Contains(result))
        {
            evaluatingCell.DependsOnCells.Add(result);
        }

        if (!visitedCell.DependentCells.Contains(CalculationController.ProcessingCellName))
        {
            visitedCell.DependentCells.Add(CalculationController.ProcessingCellName);
        }
        return visitedCell.CalculatedFormula;
    }

    public override double VisitParenthesizedExpr(CalculationControllerParser.ParenthesizedExprContext context)
    {
        return Visit(context.expression());
    }

    public override double VisitExponentialExpr(CalculationControllerParser.ExponentialExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
        Debug.WriteLine("{0} ^ {1}", left, right);
        return Math.Pow(left, right);
    }

    public override double VisitAdditiveExpr(CalculationControllerParser.AdditiveExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
        if (context.operatorToken.Type == CalculationControllerLexer.ADD)
        {
            Debug.WriteLine("{0} + {1}", left, right);
            return left + right;
        }
        else //CalculationControllerLexer.SUBTRACT
        {
            Debug.WriteLine("{0} - {1}", left, right);
            return left - right;
        }
    }

    public override double VisitMultiplicativeExpr(CalculationControllerParser.MultiplicativeExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
    
        if (context.operatorToken.Type == CalculationControllerLexer.MULTIPLY)
        {
            Debug.WriteLine("{0} * {1}", left, right);
            return left * right;
        }
        else // CalculationControllerLexer.DIVIDE
        {
            if (right != 0)
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
            else
            {
                throw new DivideByZeroException("Division by zero is not allowed.");
            }
        }
    }
    
    public override double VisitComparisonExpr(CalculationControllerParser.ComparisonExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
        if (context.operatorToken.Type == CalculationControllerLexer.LESS_THAN)
        {
            Debug.WriteLine("{0} < {1}", left, right);
            return left < right ? 1 : 0;  //true : false;
        }
        else if (context.operatorToken.Type == CalculationControllerLexer.GREATER_THAN)
        {
            Debug.WriteLine("{0} > {1}", left, right);
            return left > right ? 1 : 0;  //true : false;
        }
        else //CalculationControllerLexer.EQUALS
        {
            Debug.WriteLine("{0} = {1}", left, right);
            return left == right ? 1 : 0;  //true : false;
        }
    }
    
    public override double VisitMaxExpr(CalculationControllerParser.MaxExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
        return Math.Max(left, right);
    }
    
    public override double VisitMinExpr(CalculationControllerParser.MinExprContext context)
    {
        var left = WalkLeft(context);
        var right = WalkRight(context);
        return Math.Min(left, right);
    }
    
    public override double VisitNotExpr(CalculationControllerParser.NotExprContext context)
    {
        var operandValue = Visit(context.expression());
        if (operandValue == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    public override double VisitBoolExpr(CalculationControllerParser.BoolExprContext context)
    {
        var operandValue = context.GetText();
        
        bool boolValue;
        if (bool.TryParse(operandValue, out boolValue))
        {
            if (boolValue)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            throw new InvalidOperationException($"Invalid boolean expression: {operandValue}");
        }
    }
    
    private double WalkLeft(CalculationControllerParser.ExpressionContext context)
    {
        return Visit(context.GetRuleContext < CalculationControllerParser.ExpressionContext > (0));
    }

    private double WalkRight(CalculationControllerParser.ExpressionContext context)
    {
        return Visit(context.GetRuleContext < CalculationControllerParser.ExpressionContext > (1));
    }
}