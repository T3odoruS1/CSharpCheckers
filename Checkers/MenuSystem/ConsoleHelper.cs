namespace MenuSystem;

    public static class ConsoleHelper
{
    public static string MultipleChoice(bool canCancel, string title, params string[] options)
    {
        const int startX = 5;
        const int startY = 5;
        const int optionsPerLine = 2;
        const int spacingPerLine = 33;

        var currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();
            Console.WriteLine(title + "\n\n\n\n");


            for (int i = 0; i < options.Length; i++)
            {
                
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if(i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                Console.Write(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                {
                    if (currentSelection % optionsPerLine > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.RightArrow:
                {
                    if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (currentSelection >= optionsPerLine)
                        currentSelection -= optionsPerLine;
                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    if (currentSelection + optionsPerLine < options.Length)
                        currentSelection += optionsPerLine;
                    break;
                }
                case ConsoleKey.Escape:
                {
                    if (canCancel)
                        return "X";
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return options[currentSelection];
    }
}
