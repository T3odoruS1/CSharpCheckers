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
    public readonly IGameRepository Repo;


    public Play(IGameRepository repo)
    {
        Repo = repo;
    }
    
    
    // Implement bind property
    
    
    public CheckersBrain Brain { get; set; } = default!;
    public EGameSquareState[,] Board { get; set; } = default!;
    public CheckerGameState GameState { get; set; } = default!;
    public CheckerGame Game { get; set; } = default!;

    public int? InitX { get; set; }
    public int? InitY { get; set; }
    public bool FirstReq { get; set; }
    public bool? WonByBlack { get; set; }
    public int PlayerNo { get; set; }

    public IActionResult OnGet(int? id, int? firstReq, int? x, int? y, int? initX, int? initY, int? playerNo, int? pass, bool? robotBrawlMode)
    {

        #region Initial page initialization

        

        // Check if it is the first request(initial player choice)
        if (id == null)
        {
            return RedirectToPage("/Index", new { error = "No game id was provided" });
        }

        if (playerNo is null or < 0 or > 2)
        {
            
            return RedirectToPage("/Index",
                new { error = "No player number was provided or is not in allowed range." });
        }



        PlayerNo = playerNo.Value;

        if (firstReq != null)
        {
            FirstReq = true;
        }


        Game = Repo.GetGameById(id.Value);

        var options = Game.GameOptions;

        if (Game.CheckerGameStates == null || options == null)
        {
            return NotFound();
        }

        
        

        // Get brain and state
        Brain = new CheckersBrain(options);
        GameState = Game.CheckerGameStates.Last();
        var jsonString = GameState.SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        Board = FsHelpers.JaggedTo2D(jagged!);
        Brain.SetGameBoard(Board, GameState);
        Brain.TestIfGameOver();
        var movables = Brain.GetAllMoves(Brain.NextMoveByBlack());
        if (movables.Count == 0)
        {
            Brain.ToggleNextMove();
            var newState = new CheckerGameState
            {
                SerializedGameBoard = JsonSerializer.Serialize(
                    FsHelpers.ToJaggedArray(Brain.GetBoard())),
                NextMoveByBlack = Brain.NextMoveByBlack()
            };
            Game.CheckerGameStates.Add(newState);
            Repo.UpdateGame(Game);
            
        }
        
        #endregion

        
        if (pass != null)
        {
            PassTheTakingMove();
        }


        if (firstReq != null && x != null && y != null)
        {
            InitX = x;
            InitY = y;
        }


        // Unserialize json game board. Get game board.

        if (Game.GameOverAt != null)
        {
            WonByBlack = Game.GameWonBy == Game.Player2Name;
        }

        

        if (!IsPlayerMove() && OpositePlayerIsAi() && !Brain.IsGameOver())
        {
            var move = Brain.MakeMoveByAi(playerNo != 1);
            UpdateRepo(move.x, move.y);
            return Page();

        }

        // Check if player wants to switch the checker that he wants to use
        if (x != null && y != null && initX != null && initY != null &&
            x == initX && y == initY)
        {
            return RedirectToPage("Play", new { id = Game.Id, playerNo = PlayerNo });
        }

        CheckIfMakeAnotherTake();

        // If params needed for the move are not null - make move
        if (firstReq != null || x == null || y == null || initX == null || initY == null) return Page();


        Brain.MoveChecker(initX.Value, initY.Value, x.Value, y.Value);
        
        
        UpdateRepo(x.Value, y.Value);

        return Page();
    }


    /// <summary>
    /// Check if checker can do another take. All parameters changed while this check are inside of this class
    /// </summary>
    private void CheckIfMakeAnotherTake()
    {
        if (GameState.CheckerThatPreformedTakingX == null || GameState.CheckerThatPreformedTakingY == null || !IsPlayerMove()) return;
        if (!Brain.CanTake(GameState.CheckerThatPreformedTakingX.Value,
                GameState.CheckerThatPreformedTakingY.Value)) return;
        FirstReq = true;
        InitX = GameState.CheckerThatPreformedTakingX;
        InitY = GameState.CheckerThatPreformedTakingY;
    }
    
    /// <summary>
    /// Check if now is this players move
    /// </summary>
    /// <returns>True if it is this players move</returns>
    public bool IsPlayerMove()
    {
        if (PlayerNo == 0 && !GameState.NextMoveByBlack)
        {
            return true;
        }if (PlayerNo == 1 && GameState.NextMoveByBlack)
        {
            return true;
        }
        return false;
    }


    private void UpdateRepo(int x, int y)
    {
        var newState = CreateNewState(x, y, false);
        Game.CheckerGameStates!.Add(newState);
        GameState = newState;
        CheckIfMakeAnotherTake();
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
        else
        {
            FirstReq = false;
        }

        return newState;
    }
    
    /// <summary>
    /// Pass the next taking move
    /// </summary>
    private void PassTheTakingMove()
    {
        var newState = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(
                FsHelpers.ToJaggedArray(Brain.GetBoard())),
            NextMoveByBlack = !Brain.NextMoveByBlack()
        };
        Game.CheckerGameStates!.Add(newState);
        Repo.UpdateGame(Game);
        Brain.ToggleNextMove();
        GameState = newState;
    }

    /// <summary>
    /// Check if current player is AI
    /// </summary>
    /// <returns></returns>
    public bool OpositePlayerIsAi()
    {
        switch (PlayerNo)
        {
            case 1 when !Brain.NextMoveByBlack() && Game.Player1Type == EPlayerType.Ai:
            case 0 when Brain.NextMoveByBlack() && Game.Player2Type == EPlayerType.Ai:
                return true;
            default:
                return false;
        }
    }

    public bool AiInTheGame()
    {
        return Game.Player1Type == EPlayerType.Ai || Game.Player2Type == EPlayerType.Ai;
    }
}