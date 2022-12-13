using Domain;

namespace DataAccessLayer;

public interface IGameRepository
{
    // Repository classification (Fs or Db)
    public string Name { get; set; }
    
    
    /// <summary>
    /// Method for getting a list of all the saved games
    /// </summary>
    /// <returns>List of all saved games</returns>
    List<CheckerGame> GetAllGamesList();
    
    /// <summary>
    /// Get specific game by id
    /// </summary>
    /// <param name="id">Game id</param>
    /// <returns>Game with this id</returns>
    CheckerGame GetGameById(int id);
    
    
    /// <summary>
    /// Save checker game
    /// </summary>
    /// <param name="game">Game to be saved</param>
    /// <returns>Id assigned to this game after saving</returns>
    int SavaGame(CheckerGame game);

    
    /// <summary>
    /// Update already saved game
    /// </summary>
    /// <param name="game">Game to be updated</param>
    void UpdateGame(CheckerGame game);
    
    
    /// <summary>
    /// Delete game by id
    /// </summary>
    /// <param name="id">id of the game to be deleted</param>
    void DeleteGameById(int id);

}