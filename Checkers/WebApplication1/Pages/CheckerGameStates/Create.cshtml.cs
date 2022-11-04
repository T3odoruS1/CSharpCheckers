using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain;

namespace WebApplication1.Pages.CheckerGameStates
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
        ViewData["CheckerGameId"] = new SelectList(_context.CheckerGames, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckerGameState CheckerGameState { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CheckerGameStates == null || CheckerGameState == null)
            {
                return Page();
            }

            _context.CheckerGameStates.Add(CheckerGameState);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
