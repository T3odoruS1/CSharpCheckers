using System.Text.Json;
using DAL.Db;
using DAL.FileSystem;
using Domain;
using GameBrain;

namespace CheckersApp;


public class DalTester
{
    /// <summary>
    /// Method for testing Data Access Layer functionality before the launch of console app. Runs tests on
    /// database and filesystem access layers to verify their functionality before using the app. If something is wrong
    /// en exception will be thrown. Domain classes need to have Equals method implemented.
    /// </summary>
    /// <param name="gameRepoFs">File system game repository</param>
    /// <param name="gameRepoDb">Database game repository</param>
    /// <param name="optionRepoFs">File system option repository</param>
    /// <param name="optionRepoDb">Database option repository</param>
    /// <exception cref="Exception">Thrown if some aspect of DAL is not functional</exception>
    public void TestDalFunctionality(
        GameRepositoryFileSystem gameRepoFs, 
        GameRepositoryDatabase gameRepoDb, 
        GameOptionsRepositoryFileSystem optionRepoFs, 
        GameOptionsRepositoryDatabase optionRepoDb)
    {
        Console.WriteLine("Running pre launch checks.");
        
        
        // Creating game options for testing
        var optionsForFs = new CheckerGameOptions
        {
            Name = "TestOptions",
            Width = 12,
            Height = 10
        };
        var optionsForDb = new CheckerGameOptions
        {
            Name = "TestOptions",
            Width = 12,
            Height = 10
        };
        
        var optFsId = optionRepoFs.SaveGameOptions(optionsForFs);
        Console.WriteLine($"OptFsId = {optFsId}");
        var optDbId = optionRepoDb.SaveGameOptions(optionsForDb);
        Console.WriteLine($"OptDbId = {optDbId}");

        var board = new CheckersBrain(optionsForFs).GetBoard();

        var stateFs = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(FsHelpers.ToJaggedArray(board)),
            NextMoveByBlack = true
        };
        
        var stateDb = new CheckerGameState
        {
            SerializedGameBoard = JsonSerializer.Serialize(FsHelpers.ToJaggedArray(board)),
            NextMoveByBlack = true
        };

        var gameForFs = new CheckerGame
        {
            GameOptions = optionsForFs,
            Name = "Test game",
            Player1Type = EPlayerType.Human,
            Player1Name = "test person 1",
            Player2Type = EPlayerType.Ai,
            Player2Name = "Test ai name",
            CheckerGameStates = new List<CheckerGameState>()
        };

        gameForFs.CheckerGameStates.Add(stateFs);
        
        var gameForDb = new CheckerGame
        {
            GameOptions = optionsForDb,
            Name = "Test game",
            Player1Type = EPlayerType.Human,
            Player1Name = "test person 1",
            Player2Type = EPlayerType.Ai,
            Player2Name = "Test ai name",
            CheckerGameStates = new List<CheckerGameState>()

        };
        
        gameForDb.CheckerGameStates.Add(stateDb);

        

        var gameDbId = gameRepoDb.SavaGame(gameForDb);
        Console.WriteLine("GameDbId = " + gameDbId);
        var gameFsId = gameRepoFs.SavaGame(gameForFs);
        Console.WriteLine("GameFsId = " + gameFsId);

        
        // Game and options are saved by now.
        
        // Testing options repositories.
        var optionsFromFs = optionRepoFs.GetOptionsById(optFsId);
        var optionsFromDb = optionRepoDb.GetOptionsById(optDbId);

        if (!optionsFromFs.Equals(optionsForFs))
        {
            Console.WriteLine("Options from fs \n" + optionsFromFs);
            Console.WriteLine("Options for fs \n" + optionsForFs);
            throw new Exception("Options data access file system is not working properly.");
        }
        
        if (!optionsFromDb.Equals(optionsForDb))
        {
            throw new Exception("Options data access database is not working properly.");
        }

        var gameFromFs = gameRepoFs.GetGameById(gameFsId);
        var gameFromDb = gameRepoDb.GetGameById(gameDbId);

        if (!gameForFs.Equals(gameFromFs) || !gameFromFs.CheckerGameStates!.Last().Equals(stateFs))
        {
            throw new Exception("Game data access file system is not working properly.");

        }

        if (!gameForDb.Equals(gameFromDb) || !gameFromDb.CheckerGameStates!.Last().Equals(stateDb))
        {
            throw new Exception("Game data access database is not working properly.");

        }
        
        gameRepoDb.DeleteGameById(gameDbId);
        gameRepoFs.DeleteGameById(gameFsId);
        
        optionRepoDb.DeleteOptionsById(optDbId);
        optionRepoFs.DeleteOptionsById(optFsId);

        Console.WriteLine("Test passed successfully");

    }
}