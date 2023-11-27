using System.Text.Json;
using System.Text.Json.Serialization;

using Cell = Controllers.Cell;

namespace ExcelApp;

public struct FileRepresentation {
    [JsonInclude]
    public int CountRow;

    [JsonInclude]
    public int CountColumn;

    [JsonInclude]
    public IDictionary<string, Cell> Cells;

    public FileRepresentation(int rows, int columns, IDictionary<string, Cell> cells) {
        CountColumn = columns;
        CountRow = rows;
        Cells = cells;
    }
}