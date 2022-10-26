using DataAccessLayer;

namespace DAL.FileSystem;

public class GameOptionsLastUsedFileSystem : IGameOptionLastSave
{
    
    private readonly string _lastOptDir = "." + 
                                          Path.DirectorySeparatorChar 
                                          + "lastUsedOptName";
    
    public string Name { get; set; } = FsHelpers.FileSystemIdentifier;
    
    public void NoteLastUsedOption(string optionName)
    {
        FsHelpers.CheckOrCreateDirectory(_lastOptDir);
        File.WriteAllText(FsHelpers.GetFileName("LastOption", _lastOptDir), optionName);    }


    

    public string GetLastUsedOptions()
{
    var fileContent = File.ReadAllText(FsHelpers.GetFileName("LastOption", _lastOptDir));

    return fileContent;
}
}