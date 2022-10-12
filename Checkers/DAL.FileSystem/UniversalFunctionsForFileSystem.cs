namespace DAL.FileSystem;

public class UniversalFunctionsForFileSystem
{
    
    private const string FileExtension = "json";
    
    
    
    public void CheckOrCreateDirectory(string optionsDir)
    {
        if (!Directory.Exists(optionsDir))
        {
            Directory.CreateDirectory(optionsDir);
        }
    }
    
    
    public string GetFileName(string id, string optionsDir)
    
        // id - filename
    {
        return optionsDir +
               Path.DirectorySeparatorChar +
               id + "." + FileExtension;
        
    }

}