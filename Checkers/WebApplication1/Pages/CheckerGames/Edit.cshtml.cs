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

namespace WebApplication1.Pages.CheckerGames
{
    public class EditModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public EditModel(DAL.Db.AppDbContext context)
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

            var checkergame =  await _context.CheckerGames.FirstOrDefaultAsync(m => m.Id == id);
            if (checkergame == null)
            {
                return NotFound();
            }
            CheckerGame = checkergame;
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

            _context.Attach(CheckerGame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckerGameExists(CheckerGame.Id))
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

        private bool CheckerGameExists(int id)
        {
          return (_context.CheckerGames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
