using System.Text.Json;
using ConsoleUI;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;

namespace CheckersApp;

public class GameRunner
{
    private IGameRepository _repo;
    private readonly CheckerGame _game;
    private  CheckersBrain _brain = default!;


    /// <summary>
    /// Constructor method for game runner class
    /// </summary>
    /// <param name="repo">Game repository made using IGameRepository interface</param>
    /// <param name="id">Game id</param>
    public GameRunner(IGameRepository repo, int id)
    {
        _repo = repo;
        
        _game = _repo.GetGameById(id);
    }

    /// <summary>
    /// Method to run the game in the console app
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if game does not have game states or options. That means that the
    /// game was not saved properly.</exception>
    public void RunGame()
    {
        var options = _game.GameOptions;
        if (_game.CheckerGameStates == null || options == null)
        {
            throw new ArgumentException("The game that was recieved with given id does not have options or states.");
        }

        


        var exit = false;
        do
        {
            _brain = new CheckersBrain(options);
            var state = GetPreparedBoard(out var board);
            _brain.SetGameBoard(board, state);
            Console.Clear();

            if (_game.GameWonBy == null)
            {
                if (state.NextMoveByBlack && _game.Player2Type == EPlayerType.Ai)
                {
                    _brain.MakeMoveByAi(state.NextMoveByBlack);
                    var coords = _brain.MakeMoveByAi(state.NextMoveByBlack);
                    var nState = CreateNewState(coords.x, coords.y, false);
                    _game.CheckerGameStates.Add(nState);
                    _repo.UpdateGame(_game);
                }
                else if (!state.NextMoveByBlack && _game.Player1Type == EPlayerType.Ai)
                {
                    var coords = _brain.MakeMoveByAi(state.NextMoveByBlack);
                    var nState = CreateNewState(coords.x, coords.y, false);
                    _game.CheckerGameStates.Add(nState);
                    _repo.UpdateGame(_game);
                }
                else
                {
                    MakeAMoveBy(state.NextMoveByBlack);
                }
            }
            else
            {
                Console.WriteLine($"Game won by {_game.GameWonBy}");
            }
            


            Console.WriteLine("Press X to exit");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.X)
            {
                exit = true;
            }
        } while (!exit);
    }

    /// <summary>
    /// Prepare the board for the game.
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    private CheckerGameState GetPreparedBoard(out EGameSquareState[,] board)
    {
        CheckerGameState state;
        state = _game.CheckerGameStates!.Last();
        var jsonString = state.SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        board = FsHelpers.JaggedTo2D(jagged!);
        return state;
    }

    /// <summary>
    /// Make player preform a move
    /// </summary>
    /// <param name="nextMoveByBlack">True if next move is by black</param>
    private void MakeAMoveBy(bool nextMoveByBlack)
    {
        int x = -1;
        int y = -1;
        
        
        // Ask user which checker to move and preform required validation
        x = PreformInitialCheckerChoice(nextMoveByBlack, x, ref y);


        var destIsValid = false;
        do
        {
            Console.Clear();
            UI.DrawGameBoard(_brain.GetBoard(), x, y);
            Console.WriteLine("Now choose where to move this checker row");

            var userXChoice = Console.ReadLine();
            if (!int.TryParse(userXChoice, out var destX))
            {
                continue;
            }
            destX -= 1;
            
            Console.WriteLine("Now choose where to move this checker column");
            
            
            var userYChoice = Console.ReadLine();
            if (!int.TryParse(userYChoice, out var destY))
            {
                continue;
            }
            destY -= 1;
            
            if (_brain.MoveIsPossible(x, y, destX, destY))
            {
                // Move
                _brain.MoveChecker(x, y, destX, destY);

                // Create new game state
                var newState = CreateNewState(destX, destY, false);

                // If possible to make another take then allow
                if (newState.CheckerThatPreformedTakingX != null)
                {
                    if (!_game.GameOptions!.TakingIsMandatory)
                    {
                        Console.Clear();
                        Console.WriteLine("In with game options in this game the taking is not mandatory. You can press x" +
                                          "to stop taking or any other button to continue.");
                        UI.DrawGameBoard(_brain.GetBoard(), null, null);

                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.X)
                        {
                            destIsValid = true;
                            newState.NextMoveByBlack = !nextMoveByBlack;
                        }
                        else
                        {
                            x = destX;
                            y = destY;
                        }
                        

                    }else
                    {
                        x = destX;
                        y = destY;
                    }
                }
                else
                {
                    destIsValid = true;
                }
                Console.Clear();
                UI.DrawGameBoard(_brain.GetBoard(), null, null);
                _game.CheckerGameStates!.Add(newState);
                _repo.UpdateGame(_game);
            }
            else
            {
                continue;
            }
            if (_brain.IsGameOver())
            {
                _game.GameWonBy = _brain.GameWonByBlack() ? _game.Player2Name : _game.Player1Name;
                _game.GameOverAt = DateTime.Now;
            }

            if (_game.GameWonBy != null)
            {
                Console.WriteLine($"Game won by: {_game.GameWonBy}");
            }
            

        } while (!destIsValid);
    }

    /// <summary>
    /// Helper method to validate user choice of the checker he wants to move
    /// </summary>
    /// <param name="nextMoveByBlack">True if next move by black</param>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <returns>x coordinate and references y coordinate</returns>
    private int PreformInitialCheckerChoice(bool nextMoveByBlack, int x, ref int y)
    {
        var choiceIsValid = false;

        do
        {
            Console.Clear();
            PrintNextPlayer(nextMoveByBlack);

            UI.DrawGameBoard(_brain.GetBoard(), null, null);
            Console.WriteLine("Choose a checker to move");
            Console.WriteLine("Enter checker row: ");

            var userXChoice = Console.ReadLine();
            if (!int.TryParse(userXChoice, out var xi))
            {
                continue;
            }

            xi -= 1;

            Console.WriteLine("Enter checker column: ");

            var userYChoice = Console.ReadLine();
            if (!int.TryParse(userYChoice, out var yi))
            {
                continue;
            }

            yi -= 1;
            if (xi > _brain.GetBoard().GetLength(1) || yi > _brain.GetBoard().GetLength(0))
            {
                continue;
            }

            try
            {
                if (!_brain.HasMoves(xi, yi) || 
                    !nextMoveByBlack &&
                     (_brain.GetBoard()[xi, yi] == EGameSquareState.Black ||
                      _brain.GetBoard()[xi, yi] == EGameSquareState.BlackKing) || 
                     nextMoveByBlack && 
                     (_brain.GetBoard()[xi, yi] == EGameSquareState.White ||
                       _brain.GetBoard()[xi, yi] == EGameSquareState.WhiteKing))
                {
                    continue;
                }
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }


            x = xi;
            y = yi;
            choiceIsValid = true;
        } while (!choiceIsValid);

        return x;
    }

    /// <summary>
    /// Prints out who should make the next move
    /// </summary>
    /// <param name="nextMoveByBlack">True if next move by black</param>
    private void PrintNextPlayer(bool nextMoveByBlack)
    {
        switch (nextMoveByBlack)
        {
            case true:
                Console.WriteLine($"Player: {_game.Player2Name} : Black, your move!");
                break;
            case false:
                Console.WriteLine($"Player: {_game.Player1Name} : White, your move!");
                break;
        }
    }


    
    /// <summary>
    /// Create new game state using current x,y coordinates.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="pass">true if pass the move</param>
    /// <returns>new game state</returns>
    private CheckerGameState CreateNewState(int x, int y, bool pass)
    {
        var newState = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(
                FsHelpers.ToJaggedArray(_brain.GetBoard())),
            NextMoveByBlack = !pass ? _brain.NextMoveByBlack() : !_brain.NextMoveByBlack()
        };
        // Check if taking was preformed

        if (_brain.TakingDone() && _brain.CanTake(x, y))
        {
            // If taking was preformed mark which checker was doing it.
            newState.CheckerThatPreformedTakingX = x;
            newState.CheckerThatPreformedTakingY = y;
        }

        return newState;
    }


   
}