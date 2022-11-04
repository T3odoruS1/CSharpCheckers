using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApplication1.Pages.CheckerGames
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public DeleteModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CheckerGame CheckerGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CheckerGames == null)
            {
                return NotFound();
            }

            var checkergame = await _context.CheckerGames.FirstOrDefaultAsync(m => m.Id == id);

            if (checkergame == null)
            {
                return NotFound();
            }
            else 
            {
                CheckerGame = checkergame;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CheckerGames == null)
            {
                return NotFound();
            }
            var checkergame = await _context.CheckerGames.FindAsync(id);

            if (checkergame != null)
            {
                CheckerGame = checkergame;
                _context.CheckerGames.Remove(CheckerGame);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
