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
    private readonly List<string> _menuItemsAsString = new List<string>();
    private bool _menuDone;
    private string _userChoice = "";


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
                
        foreach (var dictValue in _menuItems.Values)
        {
            _menuItemsAsString.Add(dictValue.Title);
        }
    }

    public string RunMenu()
    {
        
        do
        {
            string? methodReturnValue = null;
            if (_userChoice == ShortcutExit)
            {
                _menuDone = true;
                _userChoice = methodReturnValue ?? _userChoice;
                continue;

            }
            _userChoice = GetNewUserInput();
            if (_menuItems.ContainsKey(_userChoice))
            {
                if (_menuItems[_userChoice].MethodToRun != null)
                {
                    methodReturnValue = _menuItems[_userChoice].MethodToRun!();
                }


                if (methodReturnValue == ShortcutGoBack)
                {
                    
                }
                if (_userChoice == ShortcutGoBack)
                {
                    _menuDone = true;
                }

                if (methodReturnValue == ShortcutExit ||
                    _userChoice == ShortcutExit)
                {
                    _userChoice = methodReturnValue ?? _userChoice;
                    _menuDone = true;
                }

                if ((_userChoice != ShortcutGoMain && methodReturnValue != ShortcutGoMain)
                    || _level == EMenuLevel.Main) continue;
                _userChoice = methodReturnValue ?? _userChoice;
                _menuDone = true;
            }
            else
            {
                Console.WriteLine("\n Enter one of the options");
            }
        } while (_menuDone == false);

        return _userChoice;
    }

    private string GetNewUserInput()
    {
        var selectedClass = ConsoleHelper.MultipleChoice(true, Title, _menuItemsAsString.ToArray());

        var userChoice = "";

        foreach (var keyValuePair in _menuItems)
        {
            if (!keyValuePair.Value.Title.Contains(selectedClass)) continue;
            userChoice = keyValuePair.Key;
            break;
        }

        return userChoice;
    }

    public void ExitMenu()
    {
        _userChoice = ShortcutExit;
        _menuDone = true;
    }
}