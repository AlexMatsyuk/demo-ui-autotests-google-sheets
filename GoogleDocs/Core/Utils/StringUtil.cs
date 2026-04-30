using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using GoogleDocs.Attributes;
using GoogleDocs.Configurations;
using GoogleDocs.Logs;
using NUnit.Framework;

namespace GoogleDocs.Utils;

public static class StringUtil
{
    private const char LongTextChar = 'x';
    private static readonly ThreadLocal<Random> Random = new(() => new Random(Interlocked.Increment(ref _seed)));
    private static int _increment;
    private static int _seed = Environment.TickCount;
    public static int Increment => _increment += 1;

    public static string AddTimeStamp(this string text, bool addMarker = true)
    {
        return new StringBuilder()
            .Append(addMarker ? Configuration.AutotestsDataMarker : string.Empty)
            .Append(text)
            .Append(GetUniqueString())
            .ToString();
    }

    public static string AddTimeStamp(this string? text, int totalLength, bool addMarker = true)
    {
        string textWithTimeStamp = text == null ? string.Empty.AddTimeStamp(addMarker) : text.AddTimeStamp(addMarker);
        return textWithTimeStamp.AddCharacters(totalLength);
    }

    public static string AddCharacters(this string text, int totalLength)
    {
        string addition = new(LongTextChar, totalLength - (text?.Length ?? 0));
        return $"{text}{addition}";
    }

    public static string GetUniqueString()
    {
        return $"{Increment}{DateTime.UtcNow:mmssffff}{Random.Value!.Next(100, 999)}";
    }

    public static string AddUrlParts(this string url, params string[] urlParts)
    {
        List<string> parts = [url];
        parts.AddRange(urlParts);
        return string.Join('/', parts.ToArray());
    }

    public static string Shorten(this string text, int count)
    {
        return text.Length < count ? text : $"{text.Substring(0, count)} ... (shortened)";
    }

    // "AbC qwe   ZXC kLm" -> "AbCQweZXCKLm"
    public static string ToPascalCase(this string text)
    {
        List<string> parts = text.Split(" ")
            .ToList()
            .ConvertAll(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList()
            .ConvertAll(x => char.IsUpper(x[0]) ? x : char.ToUpperInvariant(x[0]) + x.Substring(1));
        return string.Join(string.Empty, parts);
    }

    public static string Repeat(this string text, int count)
    {
        return new StringBuilder(text.Length * count).Insert(0, text, count).ToString();
    }
    
    public static string TrimEndOnce(this string str, string suffix)
    {
        return str.EndsWith(suffix) ? str[..^suffix.Length] : str;
    }

    public static string GetRandomEmail(this string email, string? randomPart = null)
    {
        return email.Replace("@", randomPart == null ? "+".AddTimeStamp(false) + "@" : $"+{randomPart}@");
    }

    public static string GetLogMessage(this string text)
    {
        return $"{DateTime.Now:HH:mm:ss,ff} .. {text}";
    }

    public static string WithPrefix(this string name, string? prefix, string separator = ".")
    {
        return prefix == null ? name : $"{prefix}{separator}{name}";
    }
    
    public static List<string>? GetBugsIds(this TestContext.TestAdapter testAdapter)
    {
        string? ids = testAdapter.GetAttributes(typeof(BugAttribute)).FirstOrDefault();
        return ids?.Split(Logger.BugsSplitter).ToList();
    }
}