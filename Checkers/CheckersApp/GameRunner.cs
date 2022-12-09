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


    public GameRunner(IGameRepository repo, int id)
    {
        _repo = repo;
        _game = repo.GetGameById(id);
    }

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
            var state = _game.CheckerGameStates.Last();
            _brain = new CheckersBrain(options);
            state = GetPreparedBoard(out var board);
            _brain.SetGameBoard(board, state);
            Console.Clear();

            if (_game.GameWonBy == null)
            {
                MakeAMoveBy(state.NextMoveByBlack);    
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

    private CheckerGameState GetPreparedBoard(out EGameSquareState[,] board)
    {
        CheckerGameState state;
        state = _game.CheckerGameStates!.Last();
        var jsonString = state.SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        board = FsHelpers.JaggedTo2D(jagged!);
        return state;
    }

    private void MakeAMoveBy(bool nextMoveByBlack)
    {
        

        var choiceIsValid = false;

        int x = -1;
        int y = -1;
        do
        {
            Console.Clear();
            PrintNextPlayer(nextMoveByBlack);
            
            UI.DrawGameBoard(_brain.GetBoard(), null, null);
            Console.WriteLine("Choose a checker to move");
            Console.WriteLine("Enter checker row: ");
            
            // TODO validation
            var userXChoice = Console.ReadLine();
            if (!int.TryParse(userXChoice, out var xi))
            {
                continue;
            }

            xi -= 1;
             
            Console.WriteLine("Enter checker column: ");
            
            // TODO validation
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
                if (!_brain.HasMoves(xi, yi))
                {
                    Console.WriteLine(
                        "Checker that you picked does not have any valid moves. You have to chose another one");
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

        
        var destIsValid = false;
        do
        {
            Console.Clear();
            UI.DrawGameBoard(_brain.GetBoard(), x, y);
            Console.WriteLine("Now choose where to move this checker row");
            
            
            // TODO validation
            var userXChoice = Console.ReadLine();
            if (!int.TryParse(userXChoice, out var destX))
            {
                continue;
            }
            destX -= 1;
            
            Console.WriteLine("Now choose where to move this checker column");
            
            
            // TODO validation
            var userYChoice = Console.ReadLine();
            if (!int.TryParse(userYChoice, out var destY))
            {
                continue;
            }
            destY -= 1;
            
            if (_brain.MoveIsPossible(x, y, destX, destY))
            {
                destIsValid = true;
                _brain.MoveChecker(x, y, destX, destY);
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
            UI.DrawGameBoard(_brain.GetBoard(), null, null);
            var newState = CreateNewState(destX, destX, false);
            _game.CheckerGameStates!.Add(newState);
            _repo.UpdateGame(_game);

        } while (!destIsValid);
    }

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