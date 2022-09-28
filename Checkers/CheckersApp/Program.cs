
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

//var choice = mainMenu.RunMenu();

Console.BackgroundColor
     = ConsoleColor.White;

Console.ForegroundColor
     = ConsoleColor.Black;
Console.WriteLine(" ◉ ", Console.BackgroundColor, Console.ForegroundColor);

Console.ForegroundColor
     = ConsoleColor.Black;

Console.BackgroundColor
     = ConsoleColor.White;
Console.WriteLine(" ◎ ", Console.BackgroundColor, Console.BackgroundColor);

Console.BackgroundColor
     = ConsoleColor.Black;

Console.WriteLine(" ◉ ", Console.BackgroundColor, Console.ForegroundColor);
Console.WriteLine(" ◎ ", Console.BackgroundColor);


string DoNewGame()
{
     Console.WriteLine("New game method");
     return "---";
}







