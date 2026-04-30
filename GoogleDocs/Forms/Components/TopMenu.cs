using Forms.Constants;
using GoogleDocs.Ui;

namespace Forms.Components;

public class TopMenu
{
    private Element MenuItemLink(MenuItem menuItem) => new($"//div[@id='docs-menubar']/div[text()='{menuItem.Name}']", $"Menu item: {menuItem.Name}");
    private Element MenuSumItemLink(MenuSubItem menuSubItem) => 
        new($"//div[@role='menuitem']//span[starts-with(@aria-label,'{menuSubItem.AriaLabel}')]", $"Menu sub item: {menuSubItem.AriaLabel}");

    public void ClickMenuSubItem(MenuSubItem menuSubItem)
    {
        MenuItemLink(menuSubItem.MenuItem).Click();
        MenuSumItemLink(menuSubItem).Click();
    }

}