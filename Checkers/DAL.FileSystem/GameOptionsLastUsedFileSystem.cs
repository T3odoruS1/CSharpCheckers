using DataAccessLayer;

namespace DAL.FileSystem;

public class GameOptionsLastUsedFileSystem : IGameOptionLastSave
{
    private readonly string _lastOptDir = "." +
                                          Path.DirectorySeparatorChar
                                          + "lastUsedOptName";

    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;

    public void NoteLastUsedOptionId(int id)
    {
        FsHelpers.CheckOrCreateDirectory(_lastOptDir);
        File.WriteAllText(FsHelpers.GetFileName("LastOption", _lastOptDir), id.ToString());
    }


    public int GetLastUsedOptionsId()
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName("LastOption", _lastOptDir));

        return Int32.Parse(fileContent);
    }
}