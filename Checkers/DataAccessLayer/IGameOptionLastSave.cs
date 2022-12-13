
namespace DataAccessLayer;

public interface IGameOptionLastSave
{
    public string Name { get; set; }

    public void NoteLastUsedOptionId(int id);

    public int GetLastUsedOptionsId();
}