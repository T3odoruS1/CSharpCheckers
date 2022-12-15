namespace GameBrain;

public class Move
{
    public int x;
    public int y;
    public int destX;
    public int destY;

    public override string ToString()
    {
        return $"{x}, {y}, {destX}, {destY}";
    }
}