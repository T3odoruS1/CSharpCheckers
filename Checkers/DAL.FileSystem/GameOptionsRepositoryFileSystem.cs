using System.Text.Json;
using System.Xml;
using DataAccessLayer;
using Domain;

namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionRepository
{
    
    // If file system repo filename and entity id are the same
    private const string FileExtension = "json";
    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar 
                                          + "optionSaves";
    
    
    // File system repo identifier
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;
    
    
    
    public List<CheckerGameOptions> GetGameOptionsList()
    {
        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        
        var res = new List<CheckerGameOptions>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _optionsDir, "*" + FileExtension))
        {
            Console.WriteLine("Filename without extension");
            Console.WriteLine(Path.GetFileNameWithoutExtension(fileName));
            
            res.Add(GetOptionsById(
                Convert.ToInt32(
                    Path.GetFileNameWithoutExtension(
                        fileName))));
        }

        return res;
    }

    public CheckerGameOptions GetOptionsById(int id)
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName(id.ToString(), _optionsDir));
        var options = JsonSerializer.Deserialize<CheckerGameOptions>(fileContent);

        if (options == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return options;
    }

    public int SaveGameOptions(CheckerGameOptions options)
    {
        options.Id = Guid.NewGuid().GetHashCode();

        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(FsHelpers.GetFileName(options.Id.ToString(), _optionsDir), fileContent);
        return options.Id;
    }

    public void DeleteOptionsById(int id)
    {
        File.Delete(FsHelpers.GetFileName(id.ToString(), _optionsDir));

    }

    public void UpdateGameOptions(CheckerGameOptions options)
    {
        FsHelpers.CheckOrCreateDirectory(_optionsDir);
        var fileContent = JsonSerializer.Serialize(options);
        File.WriteAllText(FsHelpers.GetFileName(options.Id.ToString(), _optionsDir), fileContent);
    }
}