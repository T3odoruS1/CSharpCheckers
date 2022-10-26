using System.Text.Json;
using DataAccessLayer;
using Domain;

namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionRepository
{
    private const string FileExtension = "json";
    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar 
                                          + "optionSaves";
    
    
    // File system repo identifier
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;
    
    
    
    public List<string> GetGameOptionsList()
    {
        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        
        var res = new List<string>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _optionsDir, "*" + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    public CheckerGameOptions GetGameOptions(string optionName)
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName(optionName, _optionsDir));
        var options = JsonSerializer.Deserialize<CheckerGameOptions>(fileContent);

        if (options == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return options;
    }

    public void SaveGameOptions(CheckerGameOptions options)
    {
        if (!OptionNameAvailable(options.Name))
        {
            throw new ArgumentException($"Error while saving game options. Game options name {options.Name} is already taken");
        }
        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(FsHelpers.GetFileName(options.Name, _optionsDir), fileContent);
    }

    public void DeleteGameOptions(string optionName)
    {
        File.Delete(FsHelpers.GetFileName(optionName, _optionsDir));

    }

    public void UpdateGameOptions(CheckerGameOptions options)
    {
        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(FsHelpers.GetFileName(options.Name, _optionsDir), fileContent);
    }
    

    public bool OptionNameAvailable(string name)
    {
        return !GetGameOptionsList().Contains(name);
    }
}