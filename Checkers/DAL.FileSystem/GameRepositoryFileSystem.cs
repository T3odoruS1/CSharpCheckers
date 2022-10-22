using DataAccessLayer;
using Domain;
using System.Text.Json;
using GameBrain;


namespace DAL.FileSystem;

public class GameRepositoryFileSystem : IGameRepository
{

    private const string FileExtension = "json";
    private readonly UniversalFunctionsForFileSystem _funcs = new UniversalFunctionsForFileSystem();

    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar + "options";
    private readonly string _stateDir = "." + 
                                          Path.DirectorySeparatorChar + "GameSave";
    // List of all saved game opions
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
    
    // Get particular game option by file name
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

    
    // Save game option into json file
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

    public List<string> GetGameBoardNames()
    {
        _funcs.CheckOrCreateDirectory(_stateDir);
        var res = new List<string>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _stateDir, "*" + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    
    // Returns dict where key is game board and value is game options
    public Dictionary<EGameSquareState[,], string> GetGameBoard(string id)
    {
        var fileContent = File.ReadAllText(_funcs.GetFileName(id, _stateDir));
        var boardAndGameOptionName = JsonSerializer.Deserialize<Dictionary<string, EGameSquareState[][]>>(fileContent);
        var convertedBoard = _funcs.JaggedTo2D<EGameSquareState>(boardAndGameOptionName!.Values.First());
        if (boardAndGameOptionName == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        var ret = new Dictionary<EGameSquareState[,], string> { { convertedBoard, boardAndGameOptionName.Keys.First() } };
        return ret;
    }

    
    // Save Dict where key is jagged game board and value is options filename
    public void SaveGameState(string id, EGameSquareState[,] board, string options)
    {
        _funcs.CheckOrCreateDirectory(_optionsDir);
        var jaggedBoard = _funcs.ToJaggedArray(board);
        var dict = new Dictionary<string, EGameSquareState[][]>();
        dict.Add(options, jaggedBoard);
        var fileContent = JsonSerializer.Serialize(dict);
        File.WriteAllText(_funcs.GetFileName(id, _stateDir), fileContent);

    }

    public void DeleteGameState(string id)
    {
        File.Delete(_funcs.GetFileName(id, _stateDir));
    }
}