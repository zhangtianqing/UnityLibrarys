using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OsUtil : MonoBehaviour
{
    public static string CurrentThreadInfo() {
        return "��ǰӦ�ã�" + Process.GetCurrentProcess().ProcessName + " ����ID: " + Process.GetCurrentProcess().Id;
    }
    /// <summary>
    /// ����Ӧ��
    /// </summary>
    /// <param name="ApplicationPath">Ӧ�þ���·��</param>
    public static void StartProcess(string ApplicationPath)
    {
        UnityEngine.Debug.Log("�򿪱���Ӧ��");
        Process foo = new Process();
        foo.StartInfo.FileName = ApplicationPath;
        foo.Start();
    }

    /// <summary>
    /// ���Ӧ���Ƿ���������
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
                        UnityEngine.Debug.Log(processName + "��������");
                        isRunning = true;
                        continue;
                    }
                    else if (!process.ProcessName.Contains(processName) && i > processes.Length)
                    {
                        UnityEngine.Debug.Log(processName + "û������");
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
    /// �г��ѿ�����Ӧ��
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
                    list.Add("Ӧ��ID:" + process.Id + "Ӧ����:" + process.ProcessName);
                }
            }
            catch (Exception )
            {
            }
        }
        return list;
    }
    /// <summary>
    /// ɱ������
    /// </summary>
    /// <param name="processName">Ӧ�ó�����</param>
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
                        UnityEngine.Debug.Log("��ɱ������");
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
