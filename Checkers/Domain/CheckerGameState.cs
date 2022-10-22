using GameBrain;

namespace Domain;


public class GameState
{
    public int Id { get; set; }
    
    public EGameSquareState[][] SerializedGameBoard { get; set; } = default!;


    public int CheckerGameId { get; set; }
    public CheckerGame? CheckerGame { get; set; }
    
}