using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
///不知道为什么是双屏的 业务场景暂时用不上，所以只作为记录  -- 来源 ：https://blog.csdn.net/qq_39097425/article/details/81664448
public class WindowMod_MuiltScreen : MonoBehaviour {
 
 
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
 
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;  //边框用的
    const int WS_BORDER = 0;
    const int WS_POPUP = 0x800000;
 
    int _posX = 0;
    int _posY = 0;
    // 在这里设置你想要的窗口宽
    int _Txtwith = 3840;
    // 在这里设置你想要的窗口高
    int _Txtheight = 1080;
    void Start()
    {
        Cursor.visible = false; // 鼠标隐藏
        Screen.SetResolution(3840, 1080, false);
        if(Application.platform != RuntimePlatform.WindowsEditor)
        {
            StartCoroutine(Setposition());
            Debug.Log("打包出来了");
        }
        else
        {
            Debug.Log("WINDOWS EDITOR");
        }
        
    }
    IEnumerator Setposition()
    {
        yield return new WaitForSeconds(0.1f);//不知道为什么发布于行后，设置位置的不会生效，我延迟0.1秒就可以
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);      //无边框
        bool result = SetWindowPos(GetForegroundWindow(), 0, _posX-1, _posY-1, _Txtwith+2, _Txtheight+2, SWP_SHOWWINDOW);       //设置屏幕大小和位置
    }
}