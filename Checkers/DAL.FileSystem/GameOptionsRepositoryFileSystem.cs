using DataAccessLayer;
using Domain;
using System.Text.Json;



namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionsRepository
{

    private const string FileExtension = "json";

    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar + "options";
    public List<string> GetGameOptionsList()
    {
        CheckOrCreateDirectory();
        var res = new List<string>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _optionsDir, "*" + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }
    
    public GameOptions GetGameOptions(string id)
    
    // id - filename
    {
        var fileContent = File.ReadAllText(GetFileName(id));
        var options = JsonSerializer.Deserialize<GameOptions>(fileContent);

        if (options == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return options;
    }

    public void SaveGameOptions(string id, GameOptions options)
    
    // id - filename
    {
        CheckOrCreateDirectory();
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(GetFileName(id), fileContent);
    }

    private string GetFileName(string id)
    
    // id - filename
    {
        return _optionsDir +
               Path.DirectorySeparatorChar +
               id + "." + FileExtension;


    }
    
    public void DeleteGameOptions(string id)
    {
        File.Delete(GetFileName(id));
    }

    private void CheckOrCreateDirectory()
    {
        if (!Directory.Exists(_optionsDir))
        {
            Directory.CreateDirectory(_optionsDir);
        }
    }
}