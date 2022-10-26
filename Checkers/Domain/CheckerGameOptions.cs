using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
    
}