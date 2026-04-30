using GoogleDocs.Localization;

namespace Forms.Constants;

public class MenuItem(string name)
{
    public string Name { get; } = name;
    
    public static readonly MenuItem File = new(LocalizedText.File);
    public static readonly MenuItem Edit = new(LocalizedText.Edit);
    public static readonly MenuItem Insert = new(LocalizedText.Insert);
    public static readonly MenuItem Format = new(LocalizedText.Format);
    public static readonly MenuItem Data = new(LocalizedText.Data);
}