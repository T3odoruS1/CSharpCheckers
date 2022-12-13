
namespace DataAccessLayer;

public interface IGameOptionLastSave
{
    public string Name { get; set; }

    public void NoteLastUsedOptionId(int id, string dalMethod);

    public (int optId, string dalMethod) GetLastUsedOptionsId();
}