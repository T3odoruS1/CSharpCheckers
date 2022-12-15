using System.Runtime.InteropServices;
using Domain;

namespace GameBrain;

public class CheckersBrain
{
    private EGameSquareState[,] _gameBoard;

    private CheckerGameOptions _checkerGameOptions;
    private bool _gameOver;
    private bool _nextMoveByBlack;
    private bool _takingDone;
    private bool _canDoAnotherTake;
    private bool _gameWonByBlack;
    private (int x, int y)? _coordsOfTakingChecker;

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
        for (var i = 0; i < _checkerGameOptions.Width; i++)
        {
            for (var j = 0; j < _checkerGameOptions.Height; j++)
            {
                res[i, j] = _gameBoard[i, j];
            }
        }

        return res;
    }


    /// <summary>
    /// Get a deepcopy of this game brain
    /// </summary>
    /// <returns></returns>
    public CheckersBrain CloneBrain()
    {
        var boardCopy = GetBoard();
        var gameOver = _gameOver;
        var nextMoveByBlack = _nextMoveByBlack;
        var takingDone = _takingDone;
        var canDoAnotherTake = _canDoAnotherTake;
        var gameWonByBlack = _gameWonByBlack;
        var coordsOfTakingChecker = _coordsOfTakingChecker;

        var newBrain = new CheckersBrain(_checkerGameOptions);
        newBrain.SetNewParameters(
            boardCopy,
            _checkerGameOptions,
            gameOver,
            nextMoveByBlack,
            takingDone,
            canDoAnotherTake,
            gameWonByBlack,
            coordsOfTakingChecker);

        return newBrain;
    }


    
    /// <summary>
    /// Set all parameters for brain copy.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="options"></param>
    /// <param name="gameOver"></param>
    /// <param name="nextMoveByBlack"></param>
    /// <param name="takingDone"></param>
    /// <param name="canDoAnotherTake"></param>
    /// <param name="gameWonByBlack"></param>
    /// <param name="coordsOfTakingChecker"></param>
    private void SetNewParameters(
        EGameSquareState[,] board,
        CheckerGameOptions options,
        bool gameOver,
        bool nextMoveByBlack,
        bool takingDone,
        bool canDoAnotherTake,
        bool gameWonByBlack,
        (int x, int y)? coordsOfTakingChecker)
    {
        _gameBoard = board;
        _checkerGameOptions = options;
        _gameOver = gameOver;
        _nextMoveByBlack = nextMoveByBlack;
        _takingDone = takingDone;
        _canDoAnotherTake = canDoAnotherTake;
        _gameWonByBlack = gameWonByBlack;
        _coordsOfTakingChecker = coordsOfTakingChecker;
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
        if (state.CheckerThatPreformedTakingX != null && state.CheckerThatPreformedTakingY != null)
        {
            _coordsOfTakingChecker = (state.CheckerThatPreformedTakingX.Value,
                state.CheckerThatPreformedTakingY.Value);
        }

    }


    /// <summary>
    /// Method to analise potential move. Checks all the possible cases of the move and returns true of move is possible
    /// and false if move is not possble
    /// </summary>
    /// <param name="iniX">Initial x of the checker</param>
    /// <param name="iniY">Initial y of the checker</param>
    /// <param name="destX">Destination x of the checker</param>
    /// <param name="destY">Destination y of the checker</param>
    /// <param name="board">Optional parameter. If you need to evaluate external board input it here.</param>
    /// <returns>True if move is possible and false if not possible</returns>
    public bool MoveIsPossible(int iniX, int iniY, int destX, int destY, EGameSquareState[,]? board = null)
    {
        
        // Check if coordinates are in range of game board.
        if (iniX > _checkerGameOptions.Width || iniX < 0 ||
            destX > _checkerGameOptions.Width || destX < 0 ||
            iniY > _checkerGameOptions.Height || iniY < 0 ||
            destY > _checkerGameOptions.Height || destY < 0)
        {
            return false;
        }

        if (_canDoAnotherTake)
        {
            if (_coordsOfTakingChecker != null)
            {
                if ((!CanTake(_coordsOfTakingChecker.Value.x, _coordsOfTakingChecker.Value.y) ||
                     _coordsOfTakingChecker.Value.x != iniX || _coordsOfTakingChecker.Value.y != iniY) &&
                    !MoveIsTaking(iniX, iniY, destX, destY))
                {
                    return false;

                }
            }
           
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
        for (int y = 0; y < _checkerGameOptions.Height; y++)
        {
            for (int x = 0; x < _checkerGameOptions.Width; x++)
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

    private bool MoveIsTaking(int x, int y, int destX, int destY)
    {
        if (x > _checkerGameOptions.Width || x < 0 ||
            destX > _checkerGameOptions.Width || destX < 0 ||
            y > _checkerGameOptions.Height || y < 0 ||
            destY > _checkerGameOptions.Height || destY < 0)
        {
            return false;
        }
        var data = CountEnemyAdjCheckers(x, y, destX, destY, false);
        return data.Item1 == 1 && data.Item2 == 0 && _gameBoard[destX, destY] == EGameSquareState.Empty;
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
    public Tuple<int, int> CountEnemyAdjCheckers(int iniX, int iniY, int destX, int destY, bool getCoords)
    {
        // Return tuple where first number is amount of enemy checkers on this trajectory and second number 
        // amount of friendly checkers on this trajectory
        if (Math.Abs(iniX - destX) != Math.Abs(iniY - destY))
        {
            return new Tuple<int, int>(0, 0);
        }
        
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
                    
                case EGameSquareState.WhiteKing: case EGameSquareState.White:
                    if (_gameBoard[x, y] == EGameSquareState.Black ||
                        _gameBoard[x, y] == EGameSquareState.BlackKing)
                    {
                        if (getCoords)
                        {
                            return new Tuple<int, int>(x, y);
                        }

                        enemyCount++;
                    }

                    if ((_gameBoard[x, y] == EGameSquareState.White ||
                         _gameBoard[x, y] == EGameSquareState.WhiteKing) && x != iniX && y != iniY)
                    {
                        friendlyCount++;
                    }

                    break;
                case EGameSquareState.BlackKing: case EGameSquareState.Black:
                    if (_gameBoard[x, y] == EGameSquareState.White ||
                        _gameBoard[x, y] == EGameSquareState.WhiteKing)
                    {
                        if (getCoords)
                        {
                            return new Tuple<int, int>(x, y);
                        }

                        enemyCount++;
                    }

                    if ((_gameBoard[x, y] == EGameSquareState.Black ||
                         _gameBoard[x, y] == EGameSquareState.BlackKing) && x != iniX && y != iniY)
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

    private (int x, int y)? GetCoordsOfTakingChecker()
    {
        return _coordsOfTakingChecker;
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
                if (MoveIsTaking(x, y, x - 2, y - 2))
                {
                    return true;
                }


                try
                {
                    if (MoveIsTaking(x, y, x + 2, y - 2))
                    {
                        return true;
                    }

                    try
                    {
                        if (MoveIsTaking(x, y, x + 2, y + 2))
                        {
                            return true;
                        }

                        try
                        {
                            if (MoveIsTaking(x, y, x - 2, y + 2))
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
                        return false;
                    }
                }


                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        if (_gameBoard[x, y] == EGameSquareState.BlackKing || _gameBoard[x, y] == EGameSquareState.WhiteKing)
        {
            for (int yi = 0; yi < _checkerGameOptions.Height; yi++)
            {
                for (int xi = 0; xi < _checkerGameOptions.Width; xi++)
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
                    _coordsOfTakingChecker = null;
                }
                else
                {
                    _canDoAnotherTake = true;
                    _coordsOfTakingChecker = (destX, destY);
                }
            }
            else
            {
                _gameBoard[destX, destY] = _gameBoard[iniX, iniY];
                _gameBoard[iniX, iniY] = EGameSquareState.Empty;
                _nextMoveByBlack = !_nextMoveByBlack;
                _canDoAnotherTake = false;
                _coordsOfTakingChecker = null;
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
                        _coordsOfTakingChecker = (destX, destY);

                    }

                    return;
                }
            }
            else
            {
                if (destY == _gameBoard.GetLength(1) - 1)
                {
                    _gameBoard[destX, destY] = EGameSquareState.WhiteKing;
                    if (CanTake(destX, destY) && _nextMoveByBlack && _takingDone)
                    {
                        _nextMoveByBlack = false;
                        _canDoAnotherTake = true;
                        _coordsOfTakingChecker = (destX, destY);

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
        for (int yi = 0; yi < _checkerGameOptions.Height; yi++)
        {
            for (int xi = 0; xi < _checkerGameOptions.Width; xi++)
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


    private (int x, int y, int destX, int destY) GenerateARandomMove(bool isBlack)
    {
        if (!_nextMoveByBlack && isBlack)
        {
            throw new ArgumentException("Ai tried to make a move for black player but it's not it's turn");
        }

        var movableCheckers = GetAllMovableCheckersFor(isBlack);
        var allMoves = new List<(int x, int y, int destX, int destY)>();
        foreach (var coord in movableCheckers)
        {
            var move = GetMovesFor(coord.x, coord.y);
            foreach (var possibleMove in move)
            {
                allMoves.Add((possibleMove.x, possibleMove.y, possibleMove.destX, possibleMove.destY));
            }
        }

        var rnd = new Random();

        var i = rnd.Next(allMoves.Count);
        return allMoves[i];
    }
    
    /// <summary>
    /// Get all possible moves for player
    /// </summary>
    /// <param name="isBlack">Ture if player is black</param>
    /// <returns>List of tuples with every possible move coords.</returns>
    private List<(int x, int y)> GetAllMovableCheckersFor(bool isBlack)
    {
        List<(int x, int y)> ret = new();
        for (var yi = 0; yi < _checkerGameOptions.Height; yi++)
        {
            for (var xi = 0; xi < _checkerGameOptions.Width; xi++)
            {
                if (isBlack &&
                    (_gameBoard[xi, yi] == EGameSquareState.Black ||
                     _gameBoard[xi, yi] == EGameSquareState.BlackKing) &&
                    HasMoves(xi, yi))
                {
                    ret.Add((xi, yi));
                }
                else if (!isBlack &&
                         (_gameBoard[xi, yi] == EGameSquareState.White ||
                          _gameBoard[xi, yi] == EGameSquareState.WhiteKing) &&
                         HasMoves(xi, yi))
                {
                    ret.Add((xi, yi));
                }

            }
        }

        return ret;
    }

    private List<(int x, int y, int destX, int destY)> GetMovesFor(int x, int y)
    {
        var ret = new List<(int x, int y, int destX, int destY)>();
        for (int yi = 0; yi < _checkerGameOptions.Height; yi++)
        {
            for (int xi = 0; xi < _checkerGameOptions.Width; xi++)
            {
                if (MoveIsPossible(x, y, xi, yi))
                {
                    ret.Add((x, y, xi, yi));
                }
            }
        }

        return ret;
    }

    


    public (int x, int y) MakeMoveByAi(bool isBlack)
    {
        // Randomly chosen move
        var rndMove = PreformMiniMaxDecision(isBlack);


        MoveChecker(rndMove.x, rndMove.y, rndMove.destX, rndMove.destY);

        // If taking was preformed ---> preform another take.
        Console.WriteLine("Ai prefomed a move");
        return (rndMove.destX, rndMove.destY);
    }



    private Move PreformMiniMaxDecision(bool isBlack)
    {
        var brainClone = CloneBrain();

        var depth = 4;

        var desicion = MM.Minimax(depth, brainClone, !isBlack);
        Console.WriteLine(desicion.move);
        Console.WriteLine(desicion.evaluation);


        // What I need?

        // Termianl state                    - IsGameOver()           - done
        // Utility                           - EvaluateBoard()        - to be implemented.
        // Extract move from suggested board - ExtractMoveFromBoard() - to be implemented.
        // Get next board state              - GetNextBoardState()    - to be implemented.
        // Get next player                   - GetNextPlayer()        - done ?
        return desicion.move;
    }


    public List<Move> GetAllMoves(bool isBlack)
    {
        var moves = new List<Move>();
        var checkers = GetAllMovableCheckersFor(isBlack);

        foreach (var coords in checkers)
        {
            var posibilities = GetMovesFor(coords.x, coords.y);
            foreach (var posibility in posibilities)
            {
                var move = new Move()
                {
                    x = posibility.x,
                    y = posibility.y,
                    destX = posibility.destX,
                    destY = posibility.destY
                };
                if (MoveIsPossible(move.x, move.y, move.destX, move.destY))
                {
                    moves.Add(move);

                }
            }
        }

        return moves;
    }


    /// <summary>
    /// Get next player color using enums with black and white
    /// </summary>
    /// <returns></returns>
    private EPlayerSide GetNextPlayer()
    {
        return _nextMoveByBlack ? EPlayerSide.Black : EPlayerSide.White;
    }

    /// <summary>
    /// Evaluate board.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public double EvaluateBoard(EGameSquareState[,] board)
    {
        var white = CountCheckersWithKings(false, board);
        var black = CountCheckersWithKings(true, board);
        return white.r - black.r + white.k * 2 - black.k * 2;
    }


    /// <summary>
    /// Count the checkers for a specific side
    /// </summary>
    /// <param name="isBlack">True if black checkers</param>
    /// <param name="board">Game board</param>
    /// <returns>Tuple, where r is for regular checkers and k is for kings</returns>
    private (int r, int k) CountCheckersWithKings(bool isBlack, EGameSquareState[,] board)
    {
        var regular = 0;
        var kings = 0;
        
        for (int yi = 0; yi < _checkerGameOptions.Height; yi++)
        {
            for (int xi = 0; xi < _checkerGameOptions.Width; xi++)
            {
                if (isBlack)
                {
                    switch (board[xi, yi])
                    {
                        case EGameSquareState.Black:
                            regular++;
                            break;
                        case EGameSquareState.BlackKing:
                            kings++;
                            break;
                    }
                }
                else
                {
                    switch (board[xi, yi])
                    {
                        case EGameSquareState.White:
                            regular++;
                            break;
                        case EGameSquareState.WhiteKing:
                            kings++;
                            break;
                    }
                }
            }
        }

        return (regular, kings);
    }
}