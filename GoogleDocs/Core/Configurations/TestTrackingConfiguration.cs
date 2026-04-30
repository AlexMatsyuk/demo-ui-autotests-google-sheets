namespace GoogleDocs.Configurations;

public class TestTrackingConfiguration : IConfiguration
{
    public string JsonSectionName => "TestTracking";
    public string? TestCaseUrlTemplate { get; set; }
    public string? BugUrlTemplate { get; set; }
}