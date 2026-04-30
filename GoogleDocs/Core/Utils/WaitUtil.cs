using GoogleDocs.Configurations;
using GoogleDocs.Logs;
using GoogleDocs.Test;

namespace GoogleDocs.Utils;

public static class WaitUtil
{
    /// <summary>
    /// Calls given method repeatedly with given <param name="pollingIntervalSeconds"/>
    /// until it return 'true' or until <param name="timeoutSeconds"/> is over.
    /// This method and its derivatives should be used instead of WebDriverWaitUtil.WaitForCondition in non-selenium tests,
    /// in cases when the browser can be not opened.
    /// </summary>
    /// <param name="condition">Method to check the condition.</param>
    /// <param name="pollingIntervalSeconds">Default value is <see cref="WaitConfiguration.PollingInterval"/> seconds.</param>
    public static void WaitForCondition(
        Func<bool> condition,
        double timeoutSeconds,
        string conditionDescription,
        double pollingIntervalSeconds,
        Action? additionalAction = null,
        bool logAlways = true,
        string? additionalFailMessage = null,
        bool failIfNotSucceeded = true)
    {
        TimeSpan timeout = TimeSpan.FromSeconds(timeoutSeconds);
        TimeSpan pollingInterval = TimeSpan.FromSeconds(pollingIntervalSeconds);
        DateTime startTime = DateTime.UtcNow;
        bool conditionAchieved = condition();
        bool shouldStop = conditionAchieved;
        while (!shouldStop)
        {
            conditionAchieved = condition();
            shouldStop = conditionAchieved || DateTime.UtcNow.Subtract(startTime) >= timeout;
            if (shouldStop)
            {
                continue;
            }
            additionalAction?.Invoke();
            Thread.Sleep(pollingInterval);
        }
        if (failIfNotSucceeded)
        {
            Assert.AssertTrue(
                conditionAchieved,
                $"Condition '{conditionDescription}' was achieved",
                $"Condition '{conditionDescription}' wasn't achieved in {timeoutSeconds} seconds. " + additionalFailMessage,
                logAlways);
        }
        Logger.Instance.InfoDebug($"wait for the condition was {DateTime.UtcNow.Subtract(startTime):ss\\.ff} seconds");
    }

    public static void WaitForCondition(
        Func<bool> condition,
        double timeoutSeconds,
        string conditionDescription,
        bool logAlways = true)
    {
        WaitForCondition(condition, timeoutSeconds, conditionDescription, Configuration.Wait.PollingInterval, null, logAlways);
    }

    public static void WaitForCondition(Func<bool> condition, string conditionDescription, bool logAlways = true)
    {
        WaitForCondition(condition, Configuration.Wait.DefaultTimeout, conditionDescription, Configuration.Wait.PollingInterval, null, logAlways);
    }
}