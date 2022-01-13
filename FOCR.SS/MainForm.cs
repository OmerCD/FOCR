using System.Windows.Forms;

namespace FOCR.SS;

public partial class MainForm : Form
{
    private readonly ScreenShotHandler _screenShotHandler;

    public MainForm(ScreenShotHandler screenShotHandler)
    {
        _screenShotHandler = screenShotHandler;
        _screenShotHandler.OnScreenshotTaken += ScreenShotHandlerOnOnScreenshotTaken;
        _screenShotHandler.OnMatchCountWillChange += ScreenShotHandlerOnOnMatchCountWillChange;
        _screenShotHandler.OnMatchCountChanged += ScreenShotHandlerOnOnMatchCountChanged;
        InitializeComponent();
        _matchCountLabel.Text = _screenShotHandler.GetMatchCount().ToString();
    }

    private void ScreenShotHandlerOnOnMatchCountChanged(int newCount)
    {
        _matchCountLabel.ForeColor = Color.White;
        string SetTextDelegate() => _matchCountLabel.Text = newCount.ToString();
        if (_matchCountLabel.InvokeRequired)
        {
            
            Invoke((Func<string>) SetTextDelegate);
        }
        else
        {
            SetTextDelegate();
        }
    }

    private void ScreenShotHandlerOnOnMatchCountWillChange()
    {
        _matchCountLabel.ForeColor = Color.Red;
    }

    private void ScreenShotHandlerOnOnScreenshotTaken(string screenShotPath)
    {
        _pictureBox.ImageLocation = screenShotPath;
    }
}