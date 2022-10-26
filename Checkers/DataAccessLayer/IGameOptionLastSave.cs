
namespace DataAccessLayer;

public interface IGameOptionLastSave
{
    public string Name { get; set; }

    public void NoteLastUsedOption(string optionName);

    public string GetLastUsedOptions();
}