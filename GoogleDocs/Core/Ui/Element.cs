using GoogleDocs.Logs;
using GoogleDocs.Utils;
using Microsoft.Playwright;

namespace GoogleDocs.Ui;

public class Element
{
    private static Logger Log => Logger.Instance;
    public string Selector { get; }
    public Element? Parent { get; }
    public string Name { get; }

    public Element(string selector, string name, Element? parent = null)
    {
        Selector = selector;
        Name = name;
        Parent = parent;
    }

    public void Click()
    {
        Log.Info($"Click {Name}");
        GetElement().ClickAsync().GetAwaiter().GetResult();
    }

    public void RightClick()
    {
        Log.Info($"Right click {Name}");
        GetElement().ClickAsync(new() { Button = MouseButton.Right }).GetAwaiter().GetResult();
    }

    public void SetText(string text, bool pressEnter = false)
    {
        Log.Info($"Set text {Name}: {text}");
        GetElement().FillAsync(text).GetAwaiter().GetResult();
        if (pressEnter)
        {
            Browser.Instance.PressKey("Enter");
        }
    }

    public void Clear()
    {
        Log.Info($"Clear {Name}");
        GetElement().ClearAsync().GetAwaiter().GetResult();
    }

    public string GetText()
    {
        return GetElement().InnerTextAsync().GetAwaiter().GetResult();
    }

    public string GetAttribute(string name)
    {
        return GetElement().GetAttributeAsync(name).GetAwaiter().GetResult()!;
    }

    /// <summary>
    /// Gets the value of an input element. Use this for input, textarea, select elements.
    /// </summary>
    public string GetInputValue()
    {
        return GetElement().InputValueAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Gets element property via JavaScript evaluation.
    /// </summary>
    public string GetPropertyValue(string propertyName)
    {
        return GetElement().EvaluateAsync<string>($"el => el.{propertyName}").GetAwaiter().GetResult() ?? "";
    }
    
    public void WaitForValueChanged(string previousValue, double timeoutSeconds = 1)
    {
        Log.InfoDebug($"Wait for {Name} value changed");
        WaitUtil.WaitForCondition(() => GetInputValue() != previousValue, timeoutSeconds, $"Element {Name} value changed: {previousValue}", false);
    }

    public void WaitForHidden()
    {
        Log.InfoDebug($"Wait for {Name} hidden");
        GetElement().WaitForAsync(new LocatorWaitForOptions {State = WaitForSelectorState.Hidden}).GetAwaiter().GetResult();
    }

    private ILocator GetElement()
    {
        return Parent == null 
            ? Browser.Instance.Page.Locator(Selector) 
            : Browser.Instance.Page.Locator(Parent.Selector).Locator(Selector);
    }
}