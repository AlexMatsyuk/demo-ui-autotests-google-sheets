using System.Text.RegularExpressions;

namespace Forms.Utils;

public static class GoogleSheetUtil
{
    public static string GetNextCellAddress(string cellAddress)
    {
        var match = Regex.Match(cellAddress, @"^([A-Z]+)(\d+)$");
        return match.Groups[1].Value + (int.Parse(match.Groups[2].Value) + 1);
    }
}