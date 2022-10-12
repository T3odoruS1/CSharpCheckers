
using ConsoleUI;
using DAL.FileSystem;
using Domain;
using GameBrain;
using MenuSystem;
using static System.ConsoleKey;

var gameOptions = new GameOptions();
var repositoryFileSystem = new GameOptionsRepositoryFileSystem();
var stateRepositoryFileSystem = new GameStateRepositoryFileSystem();
var game = new CheckersBrain(gameOptions);


var thirdMenu = new Menu(EMenuLevel.Other,
     ">  Checkers third level  <",
     new List<MenuItem>()
     {
          new MenuItem("N", "Easter egg", EasterEggMethod),
     });
var secondMenu = new Menu(EMenuLevel.Second,
     ">  Checkers options  <",
     new List<MenuItem>()
{
     new MenuItem("N", "Current Game Options", PrintCurrentGameOptions),
     new MenuItem("C", "Create options", CreateGameOptions),
     new MenuItem("O", "List saved options", ListGameOptions),
     new MenuItem("L", "Load options", LoadGameOptions),
     new MenuItem("D", "Delete options", DeleteOptions),
     new MenuItem("S", "Save current options", SaveCurrentOptions),
     new MenuItem("T", "Something to be found here ;)", thirdMenu.RunMenu)

});

var mainMenu = new Menu(EMenuLevel.Main,
     ">  Checkers  <",
     new List<MenuItem>()
     {
          new MenuItem("N", "New Game", DoNewGame),
          new MenuItem("L", "Load Game", LoadNewGame),
          new MenuItem("O", "Options", secondMenu.RunMenu)
     });


mainMenu.RunMenu();
// UI can draw the board upto 1000 cells high and 676 cells in width.
// I doubt that we we'll need more :)
// Numbers must be even



// Print out all game options and load one that user chooses.
string LoadNewGame()
{
     Console.Clear();
     
     Console.WriteLine("Load game: ");
     var boards = stateRepositoryFileSystem.GetGameStatesList();
     int i = 1;
     Dictionary<int, string> boardMap = new Dictionary<int, string>();
     foreach (var board in boards)
     {
          Console.WriteLine(i+")" + board);
          boardMap.Add(i, board);
          UI.DrawGameBoard(stateRepositoryFileSystem.GetGameState(board));
          i++;
     }

     Console.WriteLine("Choose one of those");
     var choice = Console.ReadLine();
     var isNum = Int32.TryParse(choice, out var integerChoice);
     if (!isNum || !boardMap.ContainsKey(integerChoice))
     {
          LoadNewGame();
     }
     else
     {
          game.SetGameBoard(stateRepositoryFileSystem.GetGameState(boardMap[Int32.Parse(choice!)]));
          UI.DrawGameBoard(game.GetBoard());
          game = new CheckersBrain(gameOptions);
          WaitForUserInput();
     }

     return "B";
}


// Make new game ond use UI method to print the board. Some test code commented out
string DoNewGame()
{
     Console.Clear();
     Console.WriteLine("\nNew game! Time to play!");
     game = new CheckersBrain(gameOptions);
     UI.DrawGameBoard(game.GetBoard());
     WaitForUserInput();


     #region Use this code for changing checker positions. For testing saving functionaity

     // // For testing game saving functionality
     // game.ChangeCheckerPos(0,0,6,5);
     // //for testing game saving functionality
     // UI.DrawGameBoard(game.GetBoard());
     // Console.WriteLine("filename");
     // var fileName = Console.ReadLine();
     // stateRepositoryFileSystem.SaveGameState(fileName!, game.GetBoard());

     #endregion
     
     WaitForUserInput();
     return "B";
}


// Run delete options submenu. Delete one of them or exit this menu
string DeleteOptions()
{
     var optionToDelete = RunSubmenu();
     if (optionToDelete == "B" ||  optionToDelete == "M" || optionToDelete == "X") return optionToDelete;
     repositoryFileSystem.DeleteGameOptions(optionToDelete);
     Console.Clear();
     Console.WriteLine("Deleted");
     WaitForUserInput();
     return "B";
}


// Save current options into json file
string SaveCurrentOptions()
{
     
     Console.Clear();
     Console.WriteLine("Current game option:");
     Console.WriteLine(gameOptions);

     Console.CursorVisible = true;
     Console.WriteLine("How would you like to name this game option?");
     var userInputForFileName = Console.ReadLine();
     
     repositoryFileSystem.SaveGameOptions(userInputForFileName!, gameOptions);
     Console.CursorVisible = false;
     Console.WriteLine("Game options saved!");
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
     if (optionToLoad == "B" ||  optionToLoad == "M" || optionToLoad == "X") return optionToLoad;
     gameOptions = repositoryFileSystem.GetGameOptions(optionToLoad);
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
     foreach (var gameOption in repositoryFileSystem.GetGameOptionsList())
     {
          menuItems.Add(new MenuItem(i.ToString(), 
               gameOption + ":\t" + repositoryFileSystem.GetGameOptions(gameOption), 
               null));
          listOfOptions.Add(gameOption);
          i++;
     }
     var loadMenu = new Menu(EMenuLevel.Other,
          ">  Checkers  <", menuItems);
     
     var userChoice = loadMenu.RunMenu();

     
     // If user choice is number then return user choice return one of the options that he has chose.
     // Else return  user choice
     return !Int32.TryParse(userChoice, out var j) ? userChoice : listOfOptions[j];
}


// Making new game option
string CreateGameOptions()
{
     Console.Clear();
     ValidateUserGameOptions();
     Console.WriteLine("Alright this is your new game options:");
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
     var optionsList = repositoryFileSystem.GetGameOptionsList();
     var i = 1;
     foreach (var option in optionsList)
     {
          Console.WriteLine(i + ") " + option);
          Console.WriteLine(repositoryFileSystem.GetGameOptions(option));
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

