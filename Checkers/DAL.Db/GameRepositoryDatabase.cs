using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepositoryDatabase : IGameRepository
{
    public string Name { get; set; } = FsHelpers.DatabaseIdentifier;
    
    
    
    
    private readonly AppDbContext _dbContext;

    public GameRepositoryDatabase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<CheckerGame> GetAllGamesList()
    {
        return _dbContext.CheckerGames
            .Include(g => g.GameOptions)
            .Include(g => g.CheckerGameStates)
            .ToList();
    }

    public  CheckerGame GetGameById(int id)
    {
        return _dbContext.CheckerGames
            .Include(g => g.GameOptions)
            .Include(g => g.CheckerGameStates)
            .First(g => g.Id == id);
    }

    public int SavaGame(CheckerGame game)
    {
        _dbContext.CheckerGames.Add(game);
        _dbContext.SaveChanges();
        return game.Id;
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

    public void DeleteGameById(int id)
    {
        var gamesFromDb = GetGameById(id);
        _dbContext.CheckerGames.Remove(gamesFromDb);
        _dbContext.SaveChanges();
    }


}
