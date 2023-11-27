using System.Diagnostics;

namespace Controllers;

public class CellsController
{
    public IDictionary<string, Cell> Cells { get; set; }

    public CellsController()
    {
        Cells = new Dictionary<string, Cell>{};
    }

    public bool EditCell(string cellName, string formula)
    {
        var cell = Cells[cellName];
        var backupDependencies = cell.DependsOnCells;
        CalculationController.ProcessingCellName = cellName;

        cell.Formula = formula;
        try
        {
            cell.DependsOnCells = new List<string>();

            UpdateCell(cellName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            cell.DependsOnCells = backupDependencies;
            return false;
        }
        return true;
    }

    public static bool IsCyclicDependency(string name, string targetCellName)
    {
        if (name == targetCellName)
        {
            return true;
        }

        return CalculationController.CellsController.Cells[targetCellName].DependsOnCells.Any(childName => IsCyclicDependency(name, childName));
    }
    
    private void UpdateCell(string cellName)
    {
        var cell = Cells[cellName];
        CalculationController.ProcessingCellName = cellName;
        cell.CalculatedFormula = CalculationController.Evaluate(cell.Formula);
        
        for (var i = 0; i < cell.DependentCells.Count; i++)
        {
            var name = cell.DependentCells[i];
            var cellToUpdate = Cells[name];
            if (cellToUpdate.DependsOnCells.Contains(cellName))
            {
                UpdateCell(name);
                continue;
            }
            cell.DependentCells.RemoveAt(i--);
        }
    }
}