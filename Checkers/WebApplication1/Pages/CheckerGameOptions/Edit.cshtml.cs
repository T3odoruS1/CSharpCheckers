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

namespace WebApplication1.Pages.CheckerGameOptions
{
    public class EditModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public EditModel(DAL.Db.AppDbContext context)
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

            var checkergameoptions =  await _context.CheckerGameOptions.FirstOrDefaultAsync(m => m.Id == id);
            if (checkergameoptions == null)
            {
                return NotFound();
            }
            CheckerGameOptions = checkergameoptions;
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

            _context.Attach(CheckerGameOptions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckerGameOptionsExists(CheckerGameOptions.Id))
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

        private bool CheckerGameOptionsExists(int id)
        {
          return (_context.CheckerGameOptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
