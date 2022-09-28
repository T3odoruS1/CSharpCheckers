namespace MenuSystem;

public class Menu
{
    private const string ShortcutExit = "X";
    private const string ShortcutGoBack = "B";
    private const string ShortcutGoMain = "M";

    private string Title { get; set; }
    private readonly EMenuLevel _level;

    private readonly Dictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();
    private readonly MenuItem _menuItemExit = new MenuItem(ShortcutExit, "Exit", null);
    private readonly MenuItem _menuItemGoBack = new MenuItem(ShortcutGoBack, "Back", null);
    private readonly MenuItem _menuItemGoToMain = new MenuItem(ShortcutGoMain, "Main menu", null);


    public Menu(EMenuLevel level, string title, List<MenuItem> menuItems)
    {
        Title = title;
        _level = level;

        foreach (var itemItem in menuItems)
        {
            _menuItems.Add(itemItem.Shortcut, itemItem);
        }

        if (_level != EMenuLevel.Main)
            _menuItems.Add(ShortcutGoBack, _menuItemGoBack);
        if (_level == EMenuLevel.Other)
            _menuItems.Add(ShortcutGoMain, _menuItemGoToMain);
        _menuItems.Add(ShortcutExit, _menuItemExit);
    }

    public string RunMenu()
    {
        string userChoice;
        var menuDone = false;
        do
        {
            Console.WriteLine(Title);
            Console.WriteLine("================");
            foreach (var menuItem in _menuItems.Values)
            {
                Console.WriteLine(menuItem);
            }


            Console.WriteLine("================");
            Console.WriteLine("Your choice: ");
            userChoice = Console.ReadLine()?.ToUpper().Trim() ?? "";

            if (_menuItems.ContainsKey(userChoice))
            {
                string? methodReturnValue = null;
                if (_menuItems[userChoice].MethodToRun != null)
                {
                    methodReturnValue = _menuItems[userChoice].MethodToRun!();
                }

                if (userChoice == ShortcutGoBack)
                {
                    menuDone = true;
                }

                if (methodReturnValue == ShortcutExit ||
                    userChoice == ShortcutExit)
                {
                    userChoice = methodReturnValue ?? userChoice;
                    menuDone = true;
                }

                if ((userChoice != ShortcutGoMain && methodReturnValue != ShortcutGoMain)
                    || _level == EMenuLevel.Main) continue;
                userChoice = methodReturnValue ?? userChoice;
                menuDone = true;
            }
            else
            {
                Console.WriteLine("\n Enter one of the options");
            }
        } while (menuDone == false);

        return userChoice;
    }
}