using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApplication1.Pages.CheckerGameStates
{
    public class EditModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public EditModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CheckerGameState CheckerGameState { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CheckerGameStates == null)
            {
                return NotFound();
            }

            var checkergamestate =  await _context.CheckerGameStates.FirstOrDefaultAsync(m => m.Id == id);
            if (checkergamestate == null)
            {
                return NotFound();
            }
            CheckerGameState = checkergamestate;
           ViewData["CheckerGameId"] = new SelectList(_context.CheckerGames, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CheckerGameState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckerGameStateExists(CheckerGameState.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CheckerGameStateExists(int id)
        {
          return (_context.CheckerGameStates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
