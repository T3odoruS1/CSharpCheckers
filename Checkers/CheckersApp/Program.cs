
using System.Text.Json;
using CheckersApp;
using ConsoleUI;
using DAL.Db;
using DAL.FileSystem;
using DataAccessLayer;
using Domain;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using static System.ConsoleKey;

var gameOptions = new CheckerGameOptions();

/*
Gameplay not implemented yet

 */



var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
.UseSqlite("Data Source=/Users/edgarvildt/Developer/CheckerDb/checker.db").Options;
var ctx = new AppDbContext(dbOptions);


var lastUsedRepoFs = new GameOptionsLastUsedFileSystem();

var lastUsedRepo = lastUsedRepoFs;

IGameRepository gameRepoFs = new GameRepositoryFileSystem();
IGameRepository gameRepoDb = new GameRepositoryDatabase(ctx);

var gameRepo = gameRepoDb;

IGameOptionRepository optionRepoFs = new GameOptionsRepositoryFileSystem();
IGameOptionRepository optionsRepoDb = new GameOptionsRepositoryDatabase(ctx);

var optionRepo = optionsRepoDb;


CheckersBrain checkersBrain;
gameOptions.Name = "Default";
gameOptions.CheckerGames = new List<CheckerGame>();



#region MenuInitialization



var thirdMenu = new Menu(EMenuLevel.Other,
     "          > =◉= Checkers third level =◉= <",
     new List<MenuItem>()
     {
          new MenuItem("N", "Easter egg 💀", EasterEggMethod),
     });
var secondMenu = new Menu(EMenuLevel.Second,
     "          > =◉= Checkers options =◉= <",
     new List<MenuItem>()
{
     new MenuItem("N", "Current Game Options 🤔", PrintCurrentGameOptions),
     new MenuItem("C", "Create options 📝", CreateGameOptions),
     new MenuItem("O", "List saved options 📁", ListGameOptions),
     new MenuItem("L", "Load options 💿", LoadGameOptions),
     new MenuItem("D", "Delete options 🗑️", DeleteOptions),
     new MenuItem("S", "Save current options 💾️", SaveCurrentOptions),
     new MenuItem("P", "Change data saving method 🔄", ChangeRepoType),
     new MenuItem("T", "Something to be found here 😉", thirdMenu.RunMenu)

});


var mainMenu = new Menu(EMenuLevel.Main,
     "          > =◉= CHECKERS =◉= <",
     new List<MenuItem>()
     {
          new MenuItem("N", "New Game 🎮", DoNewGame),
          new MenuItem("L", "Load Game 💿", LoadGame),
          new MenuItem("D", "Delete SavedGame 🗑️", DeleteSavedGame),

          new MenuItem("O", "Options ⚙️", secondMenu.RunMenu)
     });

#endregion




#region Load last used game option. And save current options on exit

try
{
     gameOptions = optionRepo.GetGameOptions(lastUsedRepo.GetLastUsedOptions());

}
catch (Exception)
{
     gameOptions = new CheckerGameOptions();
}

mainMenu.RunMenu();

lastUsedRepo.NoteLastUsedOption(gameOptions.Name);

#endregion


string ChangeRepoType()
{
     Console.Clear();

     if (gameRepo.Name == FsHelpers.DatabaseIdentifier)
     {
          gameRepo = gameRepoFs;
          Console.WriteLine("Changed from Database to Filesystem");
     }else if (gameRepo.Name == FsHelpers.FileSystemIdentifier)
     {
          gameRepo = gameRepoDb;
          Console.WriteLine("Changed from Filesystem to Database");
     }

     if (optionRepo.Name == FsHelpers.DatabaseIdentifier)
     {
          optionRepo = optionRepoFs;
     }else if (optionRepo.Name == FsHelpers.FileSystemIdentifier)
     {
          optionRepo = optionsRepoDb;
     }
     WaitForUserInput();

     
     return "B";
}

string DeleteSavedGame()
{
     Console.Clear();
     var allSavedGames = gameRepo.GetAllGameNameList();
     var i = 1;
     var gameDict = new Dictionary<int, string>();
     foreach (var savedGame in allSavedGames)
     {
          var checkerGame = gameRepo.GetGameByName(savedGame);
          Console.WriteLine($"{i}) - {savedGame}");
          Console.WriteLine($"Options : {checkerGame.GameOptions}");
          var jaggedBoard =
               JsonSerializer.Deserialize<EGameSquareState[][]>(
                    checkerGame.CheckerGameStates!
                         .Last()
                         .SerializedGameBoard);
          var gameBoard = FsHelpers.JaggedTo2D(jaggedBoard!);
          UI.DrawGameBoard(gameBoard, null, null);
          gameDict.Add(i, savedGame);
          i++;
     }

     Console.WriteLine("Choose a game you want to delete. If you dont want to delete a game choose 'X'");
     var userChoice = Console.ReadLine()!.ToUpper().Trim();
     if (userChoice == "X")
     {
          return "B";
     }

     if (!int.TryParse(userChoice, out var a) || !gameDict.ContainsKey(a))
     {
          DeleteSavedGame();
     }
     else
     {
          var gameToDelete = gameRepo.GetGameByName(gameDict[a]);
          gameToDelete.GameOptions!.GameCount--;
          optionRepo.UpdateGameOptions(gameToDelete.GameOptions);
          gameRepo.DeleteGameByName(gameDict[a]);
          
     }
     return "B";
}
string LoadGame()
{
     Console.Clear();
     var allSavedGames = gameRepo.GetAllGameNameList();
     var i = 1;
     var gameDict = new Dictionary<int, string>();
     foreach (var savedGame in allSavedGames)
     {
          var checkerGame = gameRepo.GetGameByName(savedGame);
          Console.WriteLine($"\n{i}) - {checkerGame.Name}");
          
          Console.WriteLine(checkerGame.GameOptions);
          var jaggedBoard =
               JsonSerializer.Deserialize<EGameSquareState[][]>(
                    checkerGame.CheckerGameStates!
                         .Last()
                         .SerializedGameBoard);
          var gameBoard = FsHelpers.JaggedTo2D(jaggedBoard!);
          UI.DrawGameBoard(gameBoard, null, null);
          gameDict.Add(i, savedGame);
          i++;
     }
     
     Console.WriteLine("Choose the game you want to load");
     var userChoice = Console.ReadLine();
     if (!int.TryParse(userChoice, out var a))
     {
          LoadGame();
     }
     else
     {
          if (!gameDict.ContainsKey(int.Parse(userChoice)))
          {
               LoadGame();
          }
          else
          {
               var gameToBeLoaded = gameRepo.GetGameByName(gameDict[a]);
               gameOptions = gameToBeLoaded.GameOptions;
               var gameRunner = new GameRunner(gameRepo, gameToBeLoaded.Id);
               gameRunner.RunGame();

          }
     
     }
     WaitForUserInput();
     return "B";
}


// Make new game ond use UI method to print the board. Some test code commented out
string DoNewGame()
{
     Console.Clear();
     Console.WriteLine("\n\n\n\nNew game! Time to play!\n");
     Console.WriteLine("Game will be using your current game options");

     if (gameOptions.Name == "")
     {
          
          Console.WriteLine("Looks like your game options are not saved.");
          Console.WriteLine("To start this game you should save them. Give them the name please\n");
          SaveCurrentOptions();
     }
     
     // Make game brains produce a game board by given options
     checkersBrain = new CheckersBrain(gameOptions);

     
     var newGame = new CheckerGame
     {
          GameOptions = gameOptions
     };

     // newGame.OptionsId = gameOptions.Id;
     string gameName;

     do
     {
          Console.Clear();
          Console.WriteLine("Enter a name for the game please\n");
          gameName = Console.ReadLine()!;
     } while (gameName == "" &&
              !(gameName.Length > 0) &&
              !(gameName.Length < 128) &&
              gameName.Contains('"')&&
              !gameRepo.GameNameAvailable(gameName));

     newGame.Name = gameName;
     // P1 name config
     string playerName;
     do
     {
          
          Console.Clear();
          Console.WriteLine("\n\n\n\nPlease enter first user name between 1 and 128 symbols\n");
          playerName = Console.ReadLine()!;
          
          
     } while (playerName.Length is < 129 and > 0 && playerName.Contains('"'));

     newGame.Player1Name = playerName;

     ConsoleKey key;
     do
     {
          Console.Clear();
          Console.WriteLine("Is player 1 human or ai? If ai press A, if human press H");
          key = Console.ReadKey(true).Key;

     } while (key != A && key != H);

     switch (key)
     {
          case A:
               newGame.Player1Type = EPlayerType.Ai;
               break;
          case H:
               newGame.Player1Type = EPlayerType.Human;
               break;
         
     }

     // P2 config
     do
     {
          
          Console.Clear();
          Console.WriteLine("\n\n\n\nPlease enter first user name between 1 and 128 symbols\n");
          playerName = Console.ReadLine()!;
          
          
     } while (playerName.Length is < 129 and > 0 && playerName.Contains('"'));
     
     newGame.Player2Name = playerName;


     
     ConsoleKey key2;
     do
     {
          Console.Clear();
          Console.WriteLine("Is player 2 human or ai? If ai press A, if human press H");
          key2 = Console.ReadKey(true).Key;

     } while (key2 != A && key2 != H);

     switch (key2)
     {
          case A:
               newGame.Player2Type = EPlayerType.Ai;
               break;
          case H:
               newGame.Player2Type = EPlayerType.Human;
               break;
         
     }

     var jaggedBoard = FsHelpers.ToJaggedArray(checkersBrain.GetBoard());
     var serializedBoardString = JsonSerializer.Serialize(jaggedBoard);

     newGame.CheckerGameStates = new List<CheckerGameState>();

     var gameState = new CheckerGameState
     {
          NextMoveByBlack = !gameOptions.WhiteStarts,
          SerializedGameBoard = serializedBoardString
     };

     newGame.CheckerGameStates.Add(gameState);
     optionRepo.UpdateGameOptions(gameOptions);

     gameRepo.SavaGame(newGame);
     Console.WriteLine($"Game: {newGame}");
     WaitForUserInput();


     return "B";
}



// Run delete options submenu. Delete one of them or exit this menu
string DeleteOptions()
{
     var optionToDelete = RunSubmenu();
     if (optionToDelete is "B" or "M" or "X") return optionToDelete;
     var option = optionRepo.GetGameOptions(optionToDelete);
     if (option.GameCount > 0)
     {
          Console.WriteLine("\nThere is a game that uses these options. You can not delete those.");
          WaitForUserInput();

     }
     else
     {
          optionRepo.DeleteGameOptions(optionToDelete);
          Console.Clear();
          Console.WriteLine("Deleted");
          WaitForUserInput();
     }

     return "B";
}


// Save current options into json file
string SaveCurrentOptions()
{
     
     
     
     Console.WriteLine("Current game option:");
     Console.WriteLine(gameOptions);
     Console.CursorVisible = true;
     Console.WriteLine("How would you like to name this game option? You should choose a name that is not already used.");
     Console.WriteLine("Hint: Taken names:\n\n");
     if (optionRepo.GetGameOptionsList().Count != 0)
     {
          foreach (var opt in optionRepo.GetGameOptionsList())
          {

               Console.WriteLine(opt);

          }
     }

     string userInputForFileName;
     do
     {
          Console.WriteLine("Enter name for these options");
          userInputForFileName = Console.ReadLine()!; 
     } while (userInputForFileName == "" || 
              !optionRepo.OptionNameAvailable(userInputForFileName) ||
              userInputForFileName.Length is < 1 or > 128);

     gameOptions.Name = userInputForFileName;
     
     optionRepo.SaveGameOptions(gameOptions);
     Console.CursorVisible = false;
     Console.WriteLine($"Game options saved with name {gameOptions.Name}");
     WaitForUserInput();
     return "B";
}


//List all game options
string ListGameOptions()
{

     Console.Clear();
     Console.WriteLine("List of game options:\n\n");
     PrintOutAllSavedGameOptions();
     
     WaitForUserInput();
     return "B";
}


// Run game options menu and load one of them or exit this menu
string LoadGameOptions()
{
     
     var optionToLoad = RunSubmenu();
     if (optionToLoad is "B" or "M" or "X") return optionToLoad;
     gameOptions = optionRepo.GetGameOptions(optionToLoad);
     Console.Clear();
     Console.WriteLine("Loaded!");
     WaitForUserInput();
     return "B"; 
}


// Return option that user has chosen or the shortcut for navigating through the menu
string RunSubmenu()
{
     
     var menuItems = new List<MenuItem>();
     var i = 0;
     var listOfOptions = new List<string>();
     
     // For each option choice shortcut is a number in string form, other shortcuts are standard
     foreach (var gameOption in optionRepo.GetGameOptionsList())
     {
          var option = optionRepo.GetGameOptions(gameOption);
          menuItems.Add(new MenuItem(i.ToString(), 
               option.Name + ":" + 
               $"Height: {option.Height}," +
               $" Width: {option.Width}," +
               $" White starts: {option.WhiteStarts}," +
               $" Taking mandatory: {option.TakingIsMandatory}," +
               $" Amount of games: {option.GameCount}", 
               null));
          listOfOptions.Add(gameOption);
          i++;
     }
     var loadMenu = new Menu(EMenuLevel.Other,
          ">  NB! You can delete only those games, that are not used in any games.  <", menuItems);
     
     var userChoice = loadMenu.RunMenu();

     
     // If user choice is number then return user choice return one of the options that he has chose.
     // Else return  user choice
     return !int.TryParse(userChoice, out var j) ? userChoice : listOfOptions[j];
}


// Making new game option
string CreateGameOptions()
{
     Console.Clear();
     gameOptions = new CheckerGameOptions
     {
          Name = ""
     };
     ValidateUserGameOptions();
     Console.WriteLine("Alright this is your new game options:");
     gameOptions.GameCount = 0;
     Console.WriteLine(gameOptions);
     WaitForUserInput();

     return "B";
}

// Helper method for CreateGameOption. Validates inputs and sets the parameters for gameOptions object
void ValidateUserGameOptions()
{
     bool widthConvertedSuccessfully;
     bool heightConvertedSuccessfully;
     int convertedWidth;
     int convertedHeight;
     var numbersEven = false;
     do
     {
          Console.WriteLine("Enter board width. Only even numbers starting from 4 -->  ");
          widthConvertedSuccessfully = int.TryParse(Console.ReadLine(), out convertedWidth);
          Console.WriteLine("Enter board height. Only even numbers starting from 4 -->  ");
          heightConvertedSuccessfully = int.TryParse(Console.ReadLine(), out convertedHeight);
          if (!widthConvertedSuccessfully || !heightConvertedSuccessfully)
          {
               Console.WriteLine("Enter board size correctly. Enter even numbers form 4\n");
               continue;
          }

          if (convertedHeight % 2 == 0 && convertedWidth % 2 == 0)
          {
               numbersEven = true;
          }
          else
          {
               Console.WriteLine("Enter board size correctly. Enter even numbers form 4\n");
          }

     } while (!widthConvertedSuccessfully || !heightConvertedSuccessfully || !numbersEven);

     gameOptions.Height = convertedHeight;
     gameOptions.Width = convertedWidth;

     Console.WriteLine("Is taking mandatory? \nPress Y to say yes and N to say no");
     ConsoleKey key = Console.ReadKey(true).Key;
     gameOptions.TakingIsMandatory = key switch
     {
          Y => true,
          N => false,
          _ => gameOptions.TakingIsMandatory
     };

     Console.WriteLine("Do white checkers start the game? \nPress Y to say yes and N to say no");
     key = Console.ReadKey(true).Key;
     gameOptions.WhiteStarts = key switch
     {
          Y => true,
          N => false,
          _ => gameOptions.WhiteStarts
     };
}


// Function used for making menu system wait. Screen will remain the same until Enter key is pressed
void WaitForUserInput()
{
     Console.WriteLine("\n\nPress Enter to go back to menu");
     ConsoleKey userKey;
     do
     {
          userKey = Console.ReadKey(true).Key;
          
     } while (userKey != Enter);
}

string PrintCurrentGameOptions()
{
     Console.Clear();
     Console.WriteLine();
     Console.WriteLine(gameOptions);
     WaitForUserInput();
     return "B";
}

void PrintOutAllSavedGameOptions()
{
     Console.Clear();
     var optionsList = optionRepo.GetGameOptionsList();
     var i = 1;
     foreach (var option in optionsList)
     {
          Console.WriteLine("\n"+i + ") " + option);
          Console.WriteLine(optionRepo.GetGameOptions(option));
          i++;
     }
}


string EasterEggMethod()
{

     Console.Clear();

     Console.WriteLine("\n - Hello there!");
     Console.WriteLine(" - General Kenobi!");
     Console.WriteLine(@"     
          _ 💀_
         / _|_ \
        /  /|\  \
      🧪  / | \  🔪
         🖍_|_ 🌂
          /    \
         /      \
         \      /
         _\    /_
         ");
     Console.WriteLine("To exit this menu press Enter");
     
     //Wait until user presses Enter key. If pressed exit this page
     WaitForUserInput();
     return "B";
}

