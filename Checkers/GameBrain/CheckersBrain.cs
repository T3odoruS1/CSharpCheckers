using Domain;

namespace GameBrain;

public class CheckersBrain
{

    private EGameSquareState[,] _gameBoard;
    
    public CheckersBrain(GameOptions options)
    {
        var boardWidth = options.Width;
        var boardHeight = options.Height;
        // Throw exception if numbers not even
        if (boardHeight % 2 != 0 || boardWidth % 2 != 0)
        {
            throw new ArgumentException(
                "Game brains got incorrect board dimensions. They should be even numbers from 4. Your numbers: " 
                + boardHeight + " " + boardWidth);
        }
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
                        
                        if (i <= boardHeight / 2 - 2)
                        {
                            //White checker in black cell
                            _gameBoard[j, i] = EGameSquareState.White;
                        }else if (i >= boardHeight / 2 + 1)
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
                        if (i >= boardHeight / 2 + 1)
                        {
                            //Black checker in black cell
                            _gameBoard[j, i] = EGameSquareState.Black;
                        }
                        else if(i <= boardHeight / 2 - 2)
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
    public EGameSquareState[,] GetBoard()
    {
        var res = new EGameSquareState[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var i = 0; i < _gameBoard.GetLength(0); i++)
        {
            for (var j = 0; j < _gameBoard.GetLength(1); j++)
            {
                res[i, j] = _gameBoard[i, j];
            }
        }
        
        return res;

    }

    public void SetGameBoard(EGameSquareState[,] board)
    {
        try
        {
            _gameBoard = board;
        }
        catch (Exception)
        {
            Console.WriteLine("board was null");
        }
    }
    
    
    // For testing 
    public void ChangeCheckerPos(int initialX, int initialY, int newX, int newY)
    {
        (_gameBoard[initialX, initialY], _gameBoard[newX, newY]) = (_gameBoard[newX, newY], _gameBoard[initialX, initialY]);
    }
    
    
    
    
    
}