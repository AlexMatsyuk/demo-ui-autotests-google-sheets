using GoogleDocs.Localization;

namespace Forms.Constants;

public class MenuSubItem(MenuItem menuItem, string ariaLabel)
{
    public MenuItem MenuItem { get; } = menuItem;
    public string AriaLabel { get; } = ariaLabel;

    public static readonly MenuSubItem CreateFilter = new(MenuItem.Data, LocalizedText.CreateFilter);
    public static readonly MenuSubItem RemoveFilter = new(MenuItem.Data, LocalizedText.RemoveFilter);
}