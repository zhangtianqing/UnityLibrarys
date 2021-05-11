using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OsUtil : MonoBehaviour
{
    public static string CurrentThreadInfo() {
        return "当前应用：" + Process.GetCurrentProcess().ProcessName + " 进程ID: " + Process.GetCurrentProcess().Id;
    }
    /// <summary>
    /// 开启应用
    /// </summary>
    /// <param name="ApplicationPath">应用绝对路径</param>
    public static void StartProcess(string ApplicationPath)
    {
        UnityEngine.Debug.Log("打开本地应用");
        Process foo = new Process();
        foo.StartInfo.FileName = ApplicationPath;
        foo.Start();
    }

    /// <summary>
    /// 检查应用是否正在运行
    /// </summary>
    public static bool CheckProcess(string processName)
    {
        bool isRunning = false;
        Process[] processes = Process.GetProcesses();
        int i = 0;
        foreach (Process process in processes)
        {
            try
            {
                i++;
                if (!process.HasExited)
                {
                    if (process.ProcessName.Contains(processName))
                    {
                        UnityEngine.Debug.Log(processName + "正在运行");
                        isRunning = true;
                        continue;
                    }
                    else if (!process.ProcessName.Contains(processName) && i > processes.Length)
                    {
                        UnityEngine.Debug.Log(processName + "没有运行");
                        isRunning = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        return isRunning;
    }
    /// <summary>
    /// 列出已开启的应用
    /// </summary>
    public static List<string> ListAllAppliction()
    {
        Process[] processes = Process.GetProcesses();
        List<string> list = new List<string>();
        foreach (Process process in processes)
        {
            try
            {
                if (!process.HasExited)
                {
                    list.Add("应用ID:" + process.Id + "应用名:" + process.ProcessName);
                }
            }
            catch (Exception )
            {
            }
        }
        return list;
    }
    /// <summary>
    /// 杀死进程
    /// </summary>
    /// <param name="processName">应用程序名</param>
    public static bool KillProcess(string processName)
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            try
            {
                if (!process.HasExited)
                {
                    if (process.ProcessName == processName)
                    {
                        process.Kill();
                        UnityEngine.Debug.Log("已杀死进程");
                    }
                }
            }
            catch (System.InvalidOperationException)
            {
                return false;
                //UnityEngine.Debug.Log("Holy batman we've got an exception!");
            }
        }
        return true;
    }

}
