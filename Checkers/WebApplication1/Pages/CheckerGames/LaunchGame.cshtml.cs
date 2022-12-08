using DataAccessLayer;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.CheckerGames;

public class LaunchGame : PageModel
{
    private readonly IGameRepository _repo;


    public LaunchGame(IGameRepository repo)
    {
        _repo = repo;
    }

    public int GameId { get; set; }
    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return RedirectToPage("/Index", new {error = "No id"});
        }

        GameId =  (int)id;

        var game = _repo.GetGameById((int)id);

        // is it 2 player game?
        //      create 2 links 1 for tab1 and 1 for tab2
        // single player(human vs ai or ai vs ai)
        // Just redirect to play page

        if (game.Player1Type == EPlayerType.Human && game.Player2Type == EPlayerType.Human)
        {
            // Create 2 links 2 tabs
            return Page();
        }
        
        // Single player game
        return RedirectToPage("./Play", new {id = game.Id, playerNo = 0});
    }
}