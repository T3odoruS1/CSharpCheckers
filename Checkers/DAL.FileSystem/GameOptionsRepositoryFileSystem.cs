using DataAccessLayer;
using Domain;
using System.Text.Json;



namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionsRepository
{

    private const string FileExtension = "json";
    private readonly UniversalFunctionsForFileSystem _funcs = new UniversalFunctionsForFileSystem();

    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar + "options";
    public List<string> GetGameOptionsList()
    {
        _funcs.CheckOrCreateDirectory(_optionsDir);
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
        var fileContent = File.ReadAllText(_funcs.GetFileName(id, _optionsDir));
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
        _funcs.CheckOrCreateDirectory(_optionsDir);
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(_funcs.GetFileName(id, _optionsDir), fileContent);
    }
    public void DeleteGameOptions(string id)
    {
        File.Delete(_funcs.GetFileName(id, _optionsDir));
    }
}