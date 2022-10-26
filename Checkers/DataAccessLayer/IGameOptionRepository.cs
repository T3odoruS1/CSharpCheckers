using Domain;

namespace DataAccessLayer;

public interface IGameOptionRepository
{
    
    // !!!Dont let players save game options with duplicate names!!!
    
    // Specify Db of Fs
    public string Name { get; set; }
    
    // Read all game Options and return their filenames
    List<string> GetGameOptionsList();
    
    // Get game Option by string id(filename)
    CheckerGameOptions GetGameOptions(string optionName);
    
    // Create and update
    void SaveGameOptions(CheckerGameOptions options);
    
    // Delete
    void DeleteGameOptions(string optionName);

    void UpdateGameOptions(CheckerGameOptions options);
}