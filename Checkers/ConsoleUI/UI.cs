using GameBrain;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ConsoleUI;

public static class UI
{
    
    // Main function to draw the board
    
    public static void DrawGameBoard(EGameSquareState[,] board, int? activeX, int? activeY)
    {
        var width = board.GetLength(0);
        var height = board.GetLength(1);
        
        Console.Write("  ");
        for (var i = 0; i <= width; i++)
        {
            
            // Placing the index letters on top of the board
            Console.Write(GetStringWithWhitespacesForIndex(i));
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
                    default:
                        throw new ArgumentException("Invalid choice for square choice: " + board[j, i]);
                }
                // Console.ResetColor();

            }
    
            Console.WriteLine();
        }
    }

    // Get letters for columns
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

    
    // Calculate how name whitespaces to use for letter placing above the board.
    private static string GetStringWithWhitespacesForIndex(int index)
    {
        var coordLetters = GetCharacterForIndex(index);
        if (coordLetters.Length < 2 && !coordLetters.Equals("Z"))
        {
            return(" "+ GetCharacterForIndex(index) + " ");
        }
        return(" "+ GetCharacterForIndex(index) + "");
    }

    
    // Calculating how many whitespaces to put after the row index for the board to be even.
    private static string GetRowIndexWithWhitespacesForIndex(int i, int height)
    {
        return (height - i) switch
        {
            < 100 and >= 10 => (height - i + "  "),
            >= 100 => (height - i + " "),
            < 10 => (height - i + "   ")
        };
    }
    
}