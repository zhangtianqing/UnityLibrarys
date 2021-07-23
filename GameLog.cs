using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 将会自动禁用Player日志
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

    public string LogSufferName = "Game";

    private string FilePath = "";

    // Start is called before the first frame update
    void Start()
    {
        initLog();
        Application.logMessageReceived += HandleLog;
        PlayerSettings.usePlayerLog = false;
        Debug.Log("---------Power By Sele---------");
        Debug.Log("---------按天日志记录----------");
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("A" + i);

        }
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private readonly string[] logFormats = new string[] { "{0}-{1}-{2}:\n{3}\n", "{0}-{1}-{2}\n" };
    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (NeedStackTrace)
        {
            File.AppendAllText(FilePath, string.Format(logFormats[0], DateTime.Now.ToString(), type.ToString(), condition, stackTrace));
        }
        else
        {
            File.AppendAllText(FilePath, string.Format(logFormats[1], DateTime.Now.ToString(), type.ToString(), condition));
        }

    }
    void initLog()
    {
        string pathformat = "{0}\\{1}_{2}.log";
        string logfilesuffer = LogSufferName;
        string times = DateTime.Now.ToLongDateString().Replace(":", "-").Replace(" ", "_").Replace("/", "-");
        switch (position)
        {
            case LogFilePosition.Project:
                FilePath = string.Format(pathformat, Application.streamingAssetsPath, times, logfilesuffer).Replace("/", "\\");
                break;
            case LogFilePosition.System:
                FilePath = string.Format(pathformat, string.Format(@"{0}\AppData\LocalLow\{1}\{2}", Environment.GetEnvironmentVariable("USERPROFILE"), PlayerSettings.companyName, PlayerSettings.productName), times, logfilesuffer).Replace("/", "\\");
                break;
            default:
                break;
        }
        Debug.Log("LogFilePath:" + FilePath);
        string fileDir = FilePath.Substring(0, FilePath.LastIndexOf('\\'));
        
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
