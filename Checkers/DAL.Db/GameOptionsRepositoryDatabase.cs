using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameOptionsRepositoryDatabase: IGameOptionRepository
{
    public string Name { get; set; } = FsHelpers.DatabaseIdentifier;
    
    private readonly AppDbContext _dbContext;

    public GameOptionsRepositoryDatabase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }  
    
    public List<CheckerGameOptions> GetGameOptionsList()
    {
        var res = _dbContext.CheckerGameOptions
            .Include(o => o.CheckerGames)
            .ToList();
        return res;
    }

    public CheckerGameOptions GetOptionsById(int id)
    {
        return _dbContext.CheckerGameOptions.
            First(o => o.Id == id);
    }

    public int SaveGameOptions(CheckerGameOptions options)
    {
        _dbContext.CheckerGameOptions.Add(options);
        _dbContext.SaveChanges();
        return options.Id;
    }

    public void DeleteOptionsById(int id)
    {
        var optionsFromDb = GetOptionsById(id);
        _dbContext.CheckerGameOptions.Remove(optionsFromDb);
        _dbContext.SaveChanges();
    }

    public void UpdateGameOptions(CheckerGameOptions option)
    {
        var optionsFromDb = _dbContext.CheckerGameOptions.
            FirstOrDefault(o => o.Name == option.Name);
        optionsFromDb!.Name = option.Name;
        optionsFromDb.Width = option.Width;
        optionsFromDb.Height = option.Height;
        optionsFromDb.WhiteStarts = option.WhiteStarts;
        optionsFromDb.TakingIsMandatory = option.TakingIsMandatory;
        optionsFromDb.GameCount = option.GameCount;
        optionsFromDb.CheckerGames = option.CheckerGames;

        _dbContext.SaveChanges();

    }

}