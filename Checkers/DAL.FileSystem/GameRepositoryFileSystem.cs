using System.Text.Json;
using DataAccessLayer;
using Domain;

namespace DAL.FileSystem;

public class GameRepositoryFileSystem : IGameRepository
{
    private const string FileExtension = "json";
    private readonly string _gameDir = "." + 
                                        Path.DirectorySeparatorChar + "gameSaves";
    
    // File system repo identifier
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;
    
    
    
    public List<string> GetAllGameNameList()
    {
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var res = new List<string>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _gameDir, "*" + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }
        return res;
    }

    public CheckerGame GetGameByName(string name)
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName(name, _gameDir));
        var checkerGame = JsonSerializer.Deserialize<CheckerGame>(fileContent);
        var optionName = checkerGame!.GameOptions!.Name;
        var optionsRepo = new GameOptionsRepositoryFileSystem();
        checkerGame.GameOptions = optionsRepo.GetGameOptions(optionName);
        if (checkerGame == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return checkerGame;
    }

    public CheckerGame GetGameById(int id)
    {
        throw new NotImplementedException();
    }


    // Save game. Throws exception if game name is already taken
    public void SavaGame(CheckerGame game)
    {
        
        // For updating separate function
        if (!GameNameAvailable(game.Name))
        {
            throw new ArgumentException($"You provided a new game with name {game.Name}. This game name is taken. Choose another.");
        }
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var fileContent = JsonSerializer.Serialize(game);
        File.WriteAllText(FsHelpers.GetFileName(game.Name, _gameDir), fileContent);
    }

    
    // Update game. Same as SaveGame but doesn't throw exception then name is taken.
    // !!! Not tested yet. !!!
    // TODO test when gameplay is implemented
    public void UpdateGame(CheckerGame game)
    {
        FsHelpers.CheckOrCreateDirectory(_gameDir);
        var fileContent = JsonSerializer.Serialize(game);
        File.WriteAllText(FsHelpers.GetFileName(game.Name, _gameDir), fileContent);    }

    
    // Add new game
    // !!! Not tested yet !!!
    // TODO test when gameplay is implemented
    public void AddNewGameState(CheckerGameState state)
    {
        var checkersGame = state.CheckerGame!;
        checkersGame.CheckerGameStates!.Add(state);
        UpdateGame(checkersGame);
    }
    
    
    // Delete game by its name
    public void DeleteGameByName(string name)
    {
        File.Delete(FsHelpers.GetFileName(name, _gameDir));
    }

    
    // Checks if game name is already taken. Returns true if available
    public bool GameNameAvailable(string name)
    {
        return !GetAllGameNameList().Contains(name);
    }
}