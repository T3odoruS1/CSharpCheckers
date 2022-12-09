// // See https://aka.ms/new-console-template for more information
//
// using System.Text.Json;
// using ConsoleUI;
// using DAL.Db;
// using DAL.FileSystem;
// using DataAccessLayer;
// using GameBrain;
// using Microsoft.CodeAnalysis.Text;
// using Microsoft.EntityFrameworkCore;
//
// var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
//     .UseSqlite("Data Source=/Users/edgarvildt/Developer/CheckerDb/checker.db").Options;
// var ctx = new AppDbContext(dbOptions);
//
// IGameRepository gameRepoDb = new GameRepositoryDatabase(ctx);
//
//
//
// var Game = gameRepoDb.GetGameById(29);
// var options = Game.GameOptions;
// var brain = new CheckersBrain(options);
//
//
// // Get brain and state
// var Brain = new CheckersBrain(options);
// var GameState = Game.CheckerGameStates.Last();
//
//
// // Unserialize json game board. Get game board.
// var jsonString = GameState.SerializedGameBoard;
// var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);
//
// var Board = FsHelpers.JaggedTo2D(jagged!);
// Brain.SetGameBoard(Board, GameState);
//     
// brain.SetGameBoard(Board, GameState);
//
// for (int y = 0; y < Board.GetLength(1); y++)
// {
//     for (int x = 0; x < Board.GetLength(0); x++)
//     {
//         Console.WriteLine(Brain.MoveIsPossible(5, 0 , x, y));
//     }
// }
//
//
// UI.DrawGameBoard(brain.GetBoard(), 1, 2);
// Console.WriteLine(brain.MoveIsPossible(1, 2, 2, 3));
// Console.WriteLine("Move is possible");a










Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

List<int> list = new List<int>();
list.Add(12);
list.Add(13);
list.Add(14);
list.Add(62);


map.Add(1999, list);
map.Add(20002, list);
//
// foreach(KeyValuePair<int, List<int>> element in map){
//     Console.WriteLine("Key is " + element.Key);
//     Console.WriteLine("Value is " + element.Value.ToString());
//     
// }
//
//

Dictionary<string, int> month_accident = new Dictionary<string, int>();

month_accident.Add("Sep", 20);
month_accident.Add("Feb", 33);

var sortedDict = from entry in month_accident orderby entry.Value descending select entry;

foreach (KeyValuePair<string, int> element in sortedDict)
{
    Console.WriteLine(element.Key);
}








