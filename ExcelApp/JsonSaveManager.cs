using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Maui.Storage;
using Cell = Controllers.Cell;

namespace ExcelApp;

public class JsonSaveManager
{
    private IFileSaver _saver;
    private IFilePicker _loader;
    private FileResult _openedFile;

    public JsonSaveManager() {
        _saver = FileSaver.Default;
        _loader = FilePicker.Default;
    }
    
    public async Task SaveAs(FileRepresentation representation) {
        var text = JsonSerializer.Serialize<FileRepresentation>(representation);
        using var stream = new MemoryStream(Encoding.Default.GetBytes(text));
        _openedFile = new FileResult((await _saver.SaveAsync("NewTable.json", stream, new CancellationTokenSource().Token)).FilePath);
    }
    
    public async Task<FileRepresentation> Load()
    {
        _openedFile = await _loader.PickAsync();
        using var fileStream = await _openedFile.OpenReadAsync();
        return await JsonSerializer.DeserializeAsync<FileRepresentation>(fileStream);
    }
}
