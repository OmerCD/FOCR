using System.Drawing.Imaging;

namespace FOCR.SS;

public class ScreenShotHandler
{
    private readonly string _basePath;
    private bool _matchTracker;
    public event Action<string> OnScreenshotTaken;
    public event Action<int> OnMatchCountChanged;
    public event Action OnMatchCountWillChange;

    public ScreenShotHandler(string basePath)
    {
        _basePath = basePath;
    }

    public void NextMatch()
    {
        _matchTracker = true;
        OnMatchCountWillChange();
    }
    public Task SaveFirstHalf()
    {
        return SaveHalf("1");
    }

    public Task SaveSecondHalf()
    {
        return SaveHalf("2");
    }
    public int GetMatchCount()
    {
        return Directory.GetDirectories(DatePathCheck()).Length;
    }

    private Task SaveHalf(string halfName)
    {
        return Task.Run(() =>
        {
            var datePath = DatePathCheck();
            var matchPath = MatchPathCheck(datePath);
            var firstHalfPath = HalfPathCheck(matchPath, halfName);
            var screenShot = GetScreenShot();
            var filename = Path.Combine(firstHalfPath, $"{Guid.NewGuid().ToString()}.jpg");
            screenShot.Save(filename, ImageFormat.Jpeg);
            OnScreenshotTaken(filename);
        });
    }

    private string MatchPathCheck(string datePath)
    {
        var directories = Directory.GetDirectories(datePath);
        var newMatchCount = directories.Length;
        if (_matchTracker || directories.Length == 0)
        {
            newMatchCount++;
            _matchTracker = false;
            OnMatchCountChanged(newMatchCount);
        }

        return Path.Combine(datePath, newMatchCount.ToString());
    }

    private string DatePathCheck()
    {
        var today = TodayText;
        var datePath = Path.Combine(_basePath, today);
        if (!Directory.Exists(datePath))
        {
            Directory.CreateDirectory(datePath);
        }

        return datePath;
    }

    private static string HalfPathCheck(string basePath, string halfName)
    {
        var halfPath = Path.Combine(basePath, halfName);
        if (!Directory.Exists(halfPath))
        {
            Directory.CreateDirectory(halfPath);
        }

        return halfPath;
    }

    private static string TodayText => DateTime.Today.ToString("yyyy-MM-dd");

    private static Bitmap GetScreenShot()
    {
        Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        Graphics graphics = Graphics.FromImage(bitmap);

        graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

        return bitmap;
    }
}