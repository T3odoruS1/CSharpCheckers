using DataAccessLayer;
using Domain;

namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionsRepository
{
    public List<string> GetGameOptionsList()
    {
        throw new NotImplementedException();
    }
    
    public GameOptions GetGameOptions(string id)
    {
        throw new NotImplementedException();
    }

    public void SaveGameOptions(string id, GameOptions options)
    {
        throw new NotImplementedException();
    }

    public void DeleteGameOptions(string id)
    {
        throw new NotImplementedException();
    }
}