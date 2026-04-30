namespace Forms.HotKeys;

public class MacHotKeys : IHotKeys
{
    public string Modifier => "Meta";

    public string SelectAll => "Meta+a";
    public string Copy => "Meta+c";
    public string Paste => "Meta+v";
    public string Cut => "Meta+x";
    public string Undo => "Meta+z";
    public string Redo => "Meta+Shift+z";
    public string Save => "Meta+s";
    public string Find => "Meta+f";

    public string OpenFilterMenu => "Control+Option+r";
    public string ClearFormatting => "Meta+\\";
    public string InsertLink => "Meta+k";
    public string Bold => "Meta+b";
    public string Italic => "Meta+i";
    public string Underline => "Meta+u";
}
