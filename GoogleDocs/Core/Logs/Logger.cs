using System.Xml;
using GoogleDocs.Configurations;
using GoogleDocs.Test;
using GoogleDocs.Utils;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace GoogleDocs.Logs;

public class Logger
{
    public static readonly object Locker = new();
    private static readonly DateTime StartTime = DateTime.Now;
    public static readonly ThreadLocal<List<Action>> LogActionsInstances = new(() => []);
    public static readonly ThreadLocal<bool> TestFailed = new(() => false);
    public static List<Action>? LogActions => LogActionsInstances.Value;
    public static readonly Logger Instance = new();
    private readonly ILog _log;
    private static SoftAssert SoftAssert => SoftAssert.Instance;
    private static int _testsCount;
    private static int _failedTestsCount;
    private static int _skippedTestsCount;
    private static int _passedTestsCount;
    public const string BugsSplitter = ", ";

    private Logger()
    {
        XmlDocument log4NetConfig = new();
        FileStream fileStream = File.OpenRead("log4net.config");
        log4NetConfig.Load(fileStream);
        ILoggerRepository repo = LogManager.CreateRepository(typeof(Logger).Assembly, typeof(Hierarchy));
        XmlConfigurator.Configure(repo, log4NetConfig["log4net"]!);
        _log = LogManager.GetLogger(GetType());
        fileStream.Dispose();
    }

    public void Info(string message)
    {
        string logMessage = message.GetLogMessage();
        LogActions?.Add(() => _log.Info(logMessage));
    }

    /// <summary>
    /// Add error.
    /// Stop test immediately, if SoftAssert disabled.
    /// Continue test if SoftAssert enabled, but fail in the end, and collect all the errors.
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message)
    {
        TestFailed.Value = true;
        if (!Configuration.Assert.SoftAssertEnabled)
        {
            Fail(message);
            return;
        }
        string logMessage = ((Configuration.Log.XmlView ? "<error/>" : "!!! ") + $"Error: {message}").GetLogMessage();
        LogActions?.Add(() => _log.Error(logMessage));
        if (!SoftAssert.HasErrors)
        {
            SoftAssert.AddFirstError(BaseTest.StepNumber, Environment.StackTrace);
        }
        SoftAssert.AddErrorMessage($"Step {BaseTest.StepNumber}: {message}");
    }

    public void InfoDebug(string message, bool logAlways = false)
    {
        if (logAlways || Configuration.Log.DebugLevel)
        {
            Info(message);
        }
    }

    /// <summary>
    /// Throw error and stop test immediately (even if SoftAssert is enabled).
    /// </summary>
    public Exception Fail(string message)
    {
        TestFailed.Value = true;
        string logMessage = ((Configuration.Log.XmlView ? "<error/>" : "!!! ") + $"Error: {message}").GetLogMessage();
        LogActions?.Add(() => _log.Error(logMessage));
        throw new Exception(message);
    }

    public void Bug(int ticketNumber, string additionalInfo, Action? action = null)
    {
        string bugUrl = string.IsNullOrEmpty(Configuration.TestTracking.BugUrlTemplate) 
            ? ticketNumber.ToString()
            : string.Format(Configuration.TestTracking.BugUrlTemplate, ticketNumber);
        Info($"Step is wrong due to issue: {bugUrl}. {additionalInfo}. {action?.Method.Name}");
    }
    
    public void SubmitLogsInTheEndOfTest()
    {
        int finishedTests;
        int failedTests;
        int skippedTests;
        int passedTests;
        bool testFailed = TestFailed.Value || TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        lock (Locker)
        {
            finishedTests = ++_testsCount;
            failedTests = _failedTestsCount;
            skippedTests = _skippedTestsCount;
            passedTests = _passedTestsCount;
            if (testFailed)
            {
                failedTests = ++_failedTestsCount;
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped)
            {
                skippedTests = ++_skippedTestsCount;
            }
            else
            {
                passedTests = ++_passedTestsCount;
            }
        }
        if (Configuration.Log.LogPassedTests || testFailed || TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped)
        {
            LogActions?.ForEach(logAction => logAction.Invoke());
        }
        LogActions?.Clear();
        TestFailed.Value = false;
        string runningTime = (DateTime.Now - StartTime).ToString(@"hh\:mm\:ss");
        _log.Info($"Failed: {failedTests}. Passed: {passedTests}. Ignored: {skippedTests}. Total tests: {finishedTests}. Total time: {runningTime}.");
    }
}