using System.Text.Json;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.CheckerGames;

public class Play : PageModel
{
    private readonly IGameGameRepository _repo;


    public Play(IGameGameRepository repo)
    {
        _repo = repo;
    }


    public CheckersBrain Brain { get; set; } = default!;
    public EGameSquareState[,] Board { get; set; } = default!;
    public CheckerGameState GameState { get; set; } = default!;
    public CheckerGame Game { get; set; } = default!;

    public int? InitX { get; set; }
    public int? InitY { get; set; }
    public bool FirstReq { get; set; }

    public bool? WonByBlack { get; set; }

    public IActionResult OnGet(int? id, int? firstReq, int? x, int? y, int? initX, int? initY)
    {
        // Check if it is the first request(initial player choice)
        if (firstReq != null)
        {
            FirstReq = true;
        }

        if (id == null)
        {
            return NotFound();
        }

        Game = _repo.GetGameById((int)id);

        var options = Game.GameOptions;

        if (Game.CheckerGameStates == null || options == null)
        {
            return NotFound();
        }


        // Get brain and state
        Brain = new CheckersBrain(options);
        GameState = Game.CheckerGameStates.Last();


        if (firstReq != null && x != null && y != null)
        {
            InitX = x;
            InitY = y;
        }


        // Unserialize json game board. Get game board.
        var jsonString = GameState.SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        Board = FsHelpers.JaggedTo2D(jagged!);
        Brain.SetGameBoard(Board, GameState);
        if (Game.GameOverAt != null)
        {
            WonByBlack = Game.GameWonBy == Game.Player2Name;
            return Page();
            
        }
        
        

        // Check if player wants to switch the checker that he wants to use
        if (x != null && y != null && initX != null && initY != null &&
            x == initX && y == initY)
        {
            Response.Redirect($"./Play?id={Game.Id}");
            return Page();
        }

        CheckIfMakeAnotherTake();

        // If params needed for the move are not null - make move
        if (firstReq != null || x == null || y == null || initX == null || initY == null) return Page();


        Brain.MoveChecker((int)initX, (int)initY, (int)x, (int)y);
        var newState = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(
                FsHelpers.ToJaggedArray(Brain.GetBoard())),
            NextMoveByBlack = Brain.NextMoveByBlack()
        };
        // Check if taking was preformed
        // && Brain.CanTake((int)x, (int)y)
        if (Brain.TakingDone() && Brain.CanTake((int)x, (int)y))
        {
            // If taking was preformed mark which checker was doing it.
            newState.CheckerThatPreformedTakingX = x;
            newState.CheckerThatPreformedTakingY = y;
        }
        else
        {
            FirstReq = false;
        }

        Game.CheckerGameStates.Add(newState);
        GameState = newState;
        CheckIfMakeAnotherTake();
        if (Brain.IsGameOver())
        {
            Game.GameWonBy = Brain.GameWonByBlack() ? Game.Player2Name : Game.Player1Name;
            Game.GameOverAt = DateTime.Now;
        }

        _repo.UpdateGame(Game);
        if (Game.GameOverAt != null)
        {
            WonByBlack = Game.GameWonBy == Game.Player2Name;
        }

        return Page();
    }


    // Check if there is a checker that has preformed a take in previous move. If this checker can preform another
    private void CheckIfMakeAnotherTake()
    {
        if (GameState.CheckerThatPreformedTakingX != null && GameState.CheckerThatPreformedTakingY != null)
        {
            if (Brain.CanTake((int)GameState.CheckerThatPreformedTakingX,
                    (int)GameState.CheckerThatPreformedTakingY))
            {
                FirstReq = true;
                InitX = GameState.CheckerThatPreformedTakingX;
                InitY = GameState.CheckerThatPreformedTakingY;
            }
        }
    }
}