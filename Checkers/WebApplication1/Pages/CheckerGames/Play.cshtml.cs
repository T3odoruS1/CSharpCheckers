using System.Text.Json;
using DAL.Db;
using DAL.FileSystem;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.CheckerGames;

public class Play : PageModel
{
    
    private readonly AppDbContext _context;

    public Play(AppDbContext context)
    {
        _context = context;
    }


    public CheckersBrain Brain { get; set; } = default!;
    public EGameSquareState[,] State { get; set; } = default!;
    
    public async Task<IActionResult> OnGet(int? id)
    {
        var game = await _context.CheckerGames
            .Include(g => g.GameOptions)
            .Include(g => g.CheckerGameStates)
            .FirstAsync(g => g.Id == id);

        var options = await _context.CheckerGameOptions
            .FirstAsync(o => o.Id == game.OptionsId);
        
        if (game == null || options == null || game.CheckerGameStates == null)
        {
            return NotFound();
        }

        Brain = new CheckersBrain(options);
        var jsonString = game.CheckerGameStates!.Last().SerializedGameBoard;
        var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
        State = FsHelpers.JaggedTo2D(jagged!);
        return Page();
    }
}