using Domain;

namespace GameBrain;

public class CheckersBrain
{

    private EGameSquareState[,] _gameBoard;

    private readonly CheckerGameOptions _checkerGameOptions;
    private bool _gameOver = false;
    private bool _nextMoveByBlack;
    private bool _takingDone;
    private bool _canDoAnotherTake;
    
    public CheckersBrain(CheckerGameOptions options)
    {
        var boardWidth = options.Width;
        var boardHeight = options.Height;
        _nextMoveByBlack = !options.WhiteStarts;
        this._checkerGameOptions = options;
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

    public void SetGameBoard(EGameSquareState[,] board, CheckerGameState state)
    {
        try
        {
            _gameBoard = board;
        }
        catch (Exception)
        {
            Console.WriteLine("board was null");
        }

        _nextMoveByBlack = state.NextMoveByBlack;
    }
    

    public bool MoveIsPossible(int iniX, int iniY, int destX, int destY)
    {
        
        // TODO take into consideration mandatory taking rule.
        
        // Check if coordinates are in range of game board.
        if (iniX > _checkerGameOptions.Height || iniX < 0 ||
            destX > _checkerGameOptions.Height || destX < 0 ||
            iniY > _checkerGameOptions.Width || iniY < 0 ||
            destY > _checkerGameOptions.Width || destY < 0)
        {
            return false;
        }
        
        // If game square is not empty return false
        if (_gameBoard[destX, destY] != EGameSquareState.Empty)
        {
            // Console.WriteLine("Not empty");
            return false;
        }
        
        
        // Validation for regular checkers.
        if (_gameBoard[iniX, iniY] == EGameSquareState.Black || _gameBoard[iniX, iniY] == EGameSquareState.White)
        {
            if (Math.Abs(destX - iniX) == 2 && Math.Abs(destY - iniY) == 2)
            { 
                    // Check if there is checker to take.
                if (_gameBoard[iniX, iniY] == EGameSquareState.Black)
                {
                    if (_gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.White &&
                        _gameBoard[destX, destY] == EGameSquareState.Empty)
                    {
                        if (destY > iniY)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                else if (_gameBoard[iniX, iniY] == EGameSquareState.White)
                {
                    if (_gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.Black &&
                        _gameBoard[destX, destY] == EGameSquareState.Empty)
                    {
                        if (destY < iniY)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                
            }
            else if (!_canDoAnotherTake)
            {
                if (_gameBoard[iniX, iniY] == EGameSquareState.Black)
                {
                    try
                    {
                        if (destX == iniX - 1 && destY == iniY - 1)
                        {
                            return true;
                        }
                        
                        
                        try
                        {
                            return destX == iniX + 1 && destY == iniY - 1;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return false;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        try
                        {
                            return destX == iniX + 1 && destY == iniY - 1;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return false;
                        }
                    }
                }

                if (_gameBoard[iniX, iniY] == EGameSquareState.White)
                {
                    
                    try
                    {
                        if (destX == iniX - 1 && destY == iniY + 1)
                        {
                            return true;
                        }
                        
                        
                        try
                        {
                            return destX == iniX + 1 && destY == iniY + 1;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return false;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        try
                        {
                            return destX == iniX + 1 && destY == iniY + 1;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return false;
                        }
                    }
                }
            }
            
            
            // King diagonal movement
        }else if (_gameBoard[iniX, iniY] == EGameSquareState.WhiteKing ||
                  _gameBoard[iniX, iniY] == EGameSquareState.BlackKing)
        {
            // |𝑥1−𝑥2|=|𝑦1−𝑦2| - condition of 2 points being diagonally aligned 
            if (Math.Abs(iniX - destX) == Math.Abs(iniY - destY))
            {
                return true;
            }
        }

        return false;
    }

    public bool HasMoves(int x, int y)
    {
        if (_gameBoard[x,y] != EGameSquareState.Unplayable && _gameBoard[x,y] != EGameSquareState.Empty)
        {
            for (int testX = 0; testX < _checkerGameOptions.Width; testX++)
            {
                for (int testY = 0; testY < _checkerGameOptions.Height; testY++)
                {
                    if (MoveIsPossible(x, y, testX, testY))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CanTake(int x, int y)
    {
        if (_gameBoard[x, y] == EGameSquareState.Black || _gameBoard[x, y] == EGameSquareState.White)
        {
            if (_gameBoard[x, y] == EGameSquareState.Black)
            {
                
                try
                {
                    if (MoveIsPossible(x, y, x - 2, y - 2))
                    {
                        return true;
                    }
                        
                        
                    try
                    {
                        return MoveIsPossible(x, y, x + 2, y - 2);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    try
                    {
                        return MoveIsPossible(x, y, x + 2, y - 2);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }
            if(_gameBoard[x, y] == EGameSquareState.White)
            {
                
                
                try
                {
                    if (MoveIsPossible(x, y, x - 2, y + 2))
                    {
                        return true;
                    }
                        
                        
                    try
                    {
                        return MoveIsPossible(x, y, x + 2, y + 2);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    try
                    {
                        return MoveIsPossible(x, y, x + 2, y + 2);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }
            
        }
        
        // TODO if king?
        
        return false;
    }


    public void MoveChecker(int iniX, int iniY, int destX, int destY)
    {
        if (!MoveIsPossible(iniX, iniY, destX, destY))
        {
            throw new ArgumentException("Tried to move checker but suggested move is not valid" + $"iniX{iniX}, iniY{iniY}, destX{destX}, destY{destY}");
        }

        if ((_gameBoard[iniX, iniY] == EGameSquareState.Black ||
             _gameBoard[iniX, iniY] == EGameSquareState.BlackKing) && !_nextMoveByBlack ||
            (_gameBoard[iniX, iniY] == EGameSquareState.White ||
             _gameBoard[iniX, iniY] == EGameSquareState.WhiteKing) && _nextMoveByBlack)
        {
            throw new ArgumentException("Tried to move a checker that is not to be moved on this turn. This checker should not be clickable. Next move by black" + _nextMoveByBlack);
        }


        if (_gameBoard[iniX, iniY] == EGameSquareState.Black || _gameBoard[iniX, iniY] == EGameSquareState.White)
        {
            
            // Taking part for regular
            if (Math.Abs(iniX - destX) == 2 && Math.Abs(iniY - destY) == 2)
            {
                _gameBoard[destX, destY] = _gameBoard[iniX, iniY];
                _gameBoard[Avg(destX, iniX), Avg(destY, iniY)] = EGameSquareState.Empty;
                _gameBoard[iniX, iniY] = EGameSquareState.Empty;
                _takingDone = true;
                if (!CanTake(destX, destY))
                {
                    _nextMoveByBlack = !_nextMoveByBlack;
                    _canDoAnotherTake = false;

                }
                else
                {
                    _canDoAnotherTake = true;
                }
            }
            else
            {
                _gameBoard[destX, destY] = _gameBoard[iniX, iniY];
                _gameBoard[iniX, iniY] = EGameSquareState.Empty;
                _nextMoveByBlack = !_nextMoveByBlack;
                _canDoAnotherTake = false;
                _takingDone = false;
            }

            
            // Change regular checker into king
            if (_gameBoard[destX, destY] == EGameSquareState.Black)
            {
                if (destY == 0)
                {
                    _gameBoard[destX, destY] = EGameSquareState.BlackKing;
                }
            }
            else
            {
                if (destY == _gameBoard.GetLength(0) - 1)
                {
                    _gameBoard[destX, destY] = EGameSquareState.WhiteKing;
                }
            }
        } // TODO taking for kings.
        
        
        
    }

    public bool NextMoveByBlack()
    {
        return _nextMoveByBlack;
    }

    public bool TakingDone()
    {
        return _takingDone;
    }

    private int Avg(int a, int b)
    {
        return (a + b) / 2;
    }

}