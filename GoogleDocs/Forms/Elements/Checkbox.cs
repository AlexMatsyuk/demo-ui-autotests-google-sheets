using GoogleDocs.Test;
using GoogleDocs.Ui;

namespace Forms.Elements;

public class Checkbox : Element
{
    private string Attribute { get; }
    private Func<string, bool> OnCondition { get; }
    private Func<string, bool> OffCondition { get; }
    
    public Checkbox(string selector, 
                    string name, 
                    Element? parent = null, 
                    string attribute = "aria-checked",
                    Func<string, bool>? onCondition = null,
                    Func<string, bool>? offCondition = null)
        : base(selector, name, parent)
    {
        Attribute = attribute;
        OnCondition = onCondition ?? (attributeValue => attributeValue == "true");
        OffCondition = offCondition ?? (attributeValue => attributeValue == "false");
    }

    public void TurnOff()
    {
        AssertIsOn();
        Click();
        AssertIsOff();
    }
    
    public void TurnOn()
    {
        AssertIsOff();
        Click();
        AssertIsOn();
    }

    public void AssertIsOn()
    {
        Assert.AssertTrue(OnCondition.Invoke(GetAttribute(Attribute)), "Checkbox is turned on", "Checkbox is not turned on", false);
    }
    
    public void AssertIsOff()
    {
        Assert.AssertTrue(OffCondition.Invoke(GetAttribute(Attribute)), "Checkbox is turned off", "Checkbox is not turned off", false);
    }
}