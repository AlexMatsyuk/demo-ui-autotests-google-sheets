using GoogleDocs.Localization;
using GoogleDocs.Ui;

namespace Forms.Dialogs;

public class ConfirmationDialog
{
    private readonly Element _okButton = new($"div[class*='modal-dialog'] button:text-is('{LocalizedText.OK}')", "OK button");

    public void Confirm()
    {
        _okButton.Click();
        _okButton.WaitForHidden();
    }
}