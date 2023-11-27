using Antlr4.Runtime;
using Controller;

namespace Controllers;

public static class CalculationController
{
    
    public static CellsController CellsController { get; }
    public static string ProcessingCellName { get; set; }

    static CalculationController()
    {
        CellsController = new CellsController();
        ProcessingCellName = "";
    }
    public static double Evaluate(string expression)
    {
        var lexer = new CalculationControllerLexer(new AntlrInputStream(expression));
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new ThrowExceptionErrorListener());

        var tokens = new CommonTokenStream(lexer);
        var parser = new CalculationControllerParser(tokens);

        var tree = parser.compileUnit();

        var visitor = new CalculationControllerVisitor();

        return visitor.Visit(tree);
    }
}

    
    

