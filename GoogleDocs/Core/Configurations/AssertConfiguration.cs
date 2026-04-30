namespace GoogleDocs.Configurations;

public class AssertConfiguration : IConfiguration
{
    private const string SectionName = "Assert";
    public string JsonSectionName => SectionName;
    public bool SoftAssertEnabled { get; set; }
    public double EstimationFault { get; set; }
}