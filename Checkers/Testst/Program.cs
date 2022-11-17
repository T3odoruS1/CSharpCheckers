// See https://aka.ms/new-console-template for more information

using ConsoleUI;
using Domain;
using GameBrain;

var options = new CheckerGameOptions();
var brain = new CheckersBrain(options);

UI.DrawGameBoard(brain.GetBoard(), 1, 2);
Console.WriteLine("Move is possible");
Console.WriteLine(brain.MoveIsPossible(1, 2, 2, 3));