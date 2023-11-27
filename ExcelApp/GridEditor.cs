using Controllers;
using Cell = Controllers.Cell;
using Grid = Microsoft.Maui.Controls.Grid;

namespace ExcelApp;

public partial class MainPage
{
    private void CreateGrid(int rows, int cols)
    {
        while (grid.RowDefinitions.Count - 1 > rows)
        {
            DeleteLastRow();
        }
        while (grid.ColumnDefinitions.Count - 1 > cols)
        {
            DeleteLastColumn();
        }
        ClearCells();
        for (var row = grid.RowDefinitions.Count; row <= rows; row++)
        {
            AddRow();
        }
        for (var col = grid.ColumnDefinitions.Count; col <= cols; col++)
        {
            AddColumn();
        }
    }
    
    private void AddRow()
    {
        grid.RowDefinitions.Add(new RowDefinition());

        var colsCount = grid.ColumnDefinitions.Count;
        var rowsCount = grid.RowDefinitions.Count - 1;

        var label = new Label
        {
            Text = rowsCount.ToString(),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        Grid.SetRow(label, rowsCount);
        Grid.SetColumn(label, 0);
        grid.Children.Add(label);
        _views[$"0{rowsCount}"] = label;

        for (var col = 1; col < colsCount; col++)
        {
            var entry = CreateEntry();
            Grid.SetRow(entry, rowsCount);
            Grid.SetColumn(entry, col);
            grid.Children.Add(entry);
            _views[GetCellName(entry)] = entry;
            CalculationController.CellsController.Cells[GetCellName(entry)] = new Controllers.Cell();
        }
    }
    
    private void AddColumn()
    {
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        var colsCount = grid.ColumnDefinitions.Count - 1;
        var rowsCount = grid.RowDefinitions.Count;

        var label = new Label
        {
            Text = GetColumnName(colsCount),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        Grid.SetRow(label, 0);
        Grid.SetColumn(label, colsCount);
        grid.Children.Add(label);
        _views[$"{GetColumnName(colsCount)}0"] = label;

        for (var row = 1; row < rowsCount; row++)
        {
            var entry = CreateEntry();
            Grid.SetRow(entry, row);
            Grid.SetColumn(entry, colsCount);
            grid.Children.Add(entry);
            _views[GetCellName(entry)] = entry;
            CalculationController.CellsController.Cells[GetCellName(entry)] = new Controllers.Cell();
        }
    }
    
    private void DeleteLastRow()
    {
        if (grid.RowDefinitions.Count > 1)
        {
            var lastRowIndex = grid.RowDefinitions.Count - 1;
            var actualColumnsCount = grid.ColumnDefinitions.Count - 1;
            var name = $"0{lastRowIndex}";

            grid.RowDefinitions.RemoveAt(lastRowIndex);
            grid.Children.Remove(_views[name]);
            _views.Remove(name);
            for (var col = 1; col < actualColumnsCount + 1; col++)
            {
                name = GetCellNameByPosition(lastRowIndex, col);
                grid.Children.Remove(_views[name]);
                
                foreach (var dependentCellName in CalculationController.CellsController.Cells[name].DependentCells)
                {
                    var dependentCell = CalculationController.CellsController.Cells[dependentCellName];
                    dependentCell.Formula = dependentCell.Formula.Replace(name, "");
                    dependentCell.CalculatedFormula = 0.0;
                }
                    
                _views.Remove(name);
                CalculationController.CellsController.Cells.Remove(name);
            }
        }
    }
    
    private void DeleteLastColumn()
    {
        if (grid.ColumnDefinitions.Count > 1)
        {
            var lastColumnIndex = grid.ColumnDefinitions.Count - 1;
            var actualRowsCount = grid.RowDefinitions.Count - 1;
            var name = $"{GetColumnName(lastColumnIndex)}0";

            grid.ColumnDefinitions.RemoveAt(lastColumnIndex);
            grid.Children.Remove(_views[name]);
            _views.Remove(name);
            for (var row = 1; row < actualRowsCount + 1; row++)
            {
                name = GetCellNameByPosition(row, lastColumnIndex);
                grid.Children.Remove(_views[name]);
                foreach (var dependentCellName in CalculationController.CellsController.Cells[name].DependentCells)
                {
                    var dependentCell = CalculationController.CellsController.Cells[dependentCellName];
                    dependentCell.Formula = dependentCell.Formula.Replace(name, "");
                    dependentCell.CalculatedFormula = 0.0;
                }
                
                _views.Remove(name);
                CalculationController.CellsController.Cells.Remove(name);
            }
        }
    }
    
}