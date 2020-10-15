using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//public class DragScript : MonoBehaviour, IBeginDragHandler
{
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int WM_NCLBUTTONUP = 0x00A2;
    public const int WM_LBUTTONUP = 0x2020;
    public const int HT_CAPTION = 0x2;
    private const int WM_SYSCOMMAND = 0x112;
    private const int MOUSE_MOVE = 0xF012;
    public const int LeftDown = 0x00000002;


    private delegate void SendAsyncProc(IntPtr hWnd, int uMsg, IntPtr dwData, IntPtr IResult);
    [DllImport("User32.dll")]
    static extern bool ReleaseCapture();
    [DllImport("User32.dll")]
    static extern IntPtr SetCapture(IntPtr hWnd);
    [DllImport("User32.dll")]
    static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [DllImport("User32.dll")]
    static extern System.IntPtr GetActiveWindow();
    [DllImport("User32.dll")]
    static extern IntPtr SetActiveWindow(IntPtr hWnd);
    [DllImport("User32.dll")]
    static extern bool SendMessageCallback(IntPtr hWnd, int Msg, int wParam, int lParam, 
        SendAsyncProc IpCallback, int dwData);
    [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
    static extern IntPtr FindWindowNative(String ClassName, string windowName);
    [DllImport("User32.dll", EntryPoint = "SetForegroundWindow", CharSet = CharSet.Unicode)]
    static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);
    [DllImport("User32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


    public IntPtr FindWindow(string className, string windowName)
    {
        return FindWindowNative(className, windowName);
    }

    public IntPtr SetForegroundWindow(IntPtr hWnd)
    {
        return SetForegroundWindowNative(hWnd);
    }


    static System.IntPtr activeWin = GetActiveWindow();

    public void OnBeginDrag(PointerEventData eventData)
    {
        ReleaseCapture();
        SendAsyncProc WindowDropCallback = new SendAsyncProc(SendMessage_Callback);
        //SendMessageCall instead of SendMessage cuz of the instant callback that we get after triggering the drag
        SendMessageCallback(GetActiveWindow(), WM_SYSCOMMAND, MOUSE_MOVE, 0, WindowDropCallback, 0);



        //SendMessage(GetActiveWindow(), WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        //SendMessage(activeWin, 0x112, 0xF012, 0);
        print("Begin DRAG");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("DRAG");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("END DRAG");
    }

    private void SendMessage_Callback(IntPtr hwnd, int uMsg, IntPtr dwData, IntPtr IResult)
    {
        print("METH");
        ///A lot of things have been tried to regain the focus of the window after the drag click
        ///like; SetForegroundWindow, SetActive, SendMessage with other mouse events,..
        ///but nothing worked, till I decided to use the most stupid method that been in my mind
        ///from the start but chose to ignore it for self explanatory reasons ;)
        //triggering a second click after the drag click(mousedown)
        int X = (int)Input.mousePosition.x;
        int Y = (int)Input.mousePosition.y;
        mouse_event(0x02 | 0x04, X, Y, 0, 0);

        ///keeping the comments for future reference
        //IntPtr hWnd = FindWindow(null, "VC SA");
        //if (hWnd.ToInt32() > 0)
        //{
        //    print("found window");
        //    SetForegroundWindow(hWnd);
        //}

        ////SetCapture(hWnd);

        //ReleaseCapture();
        //SendMessage(hWnd, WM_NCLBUTTONUP, HT_CAPTION, 0);
        //SendMessage(hWnd, WM_LBUTTONUP, HT_CAPTION, 0);

        //SetActiveWindow(hWnd);
        //SendMessage(GetActiveWindow(), 0x112, WM_NCLBUTTONDOWN, 0);
    }

    public void clickUp()
    {
        print("UP");
    }
}
