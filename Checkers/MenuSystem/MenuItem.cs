namespace MenuSystem;

public class MenuItem
{
    // In the declaration it is null but later on it will not allow us to assign null value to it.
    public string Title { get; set; }
    public string Shortcut { get; set; }
    public Func<string>? MethodToRun { get; set; }

    public MenuItem(string shortcut, string title, Func<string>? methodToRun)
    {
        Shortcut = shortcut;
        Title = title;
        MethodToRun = methodToRun;
    }
    
    public override string ToString() => Shortcut + ") " + Title;
}