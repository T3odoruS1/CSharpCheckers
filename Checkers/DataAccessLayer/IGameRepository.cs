using Domain;
using GameBrain;

namespace DataAccessLayer;

public interface IGameRepository
{
    // crud methods

    
    
    // For game options
    
    // Read all game Options and return their filenames
    List<string> GetGameOptionsList();
    
    // Get game Option by string id(filename)
    GameOptions GetGameOptions(string id);

    
    // Create and update
    void SaveGameOptions(string id, GameOptions options);

    
    // Delete
    void DeleteGameOptions(string id);


    
    
    // Same for game boards
    
    // Read all game boards and return their names
    List<string> GetGameBoardNames();

    
    // Get game board by string name
    Dictionary<EGameSquareState[,], GameOptions> GetGameBoard(string id);

    // Sava game board
    void SaveGameState(string id, EGameSquareState[,] board, GameOptions options);

    
    // Delete game board
    void DeleteGameState(string id);
    
    
}