using System.Text.Json;
using Domain;

namespace DAL.FileSystem;

public class GameStateRepositoryFileSystem
{
    
    private const string FileExtension = "json";
    private readonly string _stateDir = "." + 
                                       Path.DirectorySeparatorChar + "gameStates";
    
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;



    public int SaveState(CheckerGameState state)
    {
        state.Id = Guid.NewGuid().GetHashCode();
        FsHelpers.CheckOrCreateDirectory(_stateDir);
        var fileContent = JsonSerializer.Serialize(state);
        File.WriteAllText(FsHelpers.GetFileName(state.Id.ToString(), _stateDir), fileContent);
        return state.Id;
    }
}