using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ToolbarHider : MonoBehaviour
{

    #region dllStuff
    const int GWL_STYLE = -16;
    const int WS_BORDER = 0x00800000;
    const int WS_DLGRFRAME = 0x0040000;             //window with double border but no title
    const int WS_SYSMENU = 0x00080000;              //window with no border
    const int WS_CAPTION = WS_BORDER | WS_DLGRFRAME;//window with a title bar
    const int WS_THICKFRAME = 262144;
    const int WS_MINIMIZE = 536870912;
    const int WS_MAXIMIZEBOX = 65536;
    const int SWP_SIZEBOX = 0x00040000;
    const int SWP_NOSIZE = 0x0001;                  //don't resize the window flag
    const int SWP_SHOWWINDOW = 0x40;                //show the window flag
    const int SWP_NOMOVE = 0x0002;                  //don't move the window flag
    const int WS_VISIBLE = 0x10000000;

    [DllImport("user32.dll")]
    static extern System.IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern System.IntPtr SetWindowLong(
        System.IntPtr hWnd,
        int nIndex,
        uint dwNewLong
        );
    [DllImport("user32.dll")]
    static extern System.IntPtr GetWindowLong(
        System.IntPtr hWnd,
        int nIndex
        );
    [DllImport("user32.dll")]
    static extern System.IntPtr SetWindowPos(
        System.IntPtr hWnd,
        System.IntPtr hWndInsertAfter,
        short x,
        short y,
        short cx,
        short cy,
        uint uFlags
        );

    static System.IntPtr hWnd;
    static System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
    static System.IntPtr HWND_TOP = new System.IntPtr(0);

    #endregion

    public static void showWindowsBorder()
    {
        if (Application.isEditor) return;
        int style = GetWindowLong(hWnd, GWL_STYLE).ToInt32();

        SetWindowLong(hWnd, GWL_STYLE, (uint)WS_VISIBLE);
        SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_NOMOVE);//getting called to apply the changes that setWindowLong did
    }

    public static void getActiveWindow()
    {
        hWnd = GetActiveWindow();
    }

}
