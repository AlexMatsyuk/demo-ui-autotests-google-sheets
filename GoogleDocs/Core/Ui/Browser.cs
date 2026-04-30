using System.Diagnostics;
using GoogleDocs.Configurations;
using GoogleDocs.Logs;
using GoogleDocs.Utils;
using Microsoft.Playwright;

namespace GoogleDocs.Ui;

public class Browser : IDisposable
{
    private static Logger Log => Logger.Instance;
    private static readonly ThreadLocal<Browser> BrowserInstances = new();
    private static readonly Lazy<IPlaywright> PlaywrightInstance = new(() =>
    {
        EnsureBrowsersInstalled();
        return Playwright.CreateAsync().GetAwaiter().GetResult();
    });
    private static readonly Lock InstallLock = new();
    private static bool _browsersInstalled;

    private readonly Logger _log = Logger.Instance;
    private readonly BrowserConfiguration _config;

    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    private bool _disposed;

    public static Browser Instance
    {
        get
        {
            BrowserInstances.Value ??= new Browser();
            return BrowserInstances.Value;
        }
    }

    public IPage Page => _page ?? throw new InvalidOperationException("Browser page is not initialized. Call OpenUrl or OpenStartUrl first.");

    private Browser()
    {
        _config = Configuration.Browser;
        InitializeBrowser();
    }

    private static void EnsureBrowsersInstalled()
    {
        if (_browsersInstalled)
        {
            return;
        }
        lock (InstallLock)
        {
            if (_browsersInstalled) return;

            var browserType = Configuration.Browser.BrowserType;
            var browserName = browserType switch
            {
                BrowserType.Firefox => "firefox",
                _ => "chromium"
            };

            Log.InfoDebug($"Installing Playwright browser '{browserName}'...");
            var exitCode = Program.Main(["install", browserName]);
            if (exitCode != 0)
            {
                throw Log.Fail($"Failed to install Playwright browser '{browserName}'. Exit code: {exitCode}");
            }
            Log.InfoDebug($"Playwright browser '{browserName}' is installed successfully");

            _browsersInstalled = true;
        }
    }

    private void InitializeBrowser()
    {
        var playwright = PlaywrightInstance.Value;

        var launchArgs = new List<string> { "--lang=en-US" };
        if (!_config.Headless)
        {
            launchArgs.Add("--start-maximized");
        }

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = _config.Headless,
            Args = launchArgs
        };

        _browser = _config.BrowserType switch
        {
            BrowserType.Chrome => playwright.Chromium.LaunchAsync(launchOptions).GetAwaiter().GetResult(),
            BrowserType.Firefox => playwright.Firefox.LaunchAsync(launchOptions).GetAwaiter().GetResult(),
            _ => throw new NotSupportedException($"Browser type '{_config.BrowserType}' is not supported")
        };

        var contextOptions = new BrowserNewContextOptions
        {
            // ViewportSize = null allows the browser to use its actual window size (for maximized window)
            // In headless mode, set explicit size since there's no physical window
            ViewportSize = _config.Headless
                ? new ViewportSize { Width = _config.HeadlessWindowWidth, Height = _config.HeadlessWindowHeight }
                : ViewportSize.NoViewport,
            // DeviceScaleFactor is only supported when viewport is set
            DeviceScaleFactor = _config.Headless ? (float)_config.ScaleFactor : null,
            // Set locale and language to English (US) to ensure consistent UI language
            Locale = "en-US",
            TimezoneId = "America/New_York",
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Accept-Language"] = "en-US,en;q=0.9"
            }
        };

        _context = _browser.NewContextAsync(contextOptions).GetAwaiter().GetResult();
        _page = _context.NewPageAsync().GetAwaiter().GetResult();

        _log.InfoDebug($"Browser initialized: Type={_config.BrowserType}, Headless={_config.Headless}");
    }

    public void OpenUrl(string url)
    {
        if (_page == null)
        {
            throw Log.Fail("Browser page is not initialized");
        }

        _page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = (float)(Configuration.Wait.PageLoadTimeout * 1000)
        }).GetAwaiter().GetResult();

        _log.Info($"Open URL: {url}");
    }

    public void OpenStartUrl()
    {
        if (string.IsNullOrEmpty(_config.StartUrl))
        {
            Logger.Instance.InfoDebug("StartUrl is not configured in BrowserConfiguration");
            return;
        }
        OpenUrl(_config.StartUrl);
    }

    public string GetUrl()
    {
        return Page.Url;
    }
    
    public void Type(string text, bool pressEnter = false)
    {
        Log.Info($"Type: {text}");
        Page.Keyboard.TypeAsync(text).GetAwaiter().GetResult();
        if(pressEnter)
        {
            PressKey("Enter");
        }
    }

    public void PressKey(string key)
    {
        Log.Info($"Press key: {key}");
        Page.Keyboard.PressAsync(key).GetAwaiter().GetResult();
    }

    public void CaptureScreenshot(string fileName)
    {
        if (_page == null)
        {
            _log.InfoDebug("Cannot capture screenshot: page is not initialized");
            return;
        }

        try
        {
            string screenshotDir = ResourceUtil.GetFilePath("Screenshots");
            Directory.CreateDirectory(screenshotDir);

            string filePath = Path.Combine(screenshotDir, $"{fileName}.png");

            _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = filePath,
                FullPage = true
            }).GetAwaiter().GetResult();

            _log.Info($"Screenshot saved: {filePath}");
        }
        catch (Exception ex)
        {
            _log.InfoDebug($"Failed to capture screenshot: {ex.Message}");
        }
    }

    public void Close()
    {
        Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            try
            {
                _page?.CloseAsync().GetAwaiter().GetResult();
                _context?.CloseAsync().GetAwaiter().GetResult();
                _browser?.CloseAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _log.InfoDebug($"Error during browser cleanup: {ex.Message}");
            }
            finally
            {
                _page = null;
                _context = null;
                _browser = null;
                BrowserInstances.Value = null!;
            }
        }

        _disposed = true;
        _log.Info("Browser closed");
    }

    ~Browser()
    {
        Dispose(false);
    }
}
