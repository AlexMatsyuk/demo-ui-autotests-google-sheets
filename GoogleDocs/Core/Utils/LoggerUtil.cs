using System.Text;
using GoogleDocs.Configurations;
using GoogleDocs.Logs;
using NUnit.Framework;

namespace GoogleDocs.Utils;

public static class LoggerUtil
{
    private const int TimeLogLength = 15;
    private static readonly ThreadLocal<bool> StepOpenedInstances = new(false);
    private static bool StepOpened { get => GetStepOpened(); set => StepOpenedInstances.Value = value; }

    public static void Categories(this Logger logger, TestContext.TestAdapter testAdapter)
    {
        string categories = string.Join(Logger.BugsSplitter, testAdapter.GetAttributes(typeof(CategoryAttribute)));
        if(string.IsNullOrEmpty(categories))
        {
            return;
        }
        if(Configuration.Log.XmlView)
        {
            logger.Info($"<categories value='{categories}'/>");
            return;
        }
        logger.Info($"### Categories: {categories}");
    }

    public static void TestCase(this Logger logger, TestContext.TestAdapter testAdapter)
    {
        string? id = testAdapter.GetTestCaseId();
        if(string.IsNullOrEmpty(id))
        {
            return;
        }
        string? urlTemplate = Configuration.TestTracking.TestCaseUrlTemplate;
        if(string.IsNullOrEmpty(urlTemplate))
        {
            return;
        }
        string url = string.Format(urlTemplate, id);
        if(Configuration.Log.XmlView)
        {
            logger.Info($"<testCase id='{id}' href='{url}'/>");
            return;
        }
        logger.Info($"### Test {id}: {url}");
    }

    public static void Bugs(this Logger logger, TestContext.TestAdapter testAdapter)
    {
        List<string>? ids = testAdapter.GetBugsIds();
        if(ids == null || !ids.Any())
        {
            return;
        }
        ids.ForEach(id =>
        {
            string url = string.IsNullOrEmpty(Configuration.TestTracking.BugUrlTemplate)
                ? id.ToString()
                : string.Format(Configuration.TestTracking.BugUrlTemplate, id);
            logger.Info(Configuration.Log.XmlView ? $"<bug id='{id}' href='{url}'/>" : $"### Bug {id}: {url}");
        });
    }

    /// <summary>
    /// Log step using method name.
    /// LogIn_UsersPage with be transformed to "Log in. Open Users Page"
    /// </summary>
    public static void Step(
        this Logger logger,
        int stepNumber,
        string methodName,
        bool addWhiteSpaces = true,
        Dictionary<string, string>? stepData = null)
    {
        Logger.Instance.StepFinish();
        LogStepStart(logger, stepNumber, addWhiteSpaces ? ConvertCamelCaseToText(methodName) : methodName, stepData);
    }

    public static void LogStepData(this Logger logger, string step, string message)
    {
        if(Configuration.Log.XmlView)
        {
            logger.Info($"<stepData id='{step}' name='{message}'/>");
            StepOpened = true;
            return;
        }
        logger.Info(message);
    }

    public static void LogStepStart(
        this Logger logger,
        int step,
        string message,
        Dictionary<string, string>? stepData = null)
    {
        if(Configuration.Log.XmlView)
        {
            logger.Info($"<step id='{step}' name='{message.Replace('\'', '´').Replace(">", "＞")}'>");
            StepOpened = true;
            stepData?.ToList().ForEach(keyValue => Logger.Instance.LogStepData(keyValue.Key, keyValue.Value));
            logger.Info("<span>");
            return;
        }
        string stepMessage = $"[ Step {step} ]  .. {message}";
        string delimitersInRowWithTime = new('-', stepMessage.Length);
        string delimiters = new('-', stepMessage.Length + TimeLogLength);
        string parsedStepMessage = new StringBuilder()
            .Append('\n')
            .Append(delimitersInRowWithTime)
            .Append('\n')
            .Append(stepMessage)
            .Append('\n')
            .Append(delimiters)
            .ToString();
        logger.Info(parsedStepMessage);
    }

    public static void StepFinish(this Logger logger)
    {
        if(!StepOpened || !Configuration.Log.XmlView)
        {
            return;
        }
        logger.Info("</span>");
        logger.Info("</step>");
        StepOpened = false;
    }

    /// <summary>
    /// Convert <param name="methodName"/> (e.g. "CreateSomethingGood_DoSomethingGreat_HaveARest").
    /// to more readable format (e.g. "Create something good. Do something great. Have a rest.").
    /// It's necessary to log automatically the steps in test classes.
    /// </summary>
    public static string ConvertCamelCaseToText(string methodName)
    {
        StringBuilder parsedMethodName = new();
        bool firstInSentence = true;
        foreach(char character in methodName)
        {
            if(char.IsUpper(character) && !firstInSentence)
            {
                parsedMethodName.Append(' ');
            }
            if(firstInSentence)
            {
                parsedMethodName.Append(character);
                firstInSentence = false;
            }
            else
            {
                parsedMethodName.Append(character.ToString().ToLowerInvariant());
            }
            if(character.Equals('_'))
            {
                firstInSentence = true;
            }
        }
        return parsedMethodName.ToString().Replace("_", ". ");
    }

    private static bool GetStepOpened()
    {
        if(!StepOpenedInstances.IsValueCreated)
        {
            StepOpenedInstances.Value = false;
        }
        return StepOpenedInstances.Value;
    }
}