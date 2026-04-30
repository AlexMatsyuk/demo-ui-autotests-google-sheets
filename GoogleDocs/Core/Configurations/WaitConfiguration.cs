namespace GoogleDocs.Configurations;

public class WaitConfiguration : IConfiguration
{
    public string JsonSectionName => "Wait";
    public double DefaultTimeout { get; set; }
    public double LongTimeout { get; set; }
    public int HttpRequestTimeout { get; set; }
    public double PageLoadTimeout { get; set; }
    public double DownloadTimeout { get; set; }
    public double EmailTimeout { get; set; }
    public double PactTimeout { get; set; }
    public double ShortTimeout { get; set; }
    public double DatabaseTimeout { get; set; }
    public double PollingInterval { get; set; }
    public double LongPollingInterval { get; set; }
    public double DatabasePollingInterval { get; set; }
}