using System.ComponentModel;

namespace FOCR.SS;

class GlobalKeyboardHookEventArgs : HandledEventArgs
{
    public GlobalKeyboardHook.KeyboardState KeyboardState { get; private set; }
    public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

    public GlobalKeyboardHookEventArgs(
        GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData,
        GlobalKeyboardHook.KeyboardState keyboardState)
    {
        KeyboardData = keyboardData;
        KeyboardState = keyboardState;
    }
}