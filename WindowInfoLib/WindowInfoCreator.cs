using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WindowInfoCreator
{
    List<WindowInfo> windowInfos;
    int currentHierarchy;
    int parentPosTop;
    int parentPosLeft;

    public List<WindowInfo> GetWindowInfo(IntPtr parentHandle)
    {
        ClearGarbage();

        WindowInfo wi = new WindowInfo();
        WindowsAPI.GetWindowInfo(parentHandle, ref wi);

        wi.handle = parentHandle;
        wi.img = WindowsAPI.CaptureTopLevelWindow(parentHandle);
        wi.hierarchy = currentHierarchy;
        wi.relativePosLeft = 0;
        wi.relativePosTop = 0;

        parentPosTop = wi.rcWindow.top;
        parentPosLeft = wi.rcWindow.left;

        windowInfos.Add(wi);

        currentHierarchy++;

        WindowsAPI.EnumChildWindows(parentHandle, GetChildren, IntPtr.Zero);

        return windowInfos;
    }

    private bool GetChildren(IntPtr handle, IntPtr lparam)
    {
        WindowInfo wi = new WindowInfo();
        WindowsAPI.GetWindowInfo(handle, ref wi);

        wi.handle = handle;
        wi.img = WindowsAPI.CaptureWindowControls(handle);
        wi.hierarchy = currentHierarchy;
        wi.relativePosTop = wi.rcWindow.top - parentPosTop;
        wi.relativePosLeft = wi.rcWindow.left - parentPosLeft;

        windowInfos.Add(wi);

        currentHierarchy++;
        WindowsAPI.EnumChildWindows(handle, GetChildren, IntPtr.Zero);
        currentHierarchy--;

        return true;

    }

    private void ClearGarbage()
    {
        windowInfos = new List<WindowInfo>();
        currentHierarchy = 0;
        parentPosTop = 0;
        parentPosLeft = 0;
    }
}

