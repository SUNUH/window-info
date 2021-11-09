using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

public static class WindowsAPI
{

    private const int SRCCOPY = 13369376;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowTextLength(IntPtr handle);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr handle, StringBuilder lpString, int nMaxCount);


    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowInfo(IntPtr handle, ref WindowInfo wi);

    public delegate bool EnumWindowsDelegate(IntPtr handle, IntPtr lparam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(IntPtr handle, EnumWindowsDelegate enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PrintIWindow(IntPtr handle, IntPtr hDC, uint flags);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr handle);
    [DllImport("user32.dll")]
    public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr dc);

    [DllImport("user32.dll")]
    private static extern int BitBlt(
        IntPtr hDestDC,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hSrcDC,
        int xSrc,
        int ySrc,
        int dwRop
        );

    public static Bitmap CaptureTopLevelWindow(IntPtr handle)
    {
        WindowInfo wi = new WindowInfo();
        GetWindowInfo(handle, ref wi);

        int width = wi.rcWindow.right - wi.rcWindow.left;
        int height = wi.rcWindow.bottom - wi.rcWindow.top;

        Bitmap img = new Bitmap(width, height);
        Graphics gra = Graphics.FromImage(img);
        IntPtr hdc = gra.GetHdc();
        PrintIWindow(handle, hdc, 0);
        gra.ReleaseHdc(hdc);
        gra.Dispose();

        return img;
    }
    public static Bitmap CaptureWindowControls(IntPtr handle)
    {
        WindowInfo wi = new WindowInfo();
        GetWindowInfo(handle, ref wi);

        IntPtr winDC = GetWindowDC(handle);

        int width = wi.rcWindow.right = wi.rcWindow.left;
        int height = wi.rcWindow.bottom = wi.rcWindow.top;

        Bitmap img = new Bitmap(width, height);
        Graphics gra = Graphics.FromImage(img);

        IntPtr hDC = gra.GetHdc();

        BitBlt(hDC, 0, 0, img.Width, img.Height, winDC, 0, 0, SRCCOPY);

        gra.ReleaseHdc(hDC);
        gra.Dispose();

        ReleaseDC(handle, winDC);

        return img;
    }
}


