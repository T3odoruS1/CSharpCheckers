using System.Text.Json;
using DataAccessLayer;
using GameBrain;

namespace DAL.FileSystem;

public class GameStateRepositoryFileSystem : IGameStateRepository
{
    
    private const string FileExtension = "json";
    
    private readonly UniversalFunctionsForFileSystem _funcs = new UniversalFunctionsForFileSystem();

    private readonly string _optionsDir = "." + 
                                          Path.DirectorySeparatorChar + "gameState";
    
    
    // List of all saved games
    public List<string> GetGameStatesList()
    {
        _funcs.CheckOrCreateDirectory(_optionsDir);
        var res = new List<string>();
        foreach (var fileName in Directory.GetFileSystemEntries(
                     _optionsDir, "*" + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    // Convert int array into enum array 
    private EGameSquareState[,] ConvertIntArrayToEnumArray(int[,] board)
    {
        var enumBoard = new EGameSquareState[board.GetLength(0), board.GetLength(1)];

        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var j = 0; j < board.GetLength(1); j++)
            {
                enumBoard[i, j] = (EGameSquareState)board[i, j];
            }
        }

        return enumBoard;
    }

    // Converting board with enums into board with ints. Used for serialization to povide json with ints
    private int[,] ConvertEnumArrayToIntArray(EGameSquareState[,] board)
    {
        var integerBoard = new int[board.GetLength(0), board.GetLength(1)];

        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var j = 0; j < board.GetLength(1); j++)
            {
                integerBoard[i, j] = (int)board[i, j];
            }
        }

        return integerBoard;
    }

    
    // Get game state enum board
    public EGameSquareState[,] GetGameState(string id)
    {
        var fileContent = File.ReadAllText(_funcs.GetFileName(id, _optionsDir));
        var board = JsonSerializer.Deserialize<int[][]>(fileContent);
        var convertedBoard = JaggedTo2D(board!);
        if (board == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }
        EGameSquareState[,] enumBoard = ConvertIntArrayToEnumArray(convertedBoard);
        return enumBoard;
    }

    
    // Save current game state into json file
    public void SaveGameState(string id, EGameSquareState[,] board)
    {
        _funcs.CheckOrCreateDirectory(_optionsDir);
        var intBoard = ConvertEnumArrayToIntArray(board);
        var jaggedIntBoard = ToJaggedArray(intBoard);
        var fileContent = JsonSerializer.Serialize(jaggedIntBoard);
        File.WriteAllText(_funcs.GetFileName(id, _optionsDir), fileContent);
    }

    public void DeleteGameStates(string id)
    {
        File.Delete(_funcs.GetFileName(id, _optionsDir));
    }
    
    
    // Method is used to convert 2d into jagged arrays. Needed for json serialization
    private T[][] ToJaggedArray<T>( T[,] twoDimensionalArray)
    {
        int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
        int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
        int numberOfRows = rowsLastIndex + 1;

        int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
        int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
        int numberOfColumns = columnsLastIndex + 1;

        T[][] jaggedArray = new T[numberOfRows][];
        for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
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
    private static T[,] JaggedTo2D<T>(T[][] source)
    {
        try
        {
            int firstDim = source.Length;
            int secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

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