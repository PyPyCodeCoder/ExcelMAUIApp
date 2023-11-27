using System.Collections;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Controllers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

namespace ExcelApp;

public class GoogleDriveManager
{
    private const string AppDataFolderName = "appDataFolder";
    private const string DefaultDataStoreFolderName = ".authdata";
    string ClientSecretsFilePath = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), "client_secrets.json");
    
    private static DriveService? driveService;
    
    public bool isClientLoggedIn = false;
    
    public async Task InitClient()
    {
        if (driveService is not null)
        {
            return;
        }
        
        var clientSecrets = (await GoogleClientSecrets.FromFileAsync(ClientSecretsFilePath)).Secrets;
        var dataStore = new FileDataStore(DefaultDataStoreFolderName);
        dataStore.ClearAsync().Wait();
        var scopes = new[]
        {
            DriveService.Scope.DriveAppdata,
            DriveService.Scope.DriveFile
        };

        var credential =
            await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets
                , scopes
                , "user"
                , CancellationToken.None
                , dataStore);

        driveService = new DriveService(new DriveService.Initializer
        {
            HttpClientInitializer = credential
        });
        
        isClientLoggedIn = true;
    } 
    
    public void DeInitClient()
    {
        if (driveService is null)
        {
            return;
        }
        
        var tokenResponse = driveService.HttpClientInitializer as UserCredential;
        if (tokenResponse != null)
        {
            try
            {
                tokenResponse.RevokeTokenAsync(CancellationToken.None).Wait();
                Console.WriteLine("Access token revoked successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error revoking access token: {ex.Message}");
            }
        }
        
        driveService.Dispose();
        driveService = null;
        isClientLoggedIn = false;
    }
    
    public async Task<List<(string, string)>> ListFiles()
    {
        if (driveService is null)
        {
            throw new InvalidOperationException("Сервіс диску не ініціалізовано");
        }

        var request = driveService.Files.List();
        request.Spaces = AppDataFolderName;
        request.Fields = "files(id, name)";

        var result = await request.ExecuteAsync();

        var list = new List<(string, string)>();

        foreach (var file in result.Files)
        {
            list.Add(($"{file.Name}", $"{file.Id}"));
        }

        return list;
    }
    
    public async Task<string> WriteFile(string fileName, FileRepresentation representation)
    {
        if (driveService is null)
        {
            throw new InvalidOperationException("Сервіс диску не ініціалізовано");
        }

        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = $"{fileName}.json",
            Parents = new List<string>()
            {
                AppDataFolderName
            }
        };
        
        string jsonContent = JsonSerializer.Serialize(representation);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
        var request = driveService.Files.Create(fileMetadata, stream, "application/json");
        request.Fields = "id";

        var uploadResult = await request.UploadAsync();

        if (uploadResult.Exception is not null)
        {
            throw new Exception($"Помилка під час вивантаження файлу: {uploadResult.Exception.Message}");
        }

        return request.ResponseBody.Id;
    }
    
    public async Task<FileRepresentation> ReadFileContent(string fileId)
    {
        if (driveService is null)
        {
            throw new InvalidOperationException("Сервіс диску не ініціалізовано");
        }

        var request = driveService.Files.Get(fileId);

        using var stream = new MemoryStream();

        var downloadResult = await request.DownloadAsync(stream);

        if (downloadResult.Exception is not null)
        {
            throw new Exception($"Помилка під час завантаження файлу: {downloadResult.Exception.Message}");
        }
        
        stream.Position = 0;

        using var streamReader = new StreamReader(stream);
        string fileContent = streamReader.ReadToEnd();
        
        try
        {
            return JsonSerializer.Deserialize<FileRepresentation>(fileContent);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Помилка під час десеріалізації JSON: {ex.Message}");
        }
    }

}