using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TitlebarHider : MonoBehaviour
{
	#region dllStuff

	private const int GWL_STYLE = -16;
	private const int GWL_EXSTYLE = -20;
	private const int WS_EX_DLGMODALFRAME = 0x1;
	private const int SWP_FRAMECHANGED = 0x20;
	private const int WS_BORDER = 0x00800000;
	private const int WS_DLGRFRAME = 0x0040000;             //window with double border but no title
	private const int WS_SYSMENU = 0x00080000;              //window with no border
	private const int WS_CAPTION = WS_BORDER | WS_DLGRFRAME;//window with a title bar
	private const int WS_THICKFRAME = 262144;
	private const int WS_MINIMIZE = 536870912;
	private const int WS_MAXIMIZEBOX = 65536;
	private const int SWP_SIZEBOX = 0x00040000;
	private const int SWP_NOSIZE = 0x0001;                  //don't resize the window flag
	private const int SWP_SHOWWINDOW = 0x40;                //show the window flag
	private const int SWP_NOMOVE = 0x0002;                  //don't move the window flag
	private const int WS_VISIBLE = 0x10000000;

	[DllImport("user32.dll")]
	private static extern System.IntPtr GetActiveWindow();

	[DllImport("user32.dll")]
	private static extern System.IntPtr SetWindowLong(
		System.IntPtr hWnd,
		int nIndex,
		int dwNewLong
		);

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(
		System.IntPtr hWnd,
		int nIndex
		);

	[DllImport("user32.dll")]
	private static extern System.IntPtr SetWindowPos(
		System.IntPtr hWnd,
		System.IntPtr hWndInsertAfter,
		int x,
		int y,
		int cx,
		int cy,
		int uFlags
		);

	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

	private struct Rect
	{
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }
	}

	private static System.IntPtr hWnd;
	private static System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
	private static System.IntPtr HWND_TOP = new System.IntPtr(0);

	#endregion dllStuff

	public static void showWindowsBorder()
	{
		if (Application.isEditor) return;
		int style = GetWindowLong(GetActiveWindow(), GWL_STYLE);
		style = style & ~WS_CAPTION;
		style = style & ~WS_SYSMENU;
		style = style & ~WS_THICKFRAME;
		style = style & ~WS_MINIMIZE;
		style = style & ~WS_MAXIMIZEBOX;
		Rect rect = new Rect();

		SetWindowLong(GetActiveWindow(), GWL_STYLE, WS_VISIBLE);
		SetWindowPos(GetActiveWindow(), HWND_TOP, rect.Left, rect.Top, 250, 300, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_NOMOVE);//getting called to apply the changes that setWindowLong did

		//SetWindowLong(GetActiveWindow(), GWL_STYLE, style);
		//style = GetWindowLong(GetActiveWindow(), GWL_STYLE);
		///SetWindowLong(GetActiveWindow(), GWL_EXSTYLE, style | WS_EX_DLGMODALFRAME);
		//SetWindowLong(GetActiveWindow(), GWL_EXSTYLE, style | WS_VISIBLE);
		//SetWindowPos(GetActiveWindow(), new IntPtr(0), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);

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