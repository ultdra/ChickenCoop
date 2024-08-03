using Godot;
using System;
using System.Runtime.InteropServices;

public partial class CoopSceneManager : Node2D
{
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const uint LWA_COLORKEY = 0x1;

    public override void _Ready()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize();
        screenSize += new Vector2I(1, 1);  // Subtract 1 pixel from width and height
        GetTree().Root.ContentScaleSize = screenSize;

        GetTree().Root.TransparentBg = true;
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Transparent, true, 0);
        GetWindow().MousePassthrough = true;

        //Fix to remove white lines from the fullscreen border
        GetWindow().Mode = Window.ModeEnum.Maximized;
        GetWindow().Borderless = true;
        GetWindow().Mode = Window.ModeEnum.Maximized;
        GetWindow().Borderless = true;

        // Apply click-through for transparent areas (Windows-specific)
        if (OperatingSystem.IsWindows())
        {
            IntPtr hwnd = new IntPtr(DisplayServer.WindowGetNativeHandle(DisplayServer.HandleType.WindowHandle));
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
            SetLayeredWindowAttributes(hwnd, 0, 255, LWA_COLORKEY);
        }

    }
}
