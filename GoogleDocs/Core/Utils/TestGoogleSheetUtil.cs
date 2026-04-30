using System.Text.RegularExpressions;
using GoogleDocs.Logs;
using NUnit.Framework;

namespace GoogleDocs.Utils;

public static class TestGoogleSheetUtil
{
    public static List<string?> GetAttributes(this TestContext.TestAdapter testAdapter, Type type)
    {
        return testAdapter.GetAttributes(type.Name.Replace("Attribute", string.Empty));
    }
    
    public static List<string?> GetAttributes(this TestContext.TestAdapter testAdapter, string attributeName)
    {
        return testAdapter.Properties[attributeName].Select(x => x.ToString()).ToList();
    }

    
    public static string? GetTestCaseId(this TestContext.TestAdapter testAdapter)
    {
        var match = Regex.Match(testAdapter.Name, @"\d+");
        if (match.Success)
        {
            return match.Value;
        }
        Logger.Instance.Fail($"Cannot get test case id from method name: {testAdapter.Name}. Test method name should contain a numeric ID.");
        return null;
    }
}