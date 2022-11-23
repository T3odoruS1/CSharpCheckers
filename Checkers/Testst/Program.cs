// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ConsoleUI;
using DAL.Db;
using DAL.FileSystem;
using DataAccessLayer;
using GameBrain;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;

var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=/Users/edgarvildt/Developer/CheckerDb/checker.db").Options;
var ctx = new AppDbContext(dbOptions);

IGameGameRepository gameRepoDb = new GameRepositoryDatabase(ctx);



var Game = gameRepoDb.GetGameById(29);
var options = Game.GameOptions;
var brain = new CheckersBrain(options);


// Get brain and state
var Brain = new CheckersBrain(options);
var GameState = Game.CheckerGameStates.Last();


// Unserialize json game board. Get game board.
var jsonString = GameState.SerializedGameBoard;
var jagged = JsonSerializer.Deserialize<EGameSquareState[][]>(jsonString);

var Board = FsHelpers.JaggedTo2D(jagged!);
Brain.SetGameBoard(Board, GameState);
    
brain.SetGameBoard(Board, GameState);

for (int y = 0; y < Board.GetLength(1); y++)
{
    for (int x = 0; x < Board.GetLength(0); x++)
    {
        Console.WriteLine(Brain.MoveIsPossible(5, 0 , x, y));
    }
}


UI.DrawGameBoard(brain.GetBoard(), 1, 2);
Console.WriteLine("Move is possible");
Console.WriteLine(brain.MoveIsPossible(1, 2, 2, 3));