using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Pages.CheckerGames
{
    public class CreateModel : PageModel
    {
        private  DAL.Db.AppDbContext _context;
        private IGameGameRepository _repository;

        public CreateModel(DAL.Db.AppDbContext context, IGameGameRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult OnGet()
        {
            OptionsSelectList = new SelectList(_context.CheckerGameOptions, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckerGame CheckerGame { get; set; } = default!;
        
        public SelectList OptionsSelectList { get; set; } = default!;



        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CheckerGames == null || CheckerGame == null)
            {
                return Page();
            }

          var optId = CheckerGame.OptionsId;
          var options = _context.CheckerGameOptions.First(o => o.Id == optId);

          CheckerGame.GameOptions = options;
          var state = new CheckerGameState();
          var brain = new CheckersBrain(options);
          state.SerializedGameBoard = JsonSerializer.Serialize(FsHelpers.ToJaggedArray(brain.GetBoard()));
          CheckerGame.CheckerGameStates = new List<CheckerGameState>();
          CheckerGame.CheckerGameStates.Add(state);
            _repository.SavaGame(CheckerGame);

            return RedirectToPage("./Index");
        }
    }
}
