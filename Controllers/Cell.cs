namespace Controllers;

public class Cell
{
    public string Formula { get; set; }
    public double CalculatedFormula { get; set; }
    public IList<string> DependentCells { get; set; }
    public IList<string> DependsOnCells { get; set; }

    public Cell()
    {
        Formula = "";
        CalculatedFormula = 0.0;
        DependentCells = new List<string>();
        DependsOnCells = new List<string>();
    }
}
