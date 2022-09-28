
using ConsoleUI;
using GameBrain;
using MenuSystem;


var thirdMenu = new Menu(EMenuLevel.Other,
     ">  Checkers third level  <",
     new List<MenuItem>()
     {
          new MenuItem("N", "Nothing", null),
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
          new MenuItem("L", "Load Game", null),
          new MenuItem("O", "Options", secondMenu.RunMenu)
     });


// UI can draw the board upto 1000 cells high and 676 cells in width.
// I doubt that we we'll need more :)
// Numbers must be even
var game = new CheckersBrain(16, 16);
UI.DrawGameBoard(game.GetBoard());


string DoNewGame()
{
     Console.WriteLine("New game method");
     return "---";
}







