using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class AppDbContext : DbContext
{
    public DbSet<CheckerGame> CheckerGames { get; set; } = default!;
    public DbSet<CheckerGameOptions> CheckerGameOptions { get; set; } = default!;
    public DbSet<CheckerGameState> CheckerGameStates { get; set; } = default!;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}




