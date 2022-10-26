namespace DAL.FileSystem;

public static class FsHelpers
{
    
    private const string FileExtension = "json";

    public const string FileSystemIdentifier = "FS";
    
    
    
    // Universal methods for saving boards and game options into json
    
    public static void CheckOrCreateDirectory(string optionsDir)
    {
        if (!Directory.Exists(optionsDir))
        {
            Directory.CreateDirectory(optionsDir);
        }
    }
    
    
    public static string GetFileName(string id, string optionsDir)
    
        // id - filename
    {
        return optionsDir +
               Path.DirectorySeparatorChar +
               id + "." + FileExtension;
        
    }
    
    
    
     // Method is used to convert 2d into jagged arrays. Needed for json serialization
     public static T[][] ToJaggedArray<T>( T[,] twoDimensionalArray)
     {
         var rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
         var rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
         var numberOfRows = rowsLastIndex + 1;

         var columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
         var columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
         var numberOfColumns = columnsLastIndex + 1;

         var jaggedArray = new T[numberOfRows][];
         for (var i = rowsFirstIndex; i <= rowsLastIndex; i++)
         {
             jaggedArray[i] = new T[numberOfColumns];

             for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
             {
                 jaggedArray[i][j] = twoDimensionalArray[i, j];
             }
         }
         return jaggedArray;
     }
     
     
     // Used to convert jagged arrays into 2d. We get jagged form json, so it is necessary to convert
     public static T[,] JaggedTo2D<T>(T[][] source)
     {
         try
         {
             var firstDim = source.Length;
             var secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

             var result = new T[firstDim, secondDim];
             for (int i = 0; i < firstDim; ++i)
             for (int j = 0; j < secondDim; ++j)
                 result[i, j] = source[i][j];

             return result;
         }
         catch (InvalidOperationException)
         {
             throw new InvalidOperationException("The given jagged array is not rectangular.");
         } 
     }

     
     

}