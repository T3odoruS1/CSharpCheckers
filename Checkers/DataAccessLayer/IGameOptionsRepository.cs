using Domain;

namespace DataAccessLayer;

public interface IGameOptionsRepository
{
    // crud methods

    
    // Read
    List<string> GetGameOptionsList();
    GameOptions GetGameOptions(string id);

    // Create and update
    void SaveGameOptions(string id, GameOptions options);

    
    // Delete
    void DeleteGameOptions(string id);
    
}