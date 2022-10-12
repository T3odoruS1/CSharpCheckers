using GameBrain;

namespace DataAccessLayer;

public interface IGameStateRepository
{
    // crud methods

    
    // Read
    List<string> GetGameStatesList();
    
    
    EGameSquareState[,] GetGameState(string id);

    // Create and update
    void SaveGameState(string id, EGameSquareState[,] options);

    
    // Delete
    void DeleteGameStates(string id);

    

}