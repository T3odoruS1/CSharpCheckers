using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Db;

public class AppDbContextFactory: 
    IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Create db context
    /// </summary>
    /// <param name="args"></param>
    /// <returns>application db context</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=/Users/edgarvildt/Developer/CheckerDb/checker.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}