namespace GoogleDocs.Utils;

public class SoftAssert
{
    private static readonly ThreadLocal<SoftAssert?> SoftAssertInstances = new();
    public static SoftAssert Instance => GetSoftAssert();
    public bool HasErrors { get; private set; }
    public int? StepNumber { get; private set; }
    public string? FirstErrorStackTrace { get; set; }
    private List<string> ErrorMessages { get; } = [];

    private SoftAssert()
    {
    }

    public void AddFirstError(int? stepNumber, string stacktrace)
    {
        StepNumber = stepNumber;
        FirstErrorStackTrace = stacktrace;
        HasErrors = true;
    }

    public void AddErrorMessage(string message)
    {
        ErrorMessages.Add(message);
    }

    public string GetErrorMessages()
    {
        string firstStepMessage = StepNumber.HasValue ? $" First error on [Step {StepNumber}]." : string.Empty;
        return ErrorMessages.Count > 1
            ? $"There are {ErrorMessages.Count} errors.{firstStepMessage} Full list of errors:\n{string.Join('\n', ErrorMessages)}"
            : ErrorMessages.First();
    }

    public static void Close()
    {
        SoftAssertInstances.Value = null;
    }

    private static SoftAssert GetSoftAssert()
    {
        return SoftAssertInstances.Value ??= new SoftAssert();
    }
}