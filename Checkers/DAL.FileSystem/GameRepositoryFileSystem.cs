using System.Text.Json;
using DataAccessLayer;
using Domain;

namespace DAL.FileSystem;

public class GameRepositoryFileSystem : IGameRepository
{
    
    // !!! In this implementation filename is a hashcode of UUID. Filename and game id are the same.
    private const string FileExtension = "json";
    private readonly string _gameDir = "." + 
                                        Path.DirectorySeparatorChar + "gameSaves";
    
    // File system repo identifier
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;
    
    
    
    public List<CheckerGame> GetAllGamesList()
    {
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var games = new List<CheckerGame>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _gameDir, "*" + FileExtension))
        {
            games.Add(GetGameById(Int32.Parse(fileName))); 
        }
        return games;
    }

    public CheckerGame GetGameById(int id)
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName(id.ToString(), _gameDir));
        var checkerGame = JsonSerializer.Deserialize<CheckerGame>(fileContent);
        var optionsId = checkerGame!.GameOptions!.Id;
        var optionsRepo = new GameOptionsRepositoryFileSystem();
        checkerGame.GameOptions = optionsRepo.GetOptionsById(optionsId);
        if (checkerGame == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return checkerGame;
    }


    // Save game
    public int SavaGame(CheckerGame game)
    {

        game.Id = Guid.NewGuid().GetHashCode();
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var fileContent = JsonSerializer.Serialize(game);
        File.WriteAllText(FsHelpers.GetFileName(game.Id.ToString(), _gameDir), fileContent);
        return game.Id;
    }

    
    // Update game
    public void UpdateGame(CheckerGame game)
    {
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var fileContent = JsonSerializer.Serialize(game);
        File.WriteAllText(FsHelpers.GetFileName(game.Id.ToString(), _gameDir), fileContent);    }

    
    
    
    // Delete game by its name
    public void DeleteGameById(int id)
    {
        File.Delete(FsHelpers.GetFileName(id.ToString(), _gameDir));
    }

    
}