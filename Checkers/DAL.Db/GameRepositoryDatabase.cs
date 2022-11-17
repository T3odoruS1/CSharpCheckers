using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepositoryDatabase : IGameGameRepository
{
    public string Name { get; set; } = FsHelpers.DatabaseIdentifier;
    
    
    
    
    private readonly AppDbContext _dbContext;

    public GameRepositoryDatabase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }    
    public List<string> GetAllGameNameList()

    {
        var res = _dbContext.CheckerGames
            .Include(o => o.CheckerGameStates)
            .OrderBy(o => o.Name)
            .ToList();
        return res.Select(o => o.Name).ToList();
    }

    public CheckerGame GetGameByName(string name)
    {
        return _dbContext.CheckerGames
            .Include(g => g.CheckerGameStates)
            .Include(g => g.GameOptions)
            .First(o => o.Name == name);
    }
    
    public  CheckerGame GetGameById(int id)
    {
        return _dbContext.CheckerGames
            .Include(g => g.GameOptions)
            .Include(g => g.CheckerGameStates)
            .First(g => g.Id == id);
    }

    public void SavaGame(CheckerGame game)
    {
        if (!GameNameAvailable(game.Name))
        {
            throw new ArgumentException($"You tried to save game with name {game.Name}. That name is already used.");
        }

        
        // var optionsFromDb = _dbContext.CheckerGameOptions.
        //     First(o => o.Id == game.GameOptions!.Id);
        // Console.WriteLine(optionsFromDb);
        //
        // if (optionsFromDb == null)
        // {
        //     throw new Exception();
        // }
        //
        // optionsFromDb!.GameCount = optionsFromDb.GameCount + 1;
        //
        
        _dbContext.CheckerGames.Add(game);
        _dbContext.SaveChanges();
    }

    public void UpdateGame(CheckerGame game)
    {
         var gamesFromDb = _dbContext.CheckerGames.
                    FirstOrDefault(o => o.Name == game.Name);
         if (gamesFromDb == null)
         {
             throw new ArgumentException("Game you wanted to update was not found in the database.");
         }
         gamesFromDb.GameOptions = game.GameOptions;
         gamesFromDb.CheckerGameStates = game.CheckerGameStates;
         gamesFromDb.GameOverAt = game.GameOverAt;
         gamesFromDb.GameWonBy = game.GameWonBy;
         gamesFromDb.Player1Name = game.Player1Name;
         gamesFromDb.Player2Name = game.Player2Name;

         _dbContext.SaveChanges();

    }

    public void AddNewGameState(CheckerGameState state)
    {
        throw new NotImplementedException();
    }

    public void DeleteGameByName(string name)
    {
        var gamesFromDb = GetGameByName(name);
        _dbContext.CheckerGames.Remove(gamesFromDb);
        _dbContext.SaveChanges();
    }

    public bool GameNameAvailable(string name)
    {
        var gamesFromDb = _dbContext.CheckerGames.
            FirstOrDefault(o => o.Name == name);
        return gamesFromDb == null;

    }

    public void DeleteAllGames()
    {
        var allGames = GetAllGameNameList();
        foreach (var game in allGames)
        {
            DeleteGameByName(game);
        }
    }
};
