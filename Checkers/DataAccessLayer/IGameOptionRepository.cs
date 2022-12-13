using Domain;

namespace DataAccessLayer;

public interface IGameOptionRepository
{
    
    // Specify Db of Fs
    public string Name { get; set; }

    /// <summary>
    /// Method to get a list of all saved game options
    /// </summary>
    /// <returns>List of all saved game options</returns>
    List<CheckerGameOptions> GetGameOptionsList();
    
    
    /// <summary>
    /// Get specific game option by id
    /// </summary>
    /// <param name="id">game option id</param>
    /// <returns>game option</returns>
    CheckerGameOptions GetOptionsById(int id);
    
    /// <summary>
    /// Save game option into memory
    /// </summary>
    /// <param name="options">Game option to be saved</param>
    /// <returns>Id assigned to saved game option</returns>
    int SaveGameOptions(CheckerGameOptions options);
    
    /// <summary>
    /// Delete game option by id
    /// </summary>
    /// <param name="id">Id of the option to be deleted</param>
    void DeleteOptionsById(int id);

    /// <summary>
    /// Update game option
    /// </summary>
    /// <param name="options">Game option that is already saved but needs updating</param>
    void UpdateGameOptions(CheckerGameOptions options);

}