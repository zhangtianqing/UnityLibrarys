using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

///
///单窗口最大化全屏去边框
/// 挂载，运行，修改默认分辨率
///
public class WindowMod_SingleScreen : MonoBehaviour
{
    public Rect screenRect;
    [DllImport("User32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    ///不能被移动和改变大小
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 1;

    private const int defaultWidth = 7680;
    private const int defaultHeight = 1200;
    private IntPtr handler;

    private void Start()
    {
#if !UNITY_EDITOR     
        ///获取当前名称
        string name = Application.productName;
        ///获取句柄
        handler = FindWindow(null, name);
        ///获取失败的话要处理
        if (handler == IntPtr.Zero){
            ///因为程序是新运行的程序，肯定是在最前面的
            handler = GetForegroundWindow();
        }
        ///将创建Unity窗口的线程设置到前台
        SetForegroundWindow(handler);
        ///改变指定窗口的属性 样式 和 边框
        SetWindowLong(handler, GWL_STYLE, WS_BORDER);
        /// 
        bool result = SetWindowPos(handler, -1,0, 0, defaultWidth,defaultHeight, SWP_SHOWWINDOW);
#endif
    }
}

