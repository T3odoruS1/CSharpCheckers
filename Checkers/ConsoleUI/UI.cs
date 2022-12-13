using GameBrain;

namespace ConsoleUI;

public static class UI
{
    
    
    /// <summary>
    /// Method is used to draw a game board and hilight a cell if necessary.
    /// </summary>
    /// <param name="board">Game board in form of 2d array with enums</param>
    /// <param name="activeX">Optional parameter used for hilighting a cell</param>
    /// <param name="activeY">Optional parameter used for hilighting a cell</param>
    public static void DrawGameBoard(EGameSquareState[,] board, int? activeX, int? activeY)
    {
        var width = board.GetLength(0);
        var height = board.GetLength(1);
        
        Console.Write("    ");
        for (var i = 0; i <= width; i++)
        {
            
            // Placing the index letters on top of the board
            if (i != height)
            {
                Console.Write(" " + (i + 1)  + " ");
 
            }
        }

        Console.WriteLine();
        for (var i = 0; i < height; i++)
        {
            
            // Giving each line a number. Calculating how many whitespaces to put after the number
            Console.Write(GetRowIndexWithWhitespacesForIndex(i, height));

            // Iterating through the columns
            for (var j = 0; j < width; j++)
            {

                if (j == activeX && i == activeY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                }
                // Switch expression to decide what to print in given cell
                switch (board[j, i])
                {
                    
                    case EGameSquareState.Black :
                        Console.Write(" ◎ ");
                        break;
                    case EGameSquareState.White:
                        Console.Write(" ◉ ");
                        break;
                    case EGameSquareState.Empty:
                        Console.Write("   ");
                        break;
                    case EGameSquareState.Unplayable:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("   ");
                        Console.ResetColor();
                        break;
                    case EGameSquareState.BlackKing:
                        Console.Write("=◎=");
                        break;
                    case EGameSquareState.WhiteKing:
                        Console.Write("=◉=");
                        break;
                }
                Console.ResetColor();

            }
    
            Console.WriteLine();
        }
    }

    /// <summary>
    /// String that represents an index. (Like columns in excel) 
    /// </summary>
    /// <param name="index">index to be transformed</param>
    /// <returns>String that represents the index</returns>
    
    // ReSharper disable once UnusedMember.Local
    private static string GetCharacterForIndex(int index)
    {
        
        var columnName = "";
    
        while (index > 0)
        {
            var modulo = (index- 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            index = (index- modulo) / 26;
        }
    
        return columnName;
    }

    /// <summary>
    /// Return cell index as string with whitespaces. Used for bigger boards. If not used board will be ugly. Using this
    /// the top left corner will have the biggest number
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="height">height of the board</param>
    /// <returns>index as string</returns>
    private static string GetRowIndexWithWhitespacesForIndex(int i, int height)
    {
        return (height - i) switch
        {
            < 100 and >= 10 => ((i + 1) + "  "),
            >= 100 => ((i + 1) + " "),
            < 10 => ((i + 1) + "   ")
        };
    }
    
}