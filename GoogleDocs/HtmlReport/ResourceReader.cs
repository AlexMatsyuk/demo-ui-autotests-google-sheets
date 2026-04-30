using System;
using System.IO;

namespace HtmlReport;

internal static class ResourceReader
{
    internal static string LoadTextFromResource(string name)
    {
        string result;
        using (StreamReader sr = new(StreamFromResource(name)))
        {
            result = sr.ReadToEnd();
        }
        return result;
    }

    internal static Stream StreamFromResource(string name)
    {
        return new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name), FileMode.Open);
    }
}