using System.Text.Json;
using GoogleDocs.Configurations;

namespace GoogleDocs.Localization;

public static class LocalizedText
{
    private static readonly Dictionary<string, string> Translations = LoadTranslations();

    public static string Delete => Get(nameof(Delete), "Delete");
    public static string OK => Get(nameof(OK), "OK");
    public static string Cancel => Get(nameof(Cancel), "Cancel");
    public static string Save => Get(nameof(Save), "Save");
    public static string Close => Get(nameof(Close), "Close");
    public static string Apply => Get(nameof(Apply), "Apply");
    public static string Clear => Get(nameof(Clear), "Clear");
    public static string SelectAll => Get(nameof(SelectAll), "Select all");
    
    public static string File => Get(nameof(File), "File");
    public static string Edit => Get(nameof(Edit), "Edit");
    public static string Insert => Get(nameof(Insert), "Insert");
    public static string Format => Get(nameof(Format), "Format");
    public static string Data => Get(nameof(Data), "Data");
    
    public static string CreateFilter => Get(nameof(CreateFilter), "Create a filter");
    public static string RemoveFilter => Get(nameof(RemoveFilter), "Remove filter");


    /// <summary>
    /// Gets localized text by key. Returns default value if key not found in JSON.
    /// </summary>
    private static string Get(string key, string defaultValue)
    {
        return Translations.GetValueOrDefault(key, defaultValue);
    }

    private static Dictionary<string, string> LoadTranslations()
    {
        var locale = Configuration.Localization.Locale;
        var fileName = $"localization-{locale}.json";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return new Dictionary<string, string>();
        }

        var json = System.IO.File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }
}
