namespace Domain;

public class GameOptions
{
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool TakingIsMandatory { get; set; } = false;
    public bool WhiteStarts { get; set; } = true;

}