using Domain;

namespace DataAccessLayer;

public interface IGameRepository
{
    // !!! Don't let players save games with duplicate names !!!
    
    // Repository classification (Fs or Db)
    public string Name { get; set; }
    
    
    // Return all game names
    List<CheckerGame> GetAllGamesList();
    
    
    CheckerGame GetGameById(int id);
    
    
    // Save game
    int SavaGame(CheckerGame game);

    
    // Update game
    void UpdateGame(CheckerGame game);
    
    
    // Delete game from system by provided name
    void DeleteGameById(int id);

}