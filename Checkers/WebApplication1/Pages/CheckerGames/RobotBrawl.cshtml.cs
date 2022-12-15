using System.Text.Json;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.CheckerGames;

public class RobotBrawl : PageModel
{
    public bool? WonByBlack { get; set; }

    public readonly IGameRepository Repo;


    public RobotBrawl(IGameRepository repo)
    {
        Repo = repo;
    }


    public CheckersBrain Brain { get; set; } = default!;
    public EGameSquareState[,] Board { get; set; } = default!;
    public CheckerGameState GameState { get; set; } = default!;
    public CheckerGame Game { get; set; } = default!;

    public bool gameOver = false;
    
    
    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return RedirectToPage("/Index", new { error = "No game id was provided" });
        }

        Game = Repo.GetGameById(id.Value);

        var options = Game.GameOptions;

        if (Game.CheckerGameStates == null || options == null)
        {
            return NotFound();
        }
        
        Brain = new CheckersBrain(options);
        GameState = Game.CheckerGameStates.Last();
        var jsonString = GameState.SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        Board = FsHelpers.JaggedTo2D(jagged!);
        Brain.SetGameBoard(Board, GameState);


        if (!Brain.IsGameOver())
        {
            var move = Brain.MakeMoveByAi(Brain.NextMoveByBlack());
            UpdateRepo(move.x, move.y);
            return Page();
        }

        gameOver = true;
        return Page();




    }
    
    private void UpdateRepo(int x, int y)
    {
        var newState = CreateNewState(x, y, false);
        Game.CheckerGameStates!.Add(newState);
        GameState = newState;
        if (Brain.IsGameOver())
        {
            Game.GameWonBy = Brain.GameWonByBlack() ? Game.Player2Name : Game.Player1Name;
            Game.GameOverAt = DateTime.Now;
        }

        Repo.UpdateGame(Game);
        if (Game.GameOverAt != null)
        {
            WonByBlack = Game.GameWonBy == Game.Player2Name;
        }
    }

    /// <summary>
    /// Create new state after performing a move
    /// </summary>
    /// <param name="x">x of the checker that preformed a move</param>
    /// <param name="y">y of the checker that preformed a move</param>
    /// <param name="pass">True of pass the next move in case of taking</param>
    /// <returns>new checker game state.</returns>
    private CheckerGameState CreateNewState(int x, int y, bool pass)
    {
        var newState = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(
                FsHelpers.ToJaggedArray(Brain.GetBoard())),
            NextMoveByBlack = !pass ? Brain.NextMoveByBlack() : !Brain.NextMoveByBlack()
        };
        // Check if taking was preformed

        if (Brain.TakingDone() && Brain.CanTake(x, y))
        {
            // If taking was preformed mark which checker was doing it.
            newState.CheckerThatPreformedTakingX = x;
            newState.CheckerThatPreformedTakingY = y;
        }
        return newState;
    }
}