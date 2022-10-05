
using ConsoleUI;
using GameBrain;
using MenuSystem;

var thirdMenu = new Menu(EMenuLevel.Other,
     ">  Checkers third level  <",
     new List<MenuItem>()
     {
          new MenuItem("N", "Easter egg", EasterEggMethod),
     });
var secondMenu = new Menu(EMenuLevel.Second,
     ">  Checkers second level  <",
     new List<MenuItem>()
{
     new MenuItem("T", "Third level", thirdMenu.RunMenu),
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

     var widthConvertedSuccessfully = false;
     var heightConvertedSuccessfully = false;
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
     
     
     var game = new CheckersBrain(convertedWidth, convertedHeight);
     UI.DrawGameBoard(game.GetBoard());


     return "X";
}

string LoadNewGame()
{
     Console.Clear();
     
     Console.WriteLine("\nLoad game method not implemented yet.");
     return "X";
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
     return "X";
}








