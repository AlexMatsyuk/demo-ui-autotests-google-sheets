namespace GoogleDocs.Configurations;

public class LocalizationConfiguration : IConfiguration
{
    private const string SectionName = "Localization";
    public string JsonSectionName => SectionName;

    public string Locale { get; set; } = "en";
}
