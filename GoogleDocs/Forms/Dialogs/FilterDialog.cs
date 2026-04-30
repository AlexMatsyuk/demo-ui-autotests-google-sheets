using Forms.Elements;
using GoogleDocs.Localization;
using GoogleDocs.Ui;

namespace Forms.Dialogs;

public class FilterDialog
{
    private Checkbox ValueFilter(string value) =>
        new($"//div[@class='waffle-filterbox-menu']//div[@role='menuitemcheckbox'][./div[text()='{value}']]", $"Checkbox: {value}");

    private readonly Element _okButton = 
        new($"//div[contains(@class,'waffle-filterbox-container')]//div[@role='button' and text()='{LocalizedText.OK}']", "Button OK");

    public void ExcludeFilterValues(params string[] values)
    {
        values.ToList().ForEach(value => ValueFilter(value).TurnOff());
        _okButton.Click();
        _okButton.WaitForHidden();
    }
}