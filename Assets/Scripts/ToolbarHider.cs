using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ToolbarHider : MonoBehaviour
{

    #region dllStuff
    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;
    const int WS_EX_DLGMODALFRAME = 0x1;
    const int SWP_FRAMECHANGED = 0x20;
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
        style = style & ~WS_CAPTION;
        style = style & ~WS_SYSMENU;
        style = style & ~WS_THICKFRAME;
        style = style & ~WS_MINIMIZE;
        style = style & ~WS_MAXIMIZEBOX;


        SetWindowLong(hWnd, GWL_STYLE, (uint)WS_VISIBLE);
        //SetWindowLong(hWnd, GWL_STYLE, (uint)style);
        //style = GetWindowLong(hWnd, GWL_STYLE).ToInt32();
        //SetWindowLong(hWnd, GWL_EXSTYLE, (uint)style | WS_EX_DLGMODALFRAME);
        //SetWindowPos(hWnd, new IntPtr(0), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
        SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_NOMOVE);//getting called to apply the changes that setWindowLong did

        ///not working code for transparency 
        //AccentPolicy accent = new AccentPolicy();
        //accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
        //accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;
        //int accentStructSize = Marshal.SizeOf(accent);
        //IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
        //Marshal.StructureToPtr(accent, accentPtr, false);
        //WindowCompositionAttributeData data = new WindowCompositionAttributeData();
        //data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
        //data.SizeOfData = accentStructSize;
        //data.Data = accentPtr;
        //SetWindowCompositionAttribute(GetActiveWindow(), ref data);
        //Marshal.FreeHGlobal(accentPtr);
        }

    public static void getActiveWindow()
    {
        hWnd = GetActiveWindow();
    }

    //[DllImport("user32.dll")]
    //internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    //[StructLayout(LayoutKind.Sequential)]
    //internal struct WindowCompositionAttributeData
    //{
    //    public WindowCompositionAttribute Attribute;
    //    public IntPtr Data;
    //    public int SizeOfData;
    //}

    //internal enum WindowCompositionAttribute
    //{
    //    WCA_ACCENT_POLICY = 19
    //}
    //internal enum AccentState
    //{
    //    ACCENT_DISABLED = 0,
    //    ACCENT_ENABLE_GRADIENT = 1,
    //    ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
    //    ACCENT_ENABLE_BLURBEHIND = 3,
    //    ACCENT_INVALID_STATE = 4
    //}
    //[StructLayout(LayoutKind.Sequential)]
    //internal struct AccentPolicy
    //{
    //    public AccentState AccentState;
    //    public int AccentFlags;
    //    public int GradientColor;
    //    public int AnimationId;
    //}

}
