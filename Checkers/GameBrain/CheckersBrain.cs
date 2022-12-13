using Domain;

namespace GameBrain;

public class CheckersBrain
{
    private EGameSquareState[,] _gameBoard;

    private readonly CheckerGameOptions _checkerGameOptions;
    private bool _gameOver;
    private bool _nextMoveByBlack;
    private bool _takingDone;
    private bool _canDoAnotherTake;
    private bool _gameWonByBlack;

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
                        }
                        else if (i >= boardHeight / 2 + 1)
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
                        else if (i <= boardHeight / 2 - 2)
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
    /// <summary>
    /// Method to make a copy of the board currently used by the brain. This is a clone, not a reference.
    /// </summary>
    /// <returns>Deep copy of the game board</returns>
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

    /// <summary>
    /// Sets the game board and state in the brain.
    /// </summary>
    /// <param name="board"> 2D array of square state. The board should be provided here </param>
    /// <param name="state"> GameState object. </param>
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



    /// <summary>
    /// Method to analise potential move. Checks all the possible cases of the move and returns true of move is possible
    /// and false if move is not possble
    /// </summary>
    /// <param name="iniX">Initial x of the checker</param>
    /// <param name="iniY">Initial y of the checker</param>
    /// <param name="destX">Destination x of the checker</param>
    /// <param name="destY">Destination y of the checker</param>
    /// <returns>True if move is possible and false if not possible</returns>
    public bool MoveIsPossible(int iniX, int iniY, int destX, int destY)
    {
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
                    if ((_gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.White ||
                         _gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.WhiteKing) &&
                        _gameBoard[destX, destY] == EGameSquareState.Empty)
                    {
                        return true;
                    }
                }
                else if (_gameBoard[iniX, iniY] == EGameSquareState.White)
                {
                    if ((_gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.Black ||
                         _gameBoard[Avg(destX, iniX), Avg(destY, iniY)] == EGameSquareState.BlackKing) &&
                        _gameBoard[destX, destY] == EGameSquareState.Empty)
                    {
                        return true;
                    }
                }
            }
            // If checker can do another take then suggest only those squares that are meant for taking.
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
        }
        else if (_gameBoard[iniX, iniY] == EGameSquareState.WhiteKing ||
                 _gameBoard[iniX, iniY] == EGameSquareState.BlackKing)
        {
            // TODO check if any checkers in the way and run validation
            // |𝑥1−𝑥2|=|𝑦1−𝑦2| - condition of 2 points being diagonally aligned 
            if (Math.Abs(iniX - destX) == Math.Abs(iniY - destY))
            {
                // Got only diagonal moves

                // return true;
                // Here the brain will count the amount of enemy checkers between initial and destination coordinates.
                // If there are friendly checkers --> return false
                // If there is 1 or 0 enemy checkers --> return true
                // If there is more than 1 enemy checkers --> return false
                var counts = CountEnemyAdjCheckers(iniX, iniY, destX, destY, false);

                if (_canDoAnotherTake)
                {
                    return counts.Item1 == 1 && counts.Item2 == 0;
                }
                else
                {
                    return counts.Item1 <= 1 && counts.Item2 == 0;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Tests if the game is over. Changes inner variables.
    /// </summary>
    public void TestIfGameOver()
    {
        var whiteCount = 0;
        var blackCount = 0;
        for (int y = 0; y < _gameBoard.GetLength(1); y++)
        {
            for (int x = 0; x < _gameBoard.GetLength(0); x++)
            {
                if (_gameBoard[x, y] == EGameSquareState.Black || _gameBoard[x, y] == EGameSquareState.BlackKing)
                {
                    blackCount++;
                }

                else if (_gameBoard[x, y] == EGameSquareState.White || _gameBoard[x, y] == EGameSquareState.WhiteKing)
                {
                    whiteCount++;
                }
            }
        }

        if (whiteCount == 0)
        {
            _gameOver = true;
            _gameWonByBlack = true;
        }

        if (blackCount == 0)
        {
            _gameOver = true;
            _gameWonByBlack = false;
        }
    }

    /// <summary>
    /// Method to check if game over
    /// </summary>
    /// <returns>True if game over and false if not</returns>
    public bool IsGameOver()
    {
        return _gameOver;
    }

    /// <summary>
    /// Method to check if game was won by a player with black ckeckers
    /// </summary>
    /// <returns>True if game won by player with black checker and false if not</returns>
    public bool GameWonByBlack()
    {
        return _gameWonByBlack;
    }

    /// <summary>
    /// This is a multipurpose method. In the first case when getCoords parameter is false, this method returns a tuple,
    /// where first int is amount of enemies on the diagonal between initial coordinates and destination coordinates and second is
    /// amount of friendly checkers on the same diagonal. Second case when getCoords if true this method returns coords
    /// of the enemy checker on the diagonal between initial and destination coordinates. 
    /// </summary>
    /// <param name="iniX">Initial x</param>
    /// <param name="iniY">Initial y</param>
    /// <param name="destX">Destination x</param>
    /// <param name="destY">Destination y</param>
    /// <param name="getCoords">Function mode. True if count enemy and friendly checkers on diagonal and false if
    /// get coords of enemy checker on this diagonal.</param>
    /// <returns>Return type depends on the function mode</returns>
    private Tuple<int, int> CountEnemyAdjCheckers(int iniX, int iniY, int destX, int destY, bool getCoords)
    {
        // Return tuple where first number is amount of enemy checkers on this trajectory and second number 
        // amount of friendly checkers on this trajectory
        var biggerX = iniX > destX ? iniX : destX;
        var biggerY = iniY > destY ? iniY : destY;
        var smallerX = iniX < destX ? iniX : destX;
        var smallerY = iniY < destY ? iniY : destY;
        var xGrowing = destX > iniX;
        var yGrowing = destY > iniY;


        var enemyCount = 0;
        var friendlyCount = 0;

        var finish = false;

        do
        {
            var x = xGrowing ? smallerX : biggerX;
            var y = yGrowing ? smallerY : biggerY;

            switch (_gameBoard[iniX, iniY])
            {
                case EGameSquareState.WhiteKing:
                    if (_gameBoard[x, y] == EGameSquareState.Black ||
                        _gameBoard[x, y] == EGameSquareState.BlackKing)
                    {
                        if (getCoords)
                        {
                            return new Tuple<int, int>(x, y);
                        }

                        enemyCount++;
                    }

                    if (_gameBoard[x, y] == EGameSquareState.White ||
                        _gameBoard[x, y] == EGameSquareState.WhiteKing && x != iniX && y != iniY)
                    {
                        friendlyCount++;
                    }

                    break;
                case EGameSquareState.BlackKing:
                    if (_gameBoard[x, y] == EGameSquareState.White ||
                        _gameBoard[x, y] == EGameSquareState.WhiteKing)
                    {
                        if (getCoords)
                        {
                            return new Tuple<int, int>(x, y);
                        }

                        enemyCount++;
                    }

                    if (_gameBoard[x, y] == EGameSquareState.Black ||
                        _gameBoard[x, y] == EGameSquareState.BlackKing && x != iniX && y != iniY)
                    {
                        friendlyCount++;
                    }

                    break;
            }

            if (smallerX == biggerX && smallerY == biggerY)
            {
                finish = true;
            }

            if (xGrowing)
            {
                smallerX++;
            }
            else if (!xGrowing)
            {
                biggerX--;
            }

            if (yGrowing)
            {
                smallerY++;
            }
            else if (!yGrowing)
            {
                biggerY--;
            }
        } while (!finish);

        var checkerCount = new Tuple<int, int>(enemyCount, friendlyCount);
        return checkerCount;
    }

    /// <summary>
    /// Check if checker has any moves
    /// </summary>
    /// <param name="x">X of the checker</param>
    /// <param name="y">Y of the checker</param>
    /// <returns>True if checker has any moves false if not</returns>
    public bool HasMoves(int x, int y)
    {
        if (_gameBoard[x, y] != EGameSquareState.Unplayable && _gameBoard[x, y] != EGameSquareState.Empty)
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

    
    /// <summary>
    /// Check if checker has any taking opportunities
    /// </summary>
    /// <param name="x">X of the checker</param>
    /// <param name="y">Y of the checker</param>
    /// <returns>True if checker has any taking opportunities and false if not</returns>
    public bool CanTake(int x, int y)
    {
        if (_gameBoard[x, y] == EGameSquareState.Black || _gameBoard[x, y] == EGameSquareState.White)
        {
            try
            {
                if (MoveIsPossible(x, y, x - 2, y - 2))
                {
                    return true;
                }


                try
                {
                    if (MoveIsPossible(x, y, x + 2, y - 2))
                    {
                        return true;
                    }

                    try
                    {
                        if (MoveIsPossible(x, y, x + 2, y + 2))
                        {
                            return true;
                        }

                        try
                        {
                            if (MoveIsPossible(x, y, x - 2, y + 2))
                            {
                                return true;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return false;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }


                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        if (_gameBoard[x, y] == EGameSquareState.BlackKing || _gameBoard[x, y] == EGameSquareState.WhiteKing)
        {
            for (int yi = 0; yi < _gameBoard.GetLength(1); yi++)
            {
                for (int xi = 0; xi < _gameBoard.GetLength(0); xi++)
                {
                    // Check if diagonal and difference if bigger than 1
                    if (Math.Abs(x - xi) == Math.Abs(y - yi) &&
                        _gameBoard[xi, yi] == EGameSquareState.Empty &&
                        Math.Abs(x - xi) > 1 &&
                        CountEnemyAdjCheckers(x, y, xi, yi, false).Item1 == 1 &&
                        CountEnemyAdjCheckers(x, y, xi, yi, false).Item2 == 0)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Function to move checker. Taking is preformed here as well.
    /// </summary>
    /// <param name="iniX">Initial x of the checker</param>
    /// <param name="iniY">Initial y of the checker</param>
    /// <param name="destX">Destination x of the checker</param>
    /// <param name="destY">Destination y of the checker</param>
    /// <exception cref="ArgumentException">Thrown if move suggested by input is not valid.</exception>
    public void MoveChecker(int iniX, int iniY, int destX, int destY)
    {
        if (!MoveIsPossible(iniX, iniY, destX, destY))
        {
            throw new ArgumentException("Tried to move checker but suggested move is not valid" +
                                        $"iniX{iniX}, iniY{iniY}, destX{destX}, destY{destY}");
        }

        if ((_gameBoard[iniX, iniY] == EGameSquareState.Black ||
             _gameBoard[iniX, iniY] == EGameSquareState.BlackKing) && !_nextMoveByBlack ||
            (_gameBoard[iniX, iniY] == EGameSquareState.White ||
             _gameBoard[iniX, iniY] == EGameSquareState.WhiteKing) && _nextMoveByBlack)
        {
            throw new ArgumentException(
                "Tried to move a checker that is not to be moved on this turn. This checker should not be clickable. Next move by black" +
                _nextMoveByBlack);
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
                    if (CanTake(destX, destY) && !_nextMoveByBlack && _takingDone)
                    {
                        _nextMoveByBlack = true;
                        _canDoAnotherTake = true;
                    }

                    return;
                }
            }
            else
            {
                if (destY == _gameBoard.GetLength(0) - 1)
                {
                    _gameBoard[destX, destY] = EGameSquareState.WhiteKing;
                    if (CanTake(destX, destY) && _nextMoveByBlack && _takingDone)
                    {
                        _nextMoveByBlack = false;
                        _canDoAnotherTake = true;
                    }

                    return;
                }
            }
        }

        if (_gameBoard[iniX, iniY] == EGameSquareState.BlackKing ||
            _gameBoard[iniX, iniY] == EGameSquareState.WhiteKing)
        {
            // Check if there are 1 checker on the diagonal between the coords and no allie checkers.
            if (CountEnemyAdjCheckers(iniX, iniY, destX, destY, false).Item1 == 1 &&
                CountEnemyAdjCheckers(iniX, iniY, destX, destY, false).Item2 == 0)
            {
                // Taking is preformed
                _gameBoard[destX, destY] = _gameBoard[iniX, iniY];
                var cords = CountEnemyAdjCheckers(iniX, iniY, destX, destY, true);
                _gameBoard[cords.Item1, cords.Item2] = EGameSquareState.Empty;
                _gameBoard[iniX, iniY] = EGameSquareState.Empty;
                _takingDone = true;

                // Check if another taking can be made
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
        }

        TestIfGameOver();
    }


    
    /// <summary>
    /// Method to check if next move is by black
    /// </summary>
    /// <returns>True if next move by black and false if by white</returns>
    public bool NextMoveByBlack()
    {
        return _nextMoveByBlack;
    }

    /// <summary>
    /// Method to check if there was any taking performed in the previous go
    /// </summary>
    /// <returns>True if there was any taking in the previous go and false if not</returns>
    public bool TakingDone()
    {
        return _takingDone;
    }

    private static int Avg(int a, int b)
    {
        return (a + b) / 2;
    }

    
    /// <summary>
    /// Method to toggle next move of the player
    /// </summary>
    public void ToggleNextMove()
    {
        _nextMoveByBlack = !_nextMoveByBlack;
    }

    
    /// <summary>
    /// Method count the checkers on the board. If parameter if true counts black ones if false white ones.
    /// </summary>
    /// <param name="black">True if count black checkers and false if count white checkers</param>
    /// <returns>Amount of asked checkers</returns>
    public int CountCheckers(bool black)
    {
        int checkers = 0;
        for (int yi = 0; yi < _gameBoard.GetLength(1); yi++)
        {
            for (int xi = 0; xi < _gameBoard.GetLength(0); xi++)
            {
                if (black && (_gameBoard[xi, yi] == EGameSquareState.Black ||
                              _gameBoard[xi, yi] == EGameSquareState.BlackKing))
                {
                    checkers++;
                }
                else if (!black && (_gameBoard[xi, yi] == EGameSquareState.White ||
                                    _gameBoard[xi, yi] == EGameSquareState.WhiteKing))
                {
                    checkers++;
                }
            }
        }

        return checkers;
    }




    public void MakeMoveByAi()
    {
        throw new NotImplementedException();
    }


    public List<(int x, int y)> GetAllMovableCheckersFor(bool isBlack)
    {
        List<(int x, int y)> ret = new();
        for (int yi = 0; yi < _gameBoard.GetLength(1); yi++)
        {
            for (int xi = 0; xi < _gameBoard.GetLength(0); xi++)
            {
                if (isBlack &&
                    (_gameBoard[xi, yi] == EGameSquareState.Black ||
                     _gameBoard[xi, yi] == EGameSquareState.BlackKing) &&
                    HasMoves(xi, yi))
                {
                    ret.Add((xi, yi));
                }

            }
        }

        return ret;
    }

    private List<(int x, int y)> GetMovesFor(int x, int y)
    {
        List<(int x, int y)> ret = new List<(int x, int y)>();
        for (int yi = 0; yi < _gameBoard.GetLength(1); yi++)
        {
            for (int xi = 0; xi < _gameBoard.GetLength(0); xi++)
            {
                if (MoveIsPossible(x, y, xi, yi))
                {
                    ret.Add((xi, yi));
                }
            }
        }

        return ret;
    }


}