using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public int left;
    public int top;
    public int right;  
    public int bottom; 
    public int width { get { return right - left; } }
    public int height { get { return bottom - top; } }
}
