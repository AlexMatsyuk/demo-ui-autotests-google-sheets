namespace GoogleDocs.Configurations;

public class LogConfiguration : IConfiguration
{
    public string JsonSectionName => "Log";
    public bool DebugLevel { get; set; }
    public bool XmlView { get; set; }
    public bool LogToReport { get; set; }
    public bool ShowParsedJson { get; set; }
    public bool ShowNotParsedJson { get; set; }
    public bool SaveDomIfFailed { get; set; }
    public bool LogPassedTests { get; set; }
}