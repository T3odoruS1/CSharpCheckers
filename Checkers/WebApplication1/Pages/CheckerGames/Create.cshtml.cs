using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using DAL.FileSystem;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Pages.CheckerGames
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public CreateModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["OptionsId"] = new SelectList(_context.CheckerGameOptions, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckerGame CheckerGame { get; set; } = default!;
        
        public SelectList OptionsSelectList { get; set; } = default!;



        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CheckerGames == null || CheckerGame == null)
            {
                return Page();
            }

            _context.CheckerGames.Add(CheckerGame);
            Console.WriteLine(CheckerGame.GameOptions);
            Domain.CheckerGameOptions options = await _context.CheckerGameOptions.FirstAsync(o => o.Id == CheckerGame.OptionsId);
            options.GameCount++;
            var brain = new CheckersBrain(options);
            var board = brain.GetBoard();
            var jaggedBoard = FsHelpers.ToJaggedArray(board);
            var serializedString = JsonSerializer.Serialize(jaggedBoard);
            var gameState = new CheckerGameState();
            gameState.SerializedGameBoard = serializedString;
            gameState.NextMoveByBlack = !options.WhiteStarts;
            CheckerGame.CheckerGameStates = new List<CheckerGameState>();
            CheckerGame.CheckerGameStates.Add(gameState);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
