using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 请禁用Player日志
/// </summary>
public class GameLog : MonoBehaviour
{
    [Header("是否输出消息栈")]
    public bool NeedStackTrace = false;

    public enum LogFilePosition
    {
        Project,
        System
    }
    [Header("日志位置：Project(跟随项目)/System(存放系统默认位置)")]
    public LogFilePosition position;
    /// <summary>
    /// 日志打印索引
    /// </summary>
    public long logIndex = 0;
    /// <summary>
    /// 日志分类名
    /// </summary>
    public string LogSufferName = "Game";

    private string FilePath = "";
    private string fileDir ="";

    private void Awake()
    {
        initLog();
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private readonly string[] logFormats = new string[] { 
        "{0}-本次运行第{1}次打印日志-{2}-{3}\n{4}\n", 
        "{0}-本次运行第{1}次打印日志-{2}-{3}\n" 
    };
    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (FilePath.Equals("")|| !File.Exists(FilePath))
        {
            initLog();
        }
        if (NeedStackTrace)
        {
            File.AppendAllText(FilePath, string.Format(logFormats[0], DateTime.Now.ToString(), logIndex++, type.ToString(), condition, stackTrace));
        }
        else
        {
            File.AppendAllText(FilePath, string.Format(logFormats[1], DateTime.Now.ToString(), logIndex++, type.ToString(), condition));
        }

    }
    void initLog()
    {
        string pathformat = "{0}\\Log\\{1}_{2}.log";
        string logfilesuffer = LogSufferName;
        string times = DateTime.Now.ToLongDateString().Replace(":", "-").Replace(" ", "_").Replace("/", "-") +"-"+ DateTime.Now.ToLongTimeString().Replace(":", "").Replace(" ", "_").Replace("/", "-");

        NeedStackTrace = INIHelper.GetBool("Log_NeedStackTrace");
        switch (position)
        {
            case LogFilePosition.Project:
                FilePath = string.Format(pathformat, Application.streamingAssetsPath, times, logfilesuffer).Replace("/", "\\");
                break;
            case LogFilePosition.System:
                FilePath = string.Format(pathformat, string.Format(@"{0}\AppData\LocalLow\{1}\{2}", Environment.GetEnvironmentVariable("USERPROFILE"), Application.companyName, Application.productName), times, logfilesuffer).Replace("/", "\\");
                break;
            default:
                break;
        }
        fileDir = FilePath.Substring(0, FilePath.LastIndexOf('\\'));
        
        if (!Directory.Exists(fileDir))
        {
            Directory.CreateDirectory(fileDir);
        }

        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Dispose();
        }

    }
}
