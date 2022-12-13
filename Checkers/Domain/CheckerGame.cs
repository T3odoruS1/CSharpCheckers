using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

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

    public int OptionsId { get; set; }
    public CheckerGameOptions? GameOptions { get; set; } = default!;
    
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

    public override bool Equals(object? obj)
    {
        
        if (GetType() != obj!.GetType())
        {
            return false;
        }

        
        var other = (CheckerGame)obj;
        return Equals(other);
    }

    private bool Equals(CheckerGame other)
    {
        return Id == other.Id &&
               Name == other.Name &&
               StarterAt.Equals(other.StarterAt) &&
               Nullable.Equals(GameOverAt, other.GameOverAt) &&
               GameWonBy == other.GameWonBy &&
               Player1Name == other.Player1Name &&
               Player1Type == other.Player1Type &&
               Player2Name == other.Player2Name &&
               Player2Type == other.Player2Type;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(Name);
        hashCode.Add(StarterAt);
        hashCode.Add(GameOverAt);
        hashCode.Add(GameWonBy);
        hashCode.Add(Player1Name);
        hashCode.Add((int)Player1Type);
        hashCode.Add(Player2Name);
        hashCode.Add((int)Player2Type);
        hashCode.Add(OptionsId);
        hashCode.Add(GameOptions);
        hashCode.Add(CheckerGameStates);
        return hashCode.ToHashCode();
    }
}