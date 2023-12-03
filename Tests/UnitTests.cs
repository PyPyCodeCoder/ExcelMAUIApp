using NUnit.Framework;
using Controllers;

namespace Tests;

public class Tests
{
    [Test]
    public void CellsReferencesTest()
    {
        CalculationController.CellsController.Cells["A1"] = new Cell();
        CalculationController.CellsController.Cells["A2"] = new Cell();

        CalculationController.CellsController.EditCell("A1", "5");
        CalculationController.CellsController.EditCell("A2", "A1");
        
        Assert.That(CalculationController.CellsController.Cells["A2"].CalculatedFormula, Is.EqualTo(5));
    }

    [Test]
    public void CellsCyclicDependencyTest()
    {
        CalculationController.CellsController.Cells["A1"] = new Cell();
        CalculationController.CellsController.Cells["A2"] = new Cell();
        
        CalculationController.CellsController.EditCell("A1", "A2");
        CalculationController.CellsController.EditCell("A2", "A1");
        
        Assert.Throws<Exception>(() => CalculationController.Evaluate(CalculationController.CellsController.Cells["A1"].Formula));
    }
    
    [Test]
    public void ArithmeticExpressionsTest()
    {
        Assert.That(CalculationController.Evaluate("2 + 2"), Is.EqualTo(4));
        Assert.That(CalculationController.Evaluate("10 - 5"), Is.EqualTo(5));
        Assert.That(CalculationController.Evaluate("2 * 3"), Is.EqualTo(6));
        
        Assert.That(CalculationController.Evaluate("14 / 2"), Is.EqualTo(7));
        Assert.Throws<DivideByZeroException>(() => CalculationController.Evaluate("15 / 0"));
        
        Assert.That(CalculationController.Evaluate(" 2 ^ 3"), Is.EqualTo(8));
        
        Assert.That(CalculationController.Evaluate("max(8, 9)"), Is.EqualTo(9));
        Assert.That(CalculationController.Evaluate("min(10, 11)"), Is.EqualTo(10));
    }

    [Test]
    public void LogicalExpressionsTest()
    {
        Assert.That(CalculationController.Evaluate("5 = 3"), Is.EqualTo(0));
        Assert.That(CalculationController.Evaluate("5 = 5"), Is.EqualTo(1));
        
        Assert.That(CalculationController.Evaluate("5 < 3"), Is.EqualTo(0));
        Assert.That(CalculationController.Evaluate("3 < 5"), Is.EqualTo(1));
        
        Assert.That(CalculationController.Evaluate("3 > 5"), Is.EqualTo(0));
        Assert.That(CalculationController.Evaluate("5 > 3"), Is.EqualTo(1));
        
        Assert.That(CalculationController.Evaluate("not 1"), Is.EqualTo(0));
        Assert.That(CalculationController.Evaluate("not 0"), Is.EqualTo(1));
    }
}