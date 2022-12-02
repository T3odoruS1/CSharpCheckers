using System.Runtime.InteropServices;
using DataAccessLayer;
using Domain;
using GameBrain;

namespace CheckersApp;

public class GameRunner
{
    private IGameGameRepository _repo = default!;
    private CheckerGame _game = default!;
    private CheckersBrain _brain = default!;

    public void RunGame(IGameGameRepository repo, int id, CheckersBrain brain)
    {
        
        _repo = repo;
        _game = _repo.GetGameById(id);
        _brain = brain;

        var options = _game.GameOptions;
        if (_game.CheckerGameStates == null || options == null)
        {
            throw new ArgumentException("The game that was recieved with given id does not have options or states.");
        }

        var state = _game.CheckerGameStates.Last();
        
        
        ConsoleKey key;
        var exit = false;
        do
        {
            Console.Clear();
            if (state.NextMoveByBlack)
            {
                Console.WriteLine("Next move by black");
            }else if (!state.NextMoveByBlack)
            {
                Console.WriteLine("Next move by white");
            }
            

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.X)
            {
                exit = true;
            }
        } while (!exit);
    }
    
    
    // Implement switch player switch chair system for the console app.
    
    
}