/**
*┌──────────────────────────────────────────────────────────────┐
*│　Description： 模拟鼠标点击的操作                                                   
*│　Author：#Keneyr#                                              
*│　Version：#1.0#                                                
*│　Date：#2019.7.30#  
*│  UnityVersion: #Unity2018.3.2f#                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐                                   
*│　ClassName：#MouseSimulator#                                      
*└──────────────────────────────────────────────────────────────┘
*/
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MouseSimulator : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y); //设置光标位置
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref int x, ref int y); //获取光标位置
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo); //鼠标事件
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow(); //获取当前活动窗口的句柄
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string strClassName, string strWindowName); //根据要找窗口的类或者标题查找窗口,只能找父窗口
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow); //在窗口列表中寻找与指定条件相符的第一个子窗口
    //[DllImport("user32.dll")]
    //private static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam); //枚举一个父窗口的所有子窗口
    //public delegate bool CallBack(IntPtr hwnd, int lParam);
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle); //获取窗口大小

    //这个枚举同样来自user32.dll
    [Flags]
    enum MouseEventFlag : uint
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

    //这个也来自uer32.dll
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left { get; set; } //最左坐标
        public int Top { get; set; } //最上坐标
        public int Right { get; set; } //最右坐标
        public int Bottom { get; set; } //最下坐标
    }

    //声明Unity屏幕坐标转换和系统屏幕坐标转换所需要的变量，目前是系统坐标嵌套Game窗口，Game窗口嵌套分辨率窗口
    private int Dx, Dy; //窗口在桌面坐标系下的位置
    private int Window_Width, Window_Height; //窗口的宽和高
    private static int Resolution_Width = UnityEngine.Screen.width;
    private static int Resolution_Height = UnityEngine.Screen.height; //屏幕分辨率，其实就是Game视图下有景色的地方，是嵌套中最小的


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MouseSimulator Start");
        //模拟键盘输入
        //SendKeys.Send("{BACKSPACE}");

        //Debug.Log("Unity的屏幕分辨率" + UnityEngine.Screen.width + "x" + UnityEngine.Screen.height); //widthxheight
        //Debug.Log("系统的分辨率" + UnityEngine.Screen.currentResolution); //widthxheight

        //GetTheCurrentWindowInfo();

        GetTheChildWindowInfo();

    }


    // Update is called once per frame
    void Update()
    {

    }


    //模拟鼠标左键点击
    public void MouseClickSimulate(int x, int y)
    {
        SetCursorPos(x, y);
        mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
        mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);

    }
    public void CloseSelf()
    {
        //Debug.Log("点击鼠标，调用了CloseSelf()");
    }

    //坐标转换:Unity屏幕坐标到系统屏幕坐标转换
    public Vector2 ConvertUnityScreenToSystemScreen(int x, int y)
    {
        //
        int pointSystemCoordinatex = Dx + x + (Window_Width - Resolution_Width) / 2;
        int pointSystemCoordinatey = Dy + (Window_Height - y);
        Vector2 pos = new Vector2(pointSystemCoordinatex, pointSystemCoordinatey);
        return pos;
    }

    //获取当前活动窗口的信息
    public void GetTheCurrentWindowInfo()
    {
        //获取当前活动窗口的句柄
        IntPtr ptr = GetForegroundWindow();
        GetWindowInfo(ptr);

    }

    //根据进程名字找到该窗口,并获取其信息
    public void GetTheWindowInfoByProcessName(String processname)
    {
        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processname);
        System.Diagnostics.Process lol = processes[0];
        IntPtr ptr = lol.MainWindowHandle; //获取到窗口的句柄
        GetWindowInfo(ptr);
    }

    //获取当前主窗口下特定子窗口的信息:获取Unity下的Game视图窗口
    public void GetTheChildWindowInfo()
    {
        IntPtr father_ptr = FindWindow("UnityContainerWndClass", null);
        IntPtr child_ptr = FindWindowEx(father_ptr, IntPtr.Zero, "UnityGUIViewWndClass", "UnityEditor.GameView");
        if (child_ptr != IntPtr.Zero)
        {
            GetWindowInfo(child_ptr);
        }
        else
        {
            //Debug.Log("Cannot find the childWindow");
            return;
        }

    }

    //根据窗口句柄获取窗口基本信息:位置和大小
    private void GetWindowInfo(IntPtr ptr)
    {
        Rect currentRect = new Rect();
        GetWindowRect(ptr, ref currentRect); //ptr为窗口句柄

        //窗口的width和height
        Window_Width = currentRect.Right - currentRect.Left;
        Window_Height = currentRect.Bottom - currentRect.Top;

        //Debug.Log(Window_Width + "," + Window_Height);

        //窗口所在位置
        Dx = currentRect.Left;
        Dy = currentRect.Top;

        Debug.Log(Dx + " " + Dy);
    }

}