using GoogleDocs.Configurations;
using GoogleDocs.Ui;

namespace Forms.HotKeys;

public static class HotKeysFactory
{
    private static readonly Lock Lock = new();

    public static IHotKeys Instance
    {
        get
        {
            if (field != null)
            {
                return field;
            }
            lock (Lock)
            {
                return field ??= Create(Configuration.Browser.OsType);
            }
        }
    }

    public static IHotKeys Create(OsType osType)
    {
        return osType switch
        {
            OsType.Windows => new WindowsHotKeys(),
            OsType.Mac => new MacHotKeys(),
            _ => new WindowsHotKeys()
        };
    }
}
