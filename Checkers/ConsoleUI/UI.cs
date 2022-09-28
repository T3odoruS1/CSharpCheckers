using GameBrain;

namespace ConsoleUI;

public static class UI
{

    public static void DrawGameBoard(EGameSquareState?[,] board)
    {
        var width = board.GetLength(0);
        var height = board.GetLength(1);
        
        Console.Write(" ");
        for (var i = 0; i <= width; i++)
        {
            var coordLetters = GetCharacterForIndex(i);
            if (coordLetters.Length < 2 && !coordLetters.Equals("Z"))
            {
                Console.Write(" "+ GetCharacterForIndex(i) + " ");
            }
            else
            { 
                Console.Write(" "+ GetCharacterForIndex(i) + "");
            }
        }

        Console.WriteLine();
        for (var i = 0; i < height; i++)
        {
            
            // Giving each line a number. Calculating how many whitespaces to put after the number
            if (height - i < 100 && height - i >= 10)
            {
                Console.Write(height - i + "  ");
            }
            else if (height - i >= 100)
            {
                Console.Write(height - i + " ");
            }
            else if (height - i < 10) 
            {
                Console.Write(height - i + "   ");
            }
            
            
            // Iterating through the columns
            for (var j = 0; j < width; j++)
            {

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
                        Console.BackgroundColor = ConsoleColor.Black;
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
            }

            Console.WriteLine();
        }
    }

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
    
}