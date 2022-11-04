using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain;
using GameBrain;

namespace WebApplication1.Pages.CheckerGameOptions
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
            return Page();
        }

        [BindProperty]
        public Domain.CheckerGameOptions CheckerGameOptions { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CheckerGameOptions == null || CheckerGameOptions == null)
            {
                return Page();
            }

            _context.CheckerGameOptions.Add(CheckerGameOptions);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
