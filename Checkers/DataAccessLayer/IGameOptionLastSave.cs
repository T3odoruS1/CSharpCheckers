
namespace DataAccessLayer;

public interface IGameOptionLastSave
{
    public string Name { get; set; }

    /// <summary>
    /// On closing the app this method saves id of the last used game options with repository type that was used.
    /// </summary>
    /// <param name="id">Id of the options that were used before closing the app</param>
    /// <param name="dalMethod">type of repository</param>
    public void NoteLastUsedOptionId(int id, string dalMethod);

    /// <summary>
    /// On launch of the app returns a tuple with last used option id and repo type
    /// </summary>
    /// <returns>last used option id and repo type</returns>
    public (int optId, string dalMethod) GetLastUsedOptionsId();
}