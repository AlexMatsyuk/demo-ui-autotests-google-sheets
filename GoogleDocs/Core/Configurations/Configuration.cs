using System.Globalization;
using GoogleDocs.Utils;
using Microsoft.Extensions.Configuration;

namespace GoogleDocs.Configurations;

public static class Configuration
{
    private static readonly IConfigurationRoot ConfigurationRoot = GetConfiguration();
    public static CultureInfo CultureInfo { get; } = new(GetValue("CultureInfo") ?? "en-us");
    public static readonly BrowserConfiguration Browser = BindConfiguration<BrowserConfiguration>();
    public static readonly AssertConfiguration Assert = BindConfiguration<AssertConfiguration>();
    public static readonly WaitConfiguration Wait = BindConfiguration<WaitConfiguration>();
    public static readonly TestTrackingConfiguration TestTracking = BindConfiguration<TestTrackingConfiguration>();
    public static readonly LogConfiguration Log = BindConfiguration<LogConfiguration>();
    public static readonly LocalizationConfiguration Localization = BindConfiguration<LocalizationConfiguration>();
    public static string? AutotestsDataMarker { get; } = GetValue("AutotestsDataMarker");

    /// <summary>
    /// Get property from appsettings.json.
    /// For 2-level properties, like that:
    /// "Browser" : { "StartUrl": "some.url" }
    /// the following key should be used:
    /// GetValue("Browser:StartUrl").
    /// </summary>
    public static string? GetValue(string key)
    {
        return ConfigurationRoot[key];
    }

    public static bool GetBoolValue(string key)
    {
        return bool.Parse(ConfigurationRoot[key]!);
    }

    /// <summary>
    /// Bind section of json to configuration object.
    /// </summary>
    /// <typeparam name="T">class of the object, that should be created.</typeparam>
    public static T BindConfiguration<T>(IConfigurationRoot? configurationRoot = null)
        where T : IConfiguration
    {
        T config = Activator.CreateInstance<T>();
        (configurationRoot ?? ConfigurationRoot).GetSection(config.JsonSectionName).Bind(config);
        return config;
    }

    /// <summary>
    /// Method declares the order of configuration resources, that should be used to extract configuration data.
    /// If the same configuration key is specified in several resources, than value is overriden in the last specified resource.
    /// Method can be updated to read data from console, azure and so on.
    /// <see cref="http://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1"/>
    /// </summary>
    private static IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.custom.json", true, true)
            .AddJsonFile("override.appsettings.json", true, true)
            .Build();
    }
}