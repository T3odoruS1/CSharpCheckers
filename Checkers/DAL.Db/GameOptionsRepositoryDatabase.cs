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
    
    public List<string> GetGameOptionsList()
    {
        var res = _dbContext.CheckerGameOptions
            .Include(o => o.CheckerGames)
            .OrderBy(o => o.Name)
            .Select(o => o.Name)
            .ToList();
        return res;
    }

    public CheckerGameOptions GetGameOptions(string optionName)
    {
        return _dbContext.CheckerGameOptions.
            First(o => o.Name == optionName);
    }

    public void SaveGameOptions(CheckerGameOptions options)
    {
        if (!OptionNameAvailable(options.Name))
        {
            throw new ArgumentException($"You tried to save options with name {options.Name}. That name is already used");
        }

        _dbContext.CheckerGameOptions.Add(options);
        _dbContext.SaveChanges();
    }

    public void DeleteGameOptions(string optionName)
    {
        var optionsFromDb = GetGameOptions(optionName);
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

    public bool OptionNameAvailable(string name)
    {
        var optionsFromDb = _dbContext.CheckerGameOptions.
            FirstOrDefault(o => o.Name == name);
        return optionsFromDb == null;
    }

    public void DeleteAllOptions()
    {
        var allOptions = GetGameOptionsList();
        foreach (var option in allOptions)
        {
            DeleteGameOptions(option);
        }
    }
}