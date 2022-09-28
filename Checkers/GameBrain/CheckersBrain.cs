namespace GameBrain;

public class CheckersBrain
{

    private readonly EGameSquareState[,] _gameBoard;
    
    public CheckersBrain(int boardWidth = 8, int boardHeight = 8)
    {
        _gameBoard = new EGameSquareState[boardWidth, boardHeight];
    }
    
    
    
}