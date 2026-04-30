using GoogleDocs.Logs;
using GoogleDocs.Utils;

namespace GoogleDocs.Test;

public static class Assert
{
    private static readonly Logger Log = Logger.Instance;

    public static void AssertTrue(
        bool condition,
        string passMessage,
        string failMessage,
        bool logAlways = true)
    {
        if (condition)
        {
            Log.InfoDebug(passMessage, logAlways);
            return;
        }
        Log.Error(failMessage);
    }

    public static void AssertEqual<T>(
        List<T> actualValues,
        List<T> expectedValues,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true)
    {
        AssertCount(actualValues.Count, expectedValues.Count, propertyName);
        expectedValues.Select((value, index) => new { value, index })
            .ToList()
            .ForEach(
                x =>
                    AssertTrue(
                        (actualValues[x.index] == null && x.value == null) || actualValues[x.index]!.Equals(x.value),
                        $"{propertyName} #{x.index + 1} is correct: {x.value}",
                        $"{propertyName} #{x.index + 1} is wrong: {actualValues[x.index]}. Should be: {x.value}. {failMessage}",
                        logAlways));
    }

    public static void AssertEqualWithFault(
        List<double> actualValues,
        List<double> expectedValues,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true,
        double fault = 0.0001)
    {
        AssertCount(actualValues.Count, expectedValues.Count, propertyName);
        expectedValues.Select((value, index) => new { value, index })
            .ToList()
            .ForEach(
                x =>
                    AssertTrue(
                        (actualValues[x.index] == 0 && x.value == 0) || (Math.Abs(actualValues[x.index] - x.value) / x.value < fault),
                        $"{propertyName} #{x.index + 1} is correct: {x.value}",
                        $"{propertyName} #{x.index + 1} is wrong: {actualValues[x.index]}. Should be: {x.value}. {failMessage}",
                        logAlways));
    }

    public static void AssertEqualWithFault(
        double? actualValue,
        double? expectedValue,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true,
        double fault = 0.0001)
    {
        if (expectedValue == null)
        {
            AssertNull(actualValue, propertyName);
            return;
        }
        AssertTrue(
            (actualValue == 0 && expectedValue == 0) ||
            (Math.Abs(actualValue!.Value - expectedValue.Value) / Math.Abs(expectedValue.Value) < fault) ||
            (actualValue == 0 && Math.Abs(expectedValue.Value) < fault),
            $"Correct {propertyName}: {actualValue}",
            $"Incorrect {propertyName}: {actualValue}. Should be: {expectedValue}. {failMessage}",
            logAlways);
    }

    /// <summary>
    /// assert a list ignoring an order of elements
    /// </summary>
    public static void AssertEqualIgnoringOrder<T>(
        List<T> actualValues,
        List<T> expectedValues,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true)
    {
        List<T> redundantValues = actualValues.Exclude(expectedValues).ToList();
        List<T> missedValues = expectedValues.Exclude(actualValues).ToList();
        string? redundantValuesMessage = redundantValues.Any() ? "Redundant values are present: \"" + string.Join("\",\"", redundantValues) + "\"." : null;
        string? missedValuesMessage = missedValues.Any() ? "Values are expected, but absent: \"" + string.Join("\",\"", missedValues) + "\"." : null;
        AssertCount(actualValues.Count, expectedValues.Count, propertyName, $"{redundantValuesMessage} {missedValuesMessage}");
        expectedValues.ForEach(
            expectedValue => AssertTrue(
                actualValues.Contains(expectedValue),
                $"{propertyName} '{expectedValue}' is present",
                $"{propertyName} '{expectedValue}' is absent. {failMessage}",
                logAlways));
    }

    public static void AssertEqual<T>(
        T actualPropertyValue,
        T expectedPropertyValue,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true)
    {
        AssertTrue(
            EqualityComparer<T>.Default.Equals(actualPropertyValue, expectedPropertyValue),
            $"Correct {propertyName}: {actualPropertyValue}",
            $"Incorrect {propertyName}: {actualPropertyValue}. Should be: {expectedPropertyValue}. {failMessage}",
            logAlways);
    }

    public static void AssertEqual<TKey, TValue>(
        Dictionary<TKey, TValue> actualValues,
        Dictionary<TKey, TValue> expectedValues,
        string propertyName,
        string? failMessage = null,
        bool? logAlways = null)
        where TKey : notnull
    {
        AssertCount(actualValues.Count, expectedValues.Count, propertyName);
        expectedValues.Select((kvp, index) => new { kvp.Key, kvp.Value, index })
            .ToList()
            .ForEach(
                x =>
                {
                    bool keyExists = actualValues.TryGetValue(x.Key, out var actualValue);
                    AssertTrue(
                        keyExists,
                        $"{propertyName} #{x.index + 1} key '{x.Key}' is present",
                        $"{propertyName} #{x.index + 1} key '{x.Key}' is missing. {failMessage}",
                        logAlways ?? false);
                    if (keyExists)
                    {
                        AssertTrue(
                            (actualValue == null && x.Value == null) || (actualValue?.Equals(x.Value) ?? false),
                            $"{propertyName} #{x.index + 1} [{x.Key}] is correct: {x.Value}",
                            $"{propertyName} #{x.index + 1} [{x.Key}] is wrong: {actualValue}. Should be: {x.Value}. {failMessage}");
                    }
                });
    }

    public static void AssertNull<T>(T value, string propertyName, bool logAlways = true)
    {
        AssertTrue(value == null, $"{propertyName} is null", $"{propertyName} isn't null: {value}", logAlways);
    }

    public static void AssertNotNull<T>(
        T value,
        string propertyName,
        bool logAlways = true,
        string? errorMessage = null)
    {
        AssertTrue(value != null, $"{propertyName} isn't null", errorMessage ?? $"{propertyName} is null", logAlways);
    }

    public static void AssertNotNullOrEmpty(string value, string propertyName)
    {
        AssertTrue(!string.IsNullOrEmpty(value), $"{propertyName} isn't null or empty: {value}", $"{propertyName} is null or empty");
    }

    public static void AssertCount(
        int? actualCount,
        int? expectedCount,
        string propertyName,
        string? failMessage = null,
        bool logAlways = true)
    {
        AssertTrue(
            actualCount == expectedCount,
            $"correct count of {propertyName}: {actualCount}",
            $"incorrect count of {propertyName}: {actualCount}. Should be: {expectedCount}. {failMessage}",
            logAlways);
    }
}