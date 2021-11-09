using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

public static class WindowsAPI
{

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
    public static extern bool PrintWindow(IntPtr handle, IntPtr hDC, uint flags);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr handle);
    [DllImport("user32.dll")]
    public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr dc);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
      IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

    public static Bitmap CaptureTopLevelWindow(IntPtr handle)
    {
        WindowInfo wi = new WindowInfo();
        wi.cbSize = Marshal.SizeOf(wi);
        GetWindowInfo(handle, ref wi);
        
        int width = wi.rcWindow.right - wi.rcWindow.left;
        int height = wi.rcWindow.top - wi.rcWindow.bottom;

        Bitmap img = new Bitmap(width, height);
        Graphics gra = Graphics.FromImage(img);
        IntPtr hdc = gra.GetHdc();
        PrintWindow(handle, hdc, 0);
        gra.ReleaseHdc(hdc);
        gra.Dispose();

        return img;
    }
    public static Bitmap CaptureWindowControls(IntPtr handle)
    {
        WindowInfo wi = new WindowInfo();
        GetWindowInfo(handle, ref wi);

        IntPtr winDC = GetWindowDC(handle);

        int width = wi.rcWindow.right - wi.rcWindow.left;
        int height = wi.rcWindow.top - wi.rcWindow.bottom;

        Bitmap img = new Bitmap(width, height);
        Graphics gra = Graphics.FromImage(img);

        IntPtr hDC = gra.GetHdc();

        BitBlt(hDC, 0, 0, img.Width, img.Height, winDC, 0, 0, TernaryRasterOperations.SRCCOPY);

        gra.ReleaseHdc(hDC);
        gra.Dispose();

        ReleaseDC(handle, winDC);

        return img;
    }

    enum TernaryRasterOperations : uint
    {
        SRCCOPY = 0x00CC0020,
        SRCPAINT = 0x00EE0086,
        SRCAND = 0x008800C6,
        SRCINVERT = 0x00660046,
        SRCERASE = 0x00440328,
        NOTSRCCOPY = 0x00330008,
        NOTSRCERASE = 0x001100A6,
        MERGECOPY = 0x00C000CA,
        MERGEPAINT = 0x00BB0226,
        PATCOPY = 0x00F00021,
        PATPAINT = 0x00FB0A09,
        PATINVERT = 0x005A0049,
        DSTINVERT = 0x00550009,
        BLACKNESS = 0x00000042,
        WHITENESS = 0x00FF0062,
        CAPTUREBLT = 0x40000000
    }
}


