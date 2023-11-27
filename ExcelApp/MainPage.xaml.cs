using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;

using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using CommunityToolkit.Maui.Storage;

using Controllers;
using Cell = Controllers.Cell;
using Grid = Microsoft.Maui.Controls.Grid;


namespace ExcelApp;

public partial class MainPage
{
    private const int CountRow = 10;
    private const int CountColumn = 10;
    
    private readonly IDictionary<string, IView> _views;
    private readonly JsonSaveManager _saveManager;
    private GoogleDriveManager _googleDriveManager;

    public MainPage()
    {
        _views = new Dictionary<string, IView>();
        _saveManager = new JsonSaveManager();
        _googleDriveManager = new GoogleDriveManager();
        InitializeComponent();
        CreateGrid(CountRow, CountColumn);
    }
    
    private async void HelpButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Довідка",
            "Лабораторна робота 1. Студента Левчука Валерія",
            "Ок");
    }
    
    private async void ReadButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            FileRepresentation representation = await _saveManager.Load();
            
            CreateGrid(representation.CountRow, representation.CountColumn);
            
            
            CalculationController.CellsController.Cells = representation.Cells;
            
            ReloadCells();
            
            await DisplayAlert("Успіх", "Файл успішно завантажено", "Ок");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка під час завантаження файлу:", $"{ex.Message}", "Oк");
        }
    }

    private async void ReadFromGDriveButton_Clicked(object sender, EventArgs e)
    {
        if (!_googleDriveManager.isClientLoggedIn)
        {
            try
            {
                await _googleDriveManager.InitClient();
                await DisplayAlert("Успіх", "Вхід успішний", "Oк");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка під час входу:", $"{ex.Message}", "Oк");
            }
        }
    
        var filesList = await _googleDriveManager.ListFiles();
        var files = string.Join("", filesList);
    
        await DisplayAlert("Файли в Google Drive:", $"{files}", "Ok");
        
        var IdList = new List<string>();
        foreach (var file in filesList)
        {
            IdList.Add(file.Item2);
        }
        
        var fileId = await DisplayActionSheet("Оберіть файл для завантаження", "Скасувати", null, IdList.ToArray());
    
        if (string.IsNullOrWhiteSpace(fileId))
        {
            await DisplayAlert("Помилка", "Не введено Id файлу", "Oк");
        }
        else
        {
            try
            {
                FileRepresentation representation = await _googleDriveManager.ReadFileContent(fileId);
                
                CreateGrid(representation.CountRow, representation.CountColumn);
                
                CalculationController.CellsController.Cells = representation.Cells;
                
                ReloadCells();
    
                await DisplayAlert("Успіх", "Файл успішно завантажено", "Ок");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка під час завантаження файлу:", $"{ex.Message}", "Oк");
            }
        }
    }
    
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            FileRepresentation representation = new FileRepresentation(grid.RowDefinitions.Count - 1, grid.ColumnDefinitions.Count - 1, CalculationController.CellsController.Cells);
            await _saveManager.SaveAs(representation);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка під час збереження файлу:" , $"{ex.Message}", "Oк");
        }
    }
    
    private async void SaveToGDriveButton_Clicked(object sender, EventArgs e)
    {
        if (!_googleDriveManager.isClientLoggedIn)
        {
            try
            {
                await _googleDriveManager.InitClient();
                await DisplayAlert("Успіх", "Вхід успішний", "Oк");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка під час входу:", $"{ex.Message}", "Oк");
            }
        }
        
        string fileName = await DisplayPromptAsync("Ім'я файлу", "Введіть ім'я файлу для збереження:");
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            await DisplayAlert("Помилка", "Некоректне ім'я файлу", "Oк");
        }
        else
        {
            try
            {
                FileRepresentation representation = new FileRepresentation(grid.RowDefinitions.Count - 1, grid.ColumnDefinitions.Count - 1, CalculationController.CellsController.Cells);
                string id = await _googleDriveManager.WriteFile(fileName, representation);
                await DisplayAlert("Успіх", $"Створено файл із id: {id}", "Oк");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка під час збереження файлу:", $"{ex.Message}", "Oк");
            }
        }
    }
    
    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        _googleDriveManager.DeInitClient();
        var answer = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти?",
            "Так", "Ні");
        if (answer) Environment.Exit(0);
    }

    private void AddRowButton_Clicked(object sender, EventArgs e)
    {
        AddRow();
    }

    private void DeleteRowButton_Clicked(object sender, EventArgs e)
    {
        DeleteLastRow();
        ReloadCells();
    }
    
    private void AddColumnButton_Clicked(object sender, EventArgs e)
    {
        AddColumn();
    }

    private void DeleteColumnButton_Clicked(object sender, EventArgs e)
    {
        DeleteLastColumn();
        ReloadCells();
    }
    
}
