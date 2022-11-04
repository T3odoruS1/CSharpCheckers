using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class CheckerGame
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime StarterAt { get; set; } = DateTime.Now;
    public DateTime? GameOverAt { get; set; }

    public string? GameWonBy { get; set; }

    [MaxLength(128)]
    public string Player1Name { get; set; } = default!;
    public EPlayerType Player1Type { get; set; }
    
    [MaxLength(128)]
    public string Player2Name { get; set; } = default!;

    public EPlayerType Player2Type { get; set; }

    public CheckerGameOptions? GameOptions { get; set; }
    public int OptionsId { get; set; }
    
    public ICollection<CheckerGameState>? CheckerGameStates { get; set; }


    public override string ToString()
    {
        return "CheckerGame:\n" +
               $"Id:{Id}\n" +
               $"Name:{Name}\n" +
               $"StartedAt: {StarterAt}\n" +
               $"P1 name, type: {Player1Name}, {Player1Type}\n" +
               $"P2 name, type: {Player2Name}, {Player2Type}\n" +
               $"Game Options: {GameOptions}\n";
    }
}