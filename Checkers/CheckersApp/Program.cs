
using ConsoleUI;
using DAL.FileSystem;
using Domain;
using GameBrain;
using MenuSystem;
using static System.ConsoleKey;

var gameOptions = new GameOptions();
var repo = new GameRepositoryFileSystem();
CheckersBrain game = new CheckersBrain(gameOptions);
gameOptions.Name = "Default";



#region MenuInitialization



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
          new MenuItem("L", "Load Game", LoadGame),
          new MenuItem("D", "Delete SavedGame", DeleteSavedGame),

          new MenuItem("O", "Options", secondMenu.RunMenu)
     });

#endregion




#region Load last used game option. And save current options on exit

try
{
     gameOptions = repo.GetGameOptions(repo.LastUsedToken);

}
catch (Exception)
{
     gameOptions = new GameOptions();
}

mainMenu.RunMenu();

repo.SaveGameOptions(repo.LastUsedToken, gameOptions);

#endregion


// UI can draw the board upto 1000 cells high and 676 cells in width.
// I doubt that we we'll need more :)
// Numbers must be even



// Print out all game options and load one that user chooses.


string DeleteSavedGame(){
     var allSavedGames = repo.GetGameBoardNames();
     var i = 1;
     var gameDict = new Dictionary<int, string>();
     foreach (var savedGame in allSavedGames)
     {
          var board = repo.GetGameBoard(savedGame);
          Console.WriteLine($"{i}) - {savedGame}");
          Console.WriteLine($"Options : {board.Values.First()}");
          UI.DrawGameBoard(board.Keys.First());
          gameDict.Add(i, savedGame);
          i++;
     }

     Console.WriteLine("Choose a game you want to delete. If you dont want to delete a game choose 'X'");
     var userChoice = Console.ReadLine();
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
          repo.DeleteGameState(gameDict[a]);
     }
     return "B";
}
string LoadGame()
{
     var allSavedGames = repo.GetGameBoardNames();
     var i = 1;
     var gameDict = new Dictionary<int, string>();
     foreach (var savedGame in allSavedGames)
     {
          var board = repo.GetGameBoard(savedGame);
          Console.WriteLine($"{i}) - {savedGame}");
          Console.WriteLine($"Options : {board.Values.First()}");
          UI.DrawGameBoard(board.Keys.First());
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
               var gameToBeLoaded = repo.GetGameBoard(gameDict[a]);
               gameOptions = gameToBeLoaded.Values.First();
               
               UI.DrawGameBoard(gameToBeLoaded.Keys.First());
               
               // Not implemented yet
               game.PlayGame();
          }

     }
     WaitForUserInput();
     return "B";
}


// Make new game ond use UI method to print the board. Some test code commented out
string DoNewGame()
{
     Console.Clear();
     Console.WriteLine("\nNew game! Time to play!\n");
     Console.WriteLine("You want to load custom options?");
     game = new CheckersBrain(gameOptions);
     UI.DrawGameBoard(game.GetBoard());
     // Gameplay will be implemented here.
     game.PlayGame();


     OfferSaving();
     WaitForUserInput();


     return "B";
}


void OfferSaving()
{
     Console.WriteLine("Do you want to save current game? Press enter to save and any other button to deny.");
     var key = Console.ReadKey(true).Key;
     if (key == Enter)
     {

          var allSavedGames = repo.GetGameBoardNames();
          
          Console.WriteLine("Please choose a name for current game");
          var userChoice = Console.ReadLine();
          if (!allSavedGames.Contains(userChoice!))
          {
               gameOptions.Name = userChoice!;
               repo.SaveGameState(userChoice!, game.GetBoard(), gameOptions);
          }
          else
          {
               var prevChoice = userChoice;
               Console.WriteLine("That game name is already taken. Chose another one or press enter to rewrite that game.");
               userChoice = Console.ReadLine();
               gameOptions.Name = prevChoice!;
               repo.SaveGameState(userChoice == "" ? prevChoice! : userChoice!, game.GetBoard(), gameOptions);
          }
               
     }
}

// Run delete options submenu. Delete one of them or exit this menu
string DeleteOptions()
{
     var optionToDelete = RunSubmenu();
     if (optionToDelete == "B" ||  optionToDelete == "M" || optionToDelete == "X") return optionToDelete;
     repo.DeleteGameOptions(optionToDelete);
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
     gameOptions.Name = userInputForFileName!;
     
     repo.SaveGameOptions(userInputForFileName!, gameOptions);
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
     if (optionToLoad is "B" or "M" or "X") return optionToLoad;
     gameOptions = repo.GetGameOptions(optionToLoad);
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
     foreach (var gameOption in repo.GetGameOptionsList())
     {
          if(gameOption.Contains(repo.SavedGameOptionsFlag) || gameOption.Equals(repo.LastUsedToken)) continue;
          menuItems.Add(new MenuItem(i.ToString(), 
               gameOption + ":\t" + repo.GetGameOptions(gameOption), 
               null));
          listOfOptions.Add(gameOption);
          i++;
     }
     var loadMenu = new Menu(EMenuLevel.Other,
          ">  Checkers  <", menuItems);
     
     var userChoice = loadMenu.RunMenu();

     
     // If user choice is number then return user choice return one of the options that he has chose.
     // Else return  user choice
     return !int.TryParse(userChoice, out var j) ? userChoice : listOfOptions[j];
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
     var optionsList = repo.GetGameOptionsList();
     var i = 1;
     foreach (var option in optionsList)
     {
          if (option.Contains(repo.SavedGameOptionsFlag) || option.Equals(repo.LastUsedToken)) continue;
          Console.WriteLine(i + ") " + option);
          Console.WriteLine(repo.GetGameOptions(option));
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

