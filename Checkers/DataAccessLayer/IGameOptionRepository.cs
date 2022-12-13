using Domain;

namespace DataAccessLayer;

public interface IGameOptionRepository
{
    
    // !!!Dont let players save game options with duplicate names!!!
    
    // Specify Db of Fs
    public string Name { get; set; }
    
    // Read all game Options and return their filenames
    List<CheckerGameOptions> GetGameOptionsList();
    
    // Get game Option by string id(filename)
    CheckerGameOptions GetOptionsById(int id);
    
    // Create and update
    int SaveGameOptions(CheckerGameOptions options);
    
    // Delete
    void DeleteOptionsById(int id);

    void UpdateGameOptions(CheckerGameOptions options);

}