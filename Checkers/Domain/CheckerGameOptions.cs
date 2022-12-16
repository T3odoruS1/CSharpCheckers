using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CheckerGameOptions
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool TakingIsMandatory { get; set; }
    public bool WhiteStarts { get; set; } = true;

    public ICollection<CheckerGame>? CheckerGames { get; set; }

    public int GameCount { get; set; } = 0;

    public override string ToString()
    {
        return $"Options name: {Name}, \n" +
               $"Board width: {Width}, \n" +
               $"Board height: {Height}, \n" +
               $"Taking is mandatory: {TakingIsMandatory}, \n" +
               $"White starts: {WhiteStarts} \n" +
               $"Amount of games: {GameCount}";
    }

    public override bool Equals(object? obj)
    {
        if (GetType() != obj!.GetType())
        {
            return false;
        }

        var other = (CheckerGameOptions)obj;
        return Equals(other);
    }

    private bool Equals(CheckerGameOptions other)
    {
        return Id == other.Id &&
               Name == other.Name &&
               Width == other.Width &&
               Height == other.Height &&
               TakingIsMandatory == other.TakingIsMandatory &&
               WhiteStarts == other.WhiteStarts &&
               Nullable.Equals(CheckerGames, other.CheckerGames) &&
               GameCount == other.GameCount;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Width, Height, TakingIsMandatory, WhiteStarts, CheckerGames, GameCount);
    }
}