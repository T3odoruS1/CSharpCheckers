
using ConsoleUI;
using DAL.FileSystem;
using Domain;
using GameBrain;
using MenuSystem;
using static System.ConsoleKey;


var gameOptions = new GameOptions();
var repo = new GameOptionsRepositoryFileSystem();

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
     new MenuItem("C", "Create options", CreateGameOptions),

     new MenuItem("O", "List saved options", ListGameOptions),
     new MenuItem("L", "Load options", LoadGameOptions),
     new MenuItem("D", "Delete options", DeleteOptions),
     new MenuItem("S", "Save current options", SaveCurrentOptions),
     new MenuItem("E", "Edit current options", null),
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



string DoNewGame()
{
     Console.Clear();
     Console.WriteLine("\nNew game! Time to play!");
     
     var game = new CheckersBrain(gameOptions);
     UI.DrawGameBoard(game.GetBoard());


     return "X";
}

string DeleteOptions()
{
     Console.Clear();
     Console.WriteLine("List of game options:\n\n");
     Dictionary<int, string> optionsMap = PrintOutAllSavedGameOptionsAndGetDictionary();
     
     
     Console.WriteLine("\nChoose a number of an option you would like to delete.");
          var userChoice = Console.ReadLine();
          if (optionsMap.ContainsKey(Int32.Parse(userChoice!)))
          {
               repo.DeleteGameOptions(optionsMap[Int32.Parse(userChoice!)]);
               Console.WriteLine("Option deleted");
          }
          else
          {
               Console.WriteLine("No such option found");
          }
          WaitForUserInput();
          return "B";
}

string LoadNewGame()
{
     Console.Clear();
     
     Console.WriteLine("\nLoad game method not implemented yet.");
     return "X";
}

string SaveCurrentOptions()
{
     
     Console.Clear();
     Console.WriteLine("Current game option:");
     Console.WriteLine(gameOptions);

     Console.CursorVisible = true;
     Console.WriteLine("How would you like to name this game option?");
     var userInputForFileName = Console.ReadLine();
     
     repo.SaveGameOptions(userInputForFileName!, gameOptions);
     Console.CursorVisible = false;
     Console.WriteLine("Game options saved!");
     WaitForUserInput();
     return "B";
}

string ListGameOptions()
{

     Console.Clear();
     Console.WriteLine("List of game options:\n\n");
     PrintOutAllSavedGameOptions();
     
     WaitForUserInput();
     return "B";
}

string LoadGameOptions()
{
     
     
     
     Console.Clear();
     Console.WriteLine("Load game options:");
     Dictionary<int, string> optionsMap = PrintOutAllSavedGameOptionsAndGetDictionary();

     Console.Write("Choose game option you would like to load -->  ");
     var userChoice = Console.ReadLine();


     if (optionsMap.ContainsKey(Int32.Parse(userChoice!)))
     {
          gameOptions = repo.GetGameOptions(optionsMap[Int32.Parse(userChoice!)]);
          Console.WriteLine("\n\n\nLoaded: " + repo.GetGameOptions(optionsMap[Int32.Parse(userChoice!)]));
     }
     else
     {
          Console.WriteLine("No such game option found");
     }
     WaitForUserInput();
     return "B"; 
}

string CreateGameOptions()
{
     Console.Clear();
     ValidateUserGameOptions();
     Console.WriteLine("Alright this is your new game options:");
     Console.WriteLine(gameOptions);
     WaitForUserInput();

     return "B";
}

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

void WaitForUserInput()
{
     Console.WriteLine("\n\nPress Enter to go back to menu");
     ConsoleKey userKey;
     do
     {
          userKey = Console.ReadKey(true).Key;
          
     } while (userKey != Enter);
}

void PrintOutAllSavedGameOptions()
{
     var optionsList = repo.GetGameOptionsList();
     var i = 1;
     foreach (var option in optionsList)
     {
          Console.WriteLine(i + ") " + option);
          Console.WriteLine(repo.GetGameOptions(option));
          i++;
     }
}

Dictionary<int, string> PrintOutAllSavedGameOptionsAndGetDictionary()
{
     Dictionary<int, string> optionsMap = new Dictionary<int, string>();
     var optionsList = repo.GetGameOptionsList();
     var i = 1;
     foreach (var option in optionsList)
     {
          Console.WriteLine(i + ") Option name:" + option);
          Console.WriteLine("Option properties: " + repo.GetGameOptions(option));
          Console.WriteLine();
          optionsMap.Add(i, option);
          i++;

     }

     return optionsMap;

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

