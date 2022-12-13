using DataAccessLayer;

namespace DAL.FileSystem;

public class GameOptionsLastUsedFileSystem : IGameOptionLastSave
{
    private readonly string _lastOptDir = "." +
                                          Path.DirectorySeparatorChar
                                          + "lastUsedOptName";

    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;

    public void NoteLastUsedOptionId(int id, string dalMethod)
    {
        FsHelpers.CheckOrCreateDirectory(_lastOptDir);
        var data = id + ";" + dalMethod;
        File.WriteAllText(FsHelpers.GetFileName("LastOption", _lastOptDir), data);
    }


    public (int optId, string dalMethod) GetLastUsedOptionsId()
    {
        var fileContent = File.ReadAllText(FsHelpers.GetFileName("LastOption", _lastOptDir));
        Console.WriteLine(fileContent);
        var data = fileContent.Split(";");
        return (Int32.Parse(data[0]), data[1]);
    }
}