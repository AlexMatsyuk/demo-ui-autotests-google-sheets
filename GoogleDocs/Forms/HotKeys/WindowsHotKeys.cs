namespace Forms.HotKeys;

/// <summary>
/// Keyboard shortcuts for Windows OS.
/// </summary>
public class WindowsHotKeys : IHotKeys
{
    public string Modifier => "Control";

    public string SelectAll => "Control+a";
    public string Copy => "Control+c";
    public string Paste => "Control+v";
    public string Cut => "Control+x";
    public string Undo => "Control+z";
    public string Redo => "Control+y";
    public string Save => "Control+s";
    public string Find => "Control+f";

    public string OpenFilterMenu => "Control+Alt+R";
    public string ClearFormatting => "Control+\\";
    public string InsertLink => "Control+k";
    public string Bold => "Control+b";
    public string Italic => "Control+i";
    public string Underline => "Control+u";
}
