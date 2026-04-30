using System.Text.RegularExpressions;
using Forms.Components;
using Forms.HotKeys;
using Forms.Utils;
using GoogleDocs.Test;
using GoogleDocs.Ui;

namespace Forms.Pages;

public class GoogleSheetPage
{
    private static Browser Browser => Browser.Instance;
    private static IHotKeys HotKeys => HotKeysFactory.Instance;

    public readonly TopMenu TopMenu = new();
    public readonly Header Header = new();
    public readonly Footer Footer = new();

    public void SetCellValues(Dictionary<string, string> values)
    {
        values.ToList().ForEach(x => SetCellValue(x.Key, x.Value));
    }
    
    public void SetCellValue(string cellAddress, string value)
    {
        Header.SelectCell(cellAddress);
        Browser.Type(value, pressEnter: true);
    }

    public void OpenCellFilterMenu(string cellAddress)
    {
        Header.SelectCell(cellAddress);
        Browser.PressKey(HotKeys.OpenFilterMenu);
    }

    public void AssertCellValues(string startCell, Dictionary<string, string> expectedValues)
    {
        var actualValues = ReadColumn(startCell, expectedValues.Count + 1);
        var nextCellAddress = GoogleSheetUtil.GetNextCellAddress(expectedValues.Last().Key);
        expectedValues.Add(nextCellAddress, string.Empty);
        Assert.AssertEqual(actualValues, expectedValues, "cell values");
    }
    

    public Dictionary<string, string> ReadColumn(string startCell, int rowCount)
    {
        var result = new Dictionary<string, string>();
        Header.SelectCell(startCell);
        for (int i = 0; i < rowCount; i++)
        {
            string address = Header.GetCellAddress();
            string value = Header.GetCellValue();
            result[address] = value;
            if (i < rowCount - 1)
            {
                Browser.PressKey("ArrowDown");
                Header.WaitForCellAddressChange(address);
            }
        }
        return result;
    }
}
