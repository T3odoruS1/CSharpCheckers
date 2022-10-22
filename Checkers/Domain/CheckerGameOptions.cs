namespace Domain;

public class GameOptions
{
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool TakingIsMandatory { get; set; }
    public bool WhiteStarts { get; set; } = true;

    public ICollection<CheckerGame>? CheckerGames { get; set; }

    public override string ToString()
    {
        return $"Board width: {Width}, Board height: {Height}, Taking is mandatory: {TakingIsMandatory}, White starts: {WhiteStarts}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not GameOptions)
        {
            return false;
        }

        var comparable = (GameOptions)obj;
        if (comparable.Width == this.Width &&
            comparable.Height == this.Height &&
            comparable.TakingIsMandatory == this.TakingIsMandatory &&
            comparable.WhiteStarts == this.WhiteStarts)
        {
            return true;
        }

        return false;

    }
}