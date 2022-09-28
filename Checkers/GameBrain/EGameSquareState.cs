namespace GameBrain;

public enum EGameSquareState

// According to the rules of the checker both player's checkers start on the same color and cannot move into different
// color square. Therefore we need only empty cell and cell with each of the checkers. Black cell as playable f
{
    Unplayable, // This is while. We don't play on them
    Empty, // Black cell with no checkers
    White, // Black cell with white checker
    Black, // Black cell with black checker
    WhiteKing, // Black cell with white king checker
    BlackKing //  Black cell with black king checker
}