using Forms.Dialogs;
using GoogleDocs.Localization;
using GoogleDocs.Ui;
using GoogleDocs.Utils;

namespace Forms.Components;

public class Footer
{
    private static Browser Browser => Browser.Instance;

    private readonly Element _addNewSheetButton = new("div[class*='docs-sheet-add-button']", "Add new sheet button");
    private readonly Element _activeTab = new("div[class*='docs-sheet-active-tab']", "Active sheet tab");
    private readonly Element _deleteMenuItem = 
        new($"div[class*='docs-menu'] div[class*='goog-menuitem-content']:text-is('{LocalizedText.Delete}')", "Delete menu item");

    public void AddNewSheet()
    {
        var initialUrl = Browser.GetUrl();
        _addNewSheetButton.Click();
        WaitUtil.WaitForCondition(() => Browser.GetUrl() != initialUrl, "New sheet is added", false);
    }

    public void DeleteCurrentSheet()
    {
        var initialUrl = Browser.GetUrl();
        _activeTab.RightClick();
        _deleteMenuItem.Click();
        new ConfirmationDialog().Confirm();
        WaitUtil.WaitForCondition(() => Browser.GetUrl() != initialUrl, "Sheet is deleted", false);
    }
}