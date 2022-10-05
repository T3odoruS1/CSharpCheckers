
using ConsoleUI;
using GameBrain;
using MenuSystem;

var thirdMenu = new Menu(EMenuLevel.Other,
     ">  Checkers third level  <",
     new List<MenuItem>()
     {
          new MenuItem("N", "Nothing", NothingMethod),
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
     Console.WriteLine("\nNew game method");
     var game = new CheckersBrain(8, 8);
     UI.DrawGameBoard(game.GetBoard());
     
     return "X";
}

string LoadNewGame()
{
     Console.Clear();
     
     Console.WriteLine("\nLoad game method");
     return "X";
}

string NothingMethod()
{

     Console.Clear();

     Console.WriteLine("\nNothing method");
     return "X";
}








