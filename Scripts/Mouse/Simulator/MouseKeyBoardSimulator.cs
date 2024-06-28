using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UnityEngine;

public class MouseKeyBoardSimulator : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, int dwFlags, UIntPtr dwExtraInfo);
    //下面这个枚举也来自user32.dll
    [Flags]
    public enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }
    public enum MouseKey { 
        Left,
        right,
        scroll
    }

    


    bool hasMouseDown = false;

    void Start()
    {
        //模拟键盘输入
        SendKeys.Send("{BACKSPACE}");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            Debug.Log("backspace is pressed");
        }

        if (!hasMouseDown)
        {
            if (Time.time > 2)
            {
                //模拟鼠标在一个按钮上点击，这个按钮会调用下面的CloseSelf()方法
                SendMouseClick(20, 20, MouseKeyBoardSimulator.MouseKey.Left);
                hasMouseDown = true;
            }
        }
    }


    public static void SendMouseClick(int x,int y , MouseKey key)
    {
        SetCursorPos(x, y);
        switch (key)
        {
            case MouseKey.Left:
                mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
                mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
                break;
            case MouseKey.right:
                mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
                mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
                break;
            case MouseKey.scroll:
                //暂未实现
                mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
                mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
                break;
            default:
                break;
        }
    }

    public static void SendKeyDown(Keys k) {
        keybd_event((byte)k, 0, 0, UIntPtr.Zero);
        keybd_event((byte)k, 0, 0x2, UIntPtr.Zero);
    }

    public void CloseSelf()
    {
        UnityEngine.Application.Quit();
    }
}
