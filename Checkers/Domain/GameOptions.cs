namespace Domain;

public class GameOptions
{
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool TakingIsMandatory { get; set; }
    public bool WhiteStarts { get; set; } = true;

    public override string ToString()
    {
        return $"Board width: {Width}, Board height: {Height}, Taking is mandatory: {TakingIsMandatory}, White starts: {WhiteStarts}";
    }
}