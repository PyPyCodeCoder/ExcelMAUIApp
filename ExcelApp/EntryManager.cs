using Controllers;
using Cell = Controllers.Cell;
using Grid = Microsoft.Maui.Controls.Grid;

namespace ExcelApp;

public partial class MainPage
{
    private Entry CreateEntry()
    {
        var entry = new Entry
        {
            Text = "",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Fill
        };
        entry.Focused += Entry_Focused;
        entry.Unfocused += Entry_Unfocused;

        return entry;
    }
    
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)sender;
        var name = GetCellName(entry);
        entry.Text = CalculationController.CellsController.Cells[name].Formula;
    }

    private async void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)sender;
        var name = GetCellName(entry);
        var newFormula = entry.Text;
        var backupFormula = CalculationController.CellsController.Cells[name].Formula;
            
        if (!CalculationController.CellsController.EditCell(name, newFormula))
        {
            CalculationController.CellsController.Cells[name].Formula = backupFormula;
            await DisplayAlert("Помилка", "Некоректний ввід.", "Ок");
        }
        UpdateDependentCells(name);
        UpdateCellText(name);
    }
    
    private void ReloadCells()
    {
        foreach (var key in _views.Keys)
        {
            if (_views[key] is Entry entry)
            {
                var cellName = GetCellName(entry);
                UpdateCellText(cellName);
            }
        }
    }

    private void ClearCells()
    {
        foreach (var key in _views.Keys)
        {
            if (_views[key] is Entry entry)
            {
                var cellName = GetCellName(entry);
                CalculationController.CellsController.Cells[cellName].Formula = "";
                entry.Text = "";
            }
        }
    }
    
    private void UpdateCellText(string name)
    {
        var cell = CalculationController.CellsController.Cells[name];
        var entry = (Entry)_views[name];
        entry.Text = cell.Formula == "" ? "" : cell.CalculatedFormula.ToString();
    }

    private void UpdateDependentCells(string name)
    {
        foreach (var dependentCellName in CalculationController.CellsController.Cells[name].DependentCells)
        {
            UpdateCellText(dependentCellName);
            UpdateDependentCells(dependentCellName);
        }
    }

    private string GetCellName(IView view)
    {
        return GetCellNameByPosition(grid.GetRow(view), grid.GetColumn(view));
    }

    private string GetCellNameByPosition(int row, int col)
    {
        return GetColumnName(col) + row.ToString();
    }

    private string GetColumnName(int colIndex)
    {
        var dividend = colIndex;
        var columnName = string.Empty;
        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
}