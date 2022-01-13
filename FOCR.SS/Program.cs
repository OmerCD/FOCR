namespace FOCR.SS;

static class Program
{
    private static GlobalKeyboardHook _globalKeyboardHook;
    private static ScreenShotHandler _screenShotHandler;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        _globalKeyboardHook = new GlobalKeyboardHook();
        _globalKeyboardHook.KeyboardPressed += GlobalKeyboardHook_KeyboardPressed;
        _screenShotHandler = new ScreenShotHandler(@"C:\Program Files (x86)\Steam\userdata\182075523\760\remote\1506830\screenshots");
        Application.Run(new MainForm(_screenShotHandler));
    }

    private static void GlobalKeyboardHook_KeyboardPressed(object? sender, GlobalKeyboardHookEventArgs e)
    {
        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
        {
            switch (e.KeyboardData.VirtualCode)
            {
                case 106:
                    _screenShotHandler.SaveFirstHalf();
                    break;
                case 109:
                    _screenShotHandler.SaveSecondHalf();
                    break;
                case 110:
                    _screenShotHandler.NextMatch();
                    break;
            }
        }
    }
}

//Based on https://gist.github.com/Stasonix