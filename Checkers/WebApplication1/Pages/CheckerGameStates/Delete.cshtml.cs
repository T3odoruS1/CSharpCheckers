using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApplication1.Pages.CheckerGameStates
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public DeleteModel(DAL.Db.AppDbContext context)
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

            var checkergamestate = await _context.CheckerGameStates.FirstOrDefaultAsync(m => m.Id == id);

            if (checkergamestate == null)
            {
                return NotFound();
            }
            else 
            {
                CheckerGameState = checkergamestate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CheckerGameStates == null)
            {
                return NotFound();
            }
            var checkergamestate = await _context.CheckerGameStates.FindAsync(id);

            if (checkergamestate != null)
            {
                CheckerGameState = checkergamestate;
                _context.CheckerGameStates.Remove(CheckerGameState);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
