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
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Domain.CheckerGameOptions> CheckerGameOptions { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CheckerGameOptions != null)
            {
                CheckerGameOptions = await _context.CheckerGameOptions.ToListAsync();
                if (CheckerGameOptions.Count == 0)
                {
                    var option = new Domain.CheckerGameOptions()
                    {
                        Name = "Default"
                    };
                    _context.CheckerGameOptions.Add(option);
                    await _context.SaveChangesAsync();
                    CheckerGameOptions = await _context.CheckerGameOptions.ToListAsync();

                    
                }
            }
        }
    }
}
