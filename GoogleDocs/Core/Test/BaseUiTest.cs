using GoogleDocs.Configurations;
using GoogleDocs.Ui;
using GoogleDocs.Utils;
using NUnit.Framework;

namespace GoogleDocs.Test;

public class BaseUiTest : BaseTest
{
    protected static Browser Browser => Browser.Instance;

    [SetUp]
    public void BeforeUiTest()
    {
        Browser.OpenStartUrl();
    }

    [TearDown]
    public void AfterUiTest()
    {
        try
        {
            if (TestContext.CurrentContext.Result.PassCount == 0 || (Configuration.Assert.SoftAssertEnabled && SoftAssert.Instance.HasErrors))
            {
                Browser.CaptureScreenshot(GetType().Name.AddTimeStamp(false));
            }
        }
        catch (Exception)
        {
            // skip
        }
        finally
        {
            Browser.Close();
        }
    }
}