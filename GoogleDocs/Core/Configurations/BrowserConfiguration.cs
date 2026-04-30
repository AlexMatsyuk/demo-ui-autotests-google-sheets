using GoogleDocs.Ui;

namespace GoogleDocs.Configurations;

public class BrowserConfiguration : IConfiguration
{
    private const string SectionName = "Browser";
    public string JsonSectionName => SectionName;
    public string? StartUrl { get; set; }
    public string? BaseUrl { get; set; }
    public bool Headless { get; set; }
    public int HeadlessWindowWidth { get; set; }
    public int HeadlessWindowHeight { get; set; }
    public double ScaleFactor { get; set; }
    public BrowserType BrowserType { get; set; } = (BrowserType)Enum.Parse(typeof(BrowserType), Configuration.GetValue($"{SectionName}:Type") ?? "Chrome");
    public OsType OsType { get; set; } = (OsType)Enum.Parse(typeof(OsType), Configuration.GetValue($"{SectionName}:OS") ?? "Windows");
}