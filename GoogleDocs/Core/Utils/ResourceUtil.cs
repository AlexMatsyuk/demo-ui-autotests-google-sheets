using GoogleDocs.Logs;

namespace GoogleDocs.Utils;

public static class ResourceUtil
{
    public static readonly ThreadLocal<string> DownloadFolderInstances = new();
    private static readonly Logger Log = Logger.Instance;

    public static string GetFilePath(params string[] fileOrDirectoryNames)
    {
        List<string> paths = [AppDomain.CurrentDomain.BaseDirectory];
        paths.AddRange(fileOrDirectoryNames);
        string rootPath = Path.Combine(paths.ToArray());
        Log.InfoDebug($"root path: {rootPath}");
        return rootPath;
    }
}