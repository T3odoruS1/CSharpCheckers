using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Pages.CheckerGames
{
    public class CreateModel : PageModel
    {
        private DAL.Db.AppDbContext _context;
        private IGameRepository _repository;

        public CreateModel(DAL.Db.AppDbContext context, IGameRepository repository)
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
            if (!ModelState.IsValid)
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
            options.GameCount++;
            _context.CheckerGameOptions.Update(options);
            _repository.SavaGame(CheckerGame);

            if (CheckerGame.Player1Type == EPlayerType.Ai && CheckerGame.Player2Type == EPlayerType.Ai)
            {
                return RedirectToPage("./RobotBrawl", new { id = CheckerGame.Id });

            }

            if (CheckerGame.Player1Type == EPlayerType.Ai)
            {
                return RedirectToPage("./Play", new { id = CheckerGame.Id, playerNo = 1 });

            }if (CheckerGame.Player2Type == EPlayerType.Ai)
            {
                return RedirectToPage("./Play", new { id = CheckerGame.Id, playerNo = 0 });

            }
            return RedirectToPage("./LaunchGame", new { id = CheckerGame.Id });
        }
    }
}