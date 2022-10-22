using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CheckerGame
{
    public int Id { get; set; }

    public DateTime StarterAt { get; set; }
    public DateTime? GameOverAt { get; set; }
    public string? GameWonBy { get; set; }

    [MaxLength(128)]
    public string Player1Name { get; set; } = default!;
    public EPlayerType Player1 { get; set; }
    
    [MaxLength(128)]
    public string Player2Name { get; set; } = default!;

    public EPlayerType Player2Type { get; set; }

    public GameOptions? GameOptions { get; set; }
    public int OptionsId { get; set; }

    public ICollection<GameState>? CheckerGameState { get; set; }
    
}