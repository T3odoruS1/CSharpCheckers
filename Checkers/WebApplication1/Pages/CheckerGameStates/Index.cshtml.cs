using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Pages.CheckerGameStates
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }



        public SelectList GameSelectList { get; set; } = default!;

        public IList<CheckerGameState> CheckerGameState { get; set; } = default!;

        // Id - optional for state filtering.
        public async Task OnGetAsync()
        {
            if (_context.CheckerGameStates != null)
            {
                CheckerGameState = await _context.CheckerGameStates
                    .Include(c => c.CheckerGame).ToListAsync();
                GameSelectList = new SelectList(_context.CheckerGames, "Id", "Name");
            }
        }

        public async Task OnPostAsync(int? id)
        {
            if (id == null)
            {
                return;
            }

            CheckerGameState = await _context.CheckerGameStates
                .Include(c => c.CheckerGame)
                .Where(s => s.CheckerGame!.Id == id)
                .ToListAsync();
            GameSelectList = new SelectList(_context.CheckerGames, "Id", "Name");

        }

        public async Task DisableFilters()
        {
            OnGetAsync();
        }
    }
}