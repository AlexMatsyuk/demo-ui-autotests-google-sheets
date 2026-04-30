using GoogleDocs.Ui;
using GoogleDocs.Utils;

namespace Forms.Components;

public class Header
{
    private readonly Element _cellAddressInput = new("input#t-name-box", "Cell Address Input");
    private readonly Element _cellValueInput = new("div#formula-bar div.cell-input", "Cell Value Input");
    
    public void SelectCell(string cellAddress)
    {
        _cellAddressInput.SetText(cellAddress, pressEnter: true);
    }

    public string GetCellAddress()
    {
        return _cellAddressInput.GetInputValue();
    }

    public string GetCellValue()
    {
        return _cellValueInput.GetText().TrimEndOnce("\n");
    }

    public void WaitForCellAddressChange(string previousAddress)
    {
        _cellAddressInput.WaitForValueChanged(previousAddress);
    }
}