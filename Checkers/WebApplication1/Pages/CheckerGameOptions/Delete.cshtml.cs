using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApplication1.Pages.CheckerGameOptions
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public DeleteModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Domain.CheckerGameOptions CheckerGameOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CheckerGameOptions == null)
            {
                return NotFound();
            }

            var checkergameoptions = await _context.CheckerGameOptions.FirstOrDefaultAsync(m => m.Id == id);

            if (checkergameoptions == null)
            {
                return NotFound();
            }
            else 
            {
                CheckerGameOptions = checkergameoptions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CheckerGameOptions == null)
            {
                return NotFound();
            }
            var checkergameoptions = await _context.CheckerGameOptions.FindAsync(id);

            if (checkergameoptions != null && checkergameoptions.GameCount == 0)
            {
                CheckerGameOptions = checkergameoptions;
                _context.CheckerGameOptions.Remove(CheckerGameOptions);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
