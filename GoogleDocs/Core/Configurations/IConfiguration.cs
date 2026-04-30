namespace GoogleDocs.Configurations;

/// <summary>
/// Implement this interface in your configuration class
/// if you would like to bind configurations from json file
/// using the "Configuration.BindConfiguration"
/// </summary>
public interface IConfiguration
{
    string JsonSectionName { get; }
}