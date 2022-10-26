using DataAccessLayer;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepository : IGameGameRepository
{
    private readonly AppDbContext _dbContext;

    public string Name { get; set; } = "DB";

    public GameRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }    public List<string> GetAllGameNameList()
    
    {
        throw new NotImplementedException();
    }

    public CheckerGame GetGame(string name)
    {
        throw new NotImplementedException();
    }

    public void SavaGame(CheckerGame game)
    {
        throw new NotImplementedException();
    }

    public void UpdateGame(CheckerGame game)
    {
        throw new NotImplementedException();
    }

    public void AddNewGameState(CheckerGameState state)
    {
        throw new NotImplementedException();
    }

    public void DeleteGameByName(string name)
    {
        throw new NotImplementedException();
    }
};
