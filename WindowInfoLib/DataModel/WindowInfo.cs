using System.Runtime.InteropServices;
using System;
using System.Drawing;

[StructLayout(LayoutKind.Sequential)]
public struct WindowInfo
{

    public int cbSize;
    public Rect rcWindow;
    public Rect rcClient;
    public int dwStyle;
    public int dwExStyle;
    public int dwWindowStatus;
    public uint cxWindowBorders;
    public uint cyWindowBorders;
    public short atomWindowType;
    public short wCreatorVersion;

    public IntPtr handle;
    public Bitmap img;
    public int hierarchy;
    public int relativePosTop;
    public int relativePosLeft;

}
