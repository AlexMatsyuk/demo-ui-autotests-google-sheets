using Entities;
using GoogleDocs.Configurations;
using GoogleDocs.Ui;

namespace Forms;

public static class Navigation
{
    public static void OpenSpreadSheet(SpreadSheet spreadSheet)
    {
        string url = $"{Configuration.Browser.BaseUrl}/spreadsheets/d/{spreadSheet.Id}/edit?hl={Configuration.Localization.Locale}";
        Browser.Instance.OpenUrl(url);
    }
}