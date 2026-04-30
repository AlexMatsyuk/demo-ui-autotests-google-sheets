using GoogleDocs.Configurations;
using GoogleDocs.Utils;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Logger = GoogleDocs.Logs.Logger;

namespace GoogleDocs.Test;

[Parallelizable(ParallelScope.All)]
public class BaseTest
{
    protected Logger Log => Logger.Instance;
    protected static readonly ThreadLocal<int> StepNumberInstances = new();
    public static int StepNumber => GetStepNumber();

    [SetUp]
    public void BeforeTest()
    {
        Log.TestCase(TestContext.CurrentContext.Test);
        Log.Bugs(TestContext.CurrentContext.Test);
        Log.Categories(TestContext.CurrentContext.Test);
        if (Configuration.Log.XmlView)
        {
            Log.Info("<test>");
        }
    }

    [TearDown]
    public void AfterTest()
    {
        if (Configuration.Assert.SoftAssertEnabled)
        {
            FinishSoftAssert();
        }
        Log.StepFinish();
        if(Configuration.Log.XmlView)
        {
            Log.Info("</test>");
        }
        Log.SubmitLogsInTheEndOfTest();
        StepNumberInstances.Value = 0;
    }

    /// <summary>
    /// Log step using method name. Method name LogIn_UsersPage with be transformed to "Log in. Open Users Page"
    /// Run the method.
    /// </summary>
    /// <param name="method">test method that should be executed</param>
    protected void Step(Action method)
    {
        StepNumberInstances.Value++;
        Log.Step(StepNumber, method.Method.Name);
        method.Invoke();
        Log.StepFinish();
    }

    protected void Steps<T>(Action<T> method, List<T> parameters)
    {
        parameters.ForEach(parameter => StepWithParameter(method, parameter));
    }

    protected void Steps<T1, T2>(Action<T1, T2> method, List<T1> parameters1, List<T2> parameters2)
    {
        parameters1.ForEach(param1 => parameters2.ForEach(param2 => StepWithParameter(method, param1, param2)));
    }

    protected void Steps<T>(Action<T> method, params T[] parameters)
    {
        Steps(method, parameters.ToList());
    }

    protected static void Step(Action method, string stepDescription, Dictionary<string, string>? stepData = null)
    {
        StepNumberInstances.Value++;
        Logger.Instance.Step(StepNumber, stepDescription.Replace("'", "\""), false, stepData);
        method.Invoke();
        Logger.Instance.StepFinish();
    }

    private static void FinishSoftAssert()
    {
        SoftAssert softAssert = SoftAssert.Instance;
        try
        {
            if (softAssert.HasErrors)
            {
                TestResult result = TestExecutionContext.CurrentContext.CurrentResult;
                result.RecordAssertion(AssertionStatus.Failed, softAssert.GetErrorMessages(), softAssert.FirstErrorStackTrace);
                result.RecordTestCompletion();
                throw new AssertionException(result.Message);
            }
        }
        catch (AssertionException)
        {
        }
        finally
        {
            SoftAssert.Close();
            StepNumberInstances.Value = 0;
        }
    }

    private void StepWithParameter<T>(Action<T> method, T parameter)
    {
        StepNumberInstances.Value++;
        Log.Step(StepNumber, $"{method.Method.Name}_{parameter}");
        method.Invoke(parameter);
        Log.StepFinish();
    }

    private void StepWithParameter<T1, T2>(Action<T1, T2> method, T1 parameter1, T2 parameter2)
    {
        StepNumberInstances.Value++;
        Log.Step(StepNumber, $"{method.Method.Name}_{parameter1}_{parameter2}");
        method.Invoke(parameter1, parameter2);
        Log.StepFinish();
    }

    private static int GetStepNumber()
    {
        if (!StepNumberInstances.IsValueCreated)
        {
            StepNumberInstances.Value = 1;
        }
        return StepNumberInstances.Value;
    }
}