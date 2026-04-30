using Forms;
using Forms.Constants;
using Forms.Dialogs;
using Forms.Pages;
using GoogleDocs.Test;
using NUnit.Framework;

namespace Tests;

public class FilterTest : BaseUiTest
{
    private const string StartCell = "A1";
    private bool _sheetAdded;
    private readonly Dictionary<string, string> _cellValues = new()
    {
        ["A1"] = "TITLE",
        ["A2"] = "abc",
        ["A3"] = "qweee",
        ["A4"] = "abcd",
        ["A5"] = "abc",
        ["A6"] = "xyz",
    };
    private readonly string[] _filterValuesToExclude = ["qweee", "abcd"];
    private readonly Dictionary<string, string> _filteredCellValues = new()
    {
        ["A1"] = "TITLE",
        ["A2"] = "abc",
        ["A5"] = "abc",
        ["A6"] = "xyz",
    };

    [SetUp]
    public void TestSetUp()
    {
        Navigation.OpenSpreadSheet(Constants.SpreadSheet1);
        new GoogleSheetPage().Footer.AddNewSheet();
        _sheetAdded = true;
    }
    
    [TearDown]
    public void TestTearDown()
    {
        if (_sheetAdded)
        {
            new GoogleSheetPage().Footer.DeleteCurrentSheet();
        }
    }
    
    [Test]
    public void FilterTest1()
    {
        Step(CreateFilter);
        Step(SetCellValues);
        Step(FilterValuesAndAssert);
    }

    private void CreateFilter()
    {
        var sheet = new GoogleSheetPage();
        sheet.Header.SelectCell(StartCell);
        sheet.TopMenu.ClickMenuSubItem(MenuSubItem.CreateFilter);
    }

    private void SetCellValues()
    {
        new GoogleSheetPage().SetCellValues(_cellValues);
    }

    private void FilterValuesAndAssert()
    {
        var sheet = new GoogleSheetPage();
        sheet.OpenCellFilterMenu(StartCell);
        new FilterDialog().ExcludeFilterValues(_filterValuesToExclude);
        sheet.AssertCellValues(StartCell, _filteredCellValues);
    }
    
}