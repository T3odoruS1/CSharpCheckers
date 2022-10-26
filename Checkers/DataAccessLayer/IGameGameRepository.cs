using Domain;

namespace DataAccessLayer;

public interface IGameGameRepository
{
    // !!! Don't let players save games with duplicate names !!!
    
    // Repository classification (Fs or Db)
    public string Name { get; set; }
    
    
    // Return all game names
    List<string> GetAllGameNameList();
    
    
    // Get game name by game name
    CheckerGame GetGame(string name);
    
    
    // Save game
    void SavaGame(CheckerGame game);

    
    // Update game
    void UpdateGame(CheckerGame game);


    
    // Add new game state 
    void AddNewGameState(CheckerGameState state);
    
    
    // Delete game from system by provided name
    void DeleteGameByName(string name);
}