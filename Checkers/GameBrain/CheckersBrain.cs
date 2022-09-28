namespace GameBrain;

public class CheckersBrain
{

    private readonly EGameSquareState[,] _gameBoard;
    
    public CheckersBrain(int boardWidth = 8, int boardHeight = 8)
    {
        _gameBoard = new EGameSquareState[boardWidth, boardHeight];
        
        
        // Printing out each board line
        for (var i = 0; i < boardHeight; i++)
        {
        
            // Printing out each board row
            for (var j = 0; j < boardWidth; j++)
            {
                
                // In this case first cell is while and no checkers in it
                if (i == 0 || i % 2 == 0)
                {
                    if (j == 0 || j % 2 == 0)
                    {
                        //White cell
                        _gameBoard[j, i] = EGameSquareState.Unplayable;
                    }
                    else
                    {
                        
                        //Black cell
                        
                        if (i < 3)
                        {
                            //White checker in black cell
                            _gameBoard[j, i] = EGameSquareState.White;
                        }else if (i > boardHeight - 4)
                        {
                            //Black checker in black cell
                            _gameBoard[j, i] = EGameSquareState.Black;
                        }
                        
                        else
                        {
                            // Black cell
                            _gameBoard[j, i] = EGameSquareState.Empty;
                        }
                        
                    }
                }
                else
                {
                    if (j == 0 || j % 2 == 0)
                    {
                        
                        //Black cell
                        if (i > boardHeight - 4)
                        {
                            //Black checker in black cell
                            _gameBoard[j, i] = EGameSquareState.Black;
                        }
                        else if(i < 3)
                        {
                            // White checker in black cell
                            _gameBoard[j, i] = EGameSquareState.White;
                        }
                        else
                        {
                            // Black cell
                            _gameBoard[j, i] = EGameSquareState.Empty;
                        }
                    }
                    else
                    {
                        //White cell
                        _gameBoard[j, i] = EGameSquareState.Unplayable;

                    }
                }
            }

        }
        
    }

    // Deep copy of the array
    public EGameSquareState?[,] GetBoard()
    {
        var res = new EGameSquareState?[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var i = 0; i < _gameBoard.GetLength(0); i++)
        {
            for (var j = 0; j < _gameBoard.GetLength(1); j++)
            {
                res[i, j] = _gameBoard[i, j];
            }
        }
        
        return res;

    }
    
    
    
}