using Assets.Script.Tools.EncryptionAndDecryption;
using Assets.Script.Tools.Licence;
using System;
using System.IO;
using UnityEngine;

/// <summary>
///
/// </summary>
public class LicenceManager : MonoBehaviour
{
    /// <summary>
    /// 加密密钥
    /// </summary>
    private static string aesKey = "2b7e151628aed2a6abf7158809cf4f3c";
    private static string aesIV= "123456789abcdef1";

    private int EndYear = 2021;
    private int EndMounth = 2021;
    private int EndDay = 2021;

    /// <summary>
    /// 可用时间
    /// </summary>
    public int useDay = 20;

    public int StartYear = 2021;
    public int StartMounth = 4;
    public int StartDay = 30;

    //程序启动时间
    private DateTime startTime = DateTime.Now;
    
    /// <summary>
    /// 已使用时间
    /// </summary>
    private long usedTime = 0;
    /// <summary>
    /// 计时文件路径
    /// </summary>
    private string configFilePath = "";
    /// <summary>
    /// 永久许可密钥
    /// </summary>
    public string foreverKey = "416a4f68a4d12153483sd%824316&4sd6a43>.13as10.";
    private bool foreverRun = false;
    private bool ConfigFileLost=false;

    // Start is called before the first frame update
    void Start()
    {
        //E:\Workspaces\Unity\Librarys\config.asd
        configFilePath = new DirectoryInfo(Application.dataPath).Parent.FullName + @"\config.asd";
        
        DateTime dateTime = DateTime.Now;
        dateTime.AddDays(useDay);

        EndYear = dateTime.Year;
        EndMounth = dateTime.Month;
        EndDay = dateTime.Day;

        if (null != INIHelper.GetString("foreverkey") 
            && INIHelper.GetString("foreverkey").Equals(foreverKey)
            )
        {
            print("检测到永久许可密钥");
            foreverRun = true;
            
        }
        else {
            InitConfig();
        }
    }

    private void OnGUI()
    {
        if (!foreverRun)
        {
            if (!LicenceValid())
            {
                GUIContent gUIContent = new GUIContent("许可已过期");
                GUIStyle gUIStyle = new GUIStyle();
                gUIStyle.fontSize = 100;
                gUIStyle.normal.textColor = new Color(255f / 256f, 0, 0);
                gUIStyle.contentOffset = new Vector2(Screen.width / 2, Screen.height / 2);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width, Screen.height), gUIContent, gUIStyle);

            }
            if (ConfigFileLost)
            {
                GUIContent gUIContent = new GUIContent("请恢复配置文件");
                GUIStyle gUIStyle = new GUIStyle();
                gUIStyle.fontSize = 100;
                gUIStyle.normal.textColor = new Color(255f / 256f, 0, 0);
                gUIStyle.contentOffset = new Vector2(Screen.width / 2, Screen.height / 2);
                GUI.Label(new Rect(0, 0, Screen.width/2, Screen.height/2), gUIContent, gUIStyle);
            }
        }
    }

    private void InitConfig()
    {

        if (!File.Exists(configFilePath))
        {
            //saveConfig();
            ConfigFileLost = true;
        }
        else { 
            //1.ReadIniConfig
            string content = File.ReadAllText(configFilePath);
            //read
            content = AESHelper.AesDecrypt(content, aesKey).Replace(foreverKey, "");
            //dcode
            usedTime =Convert.ToInt64(content);

            print("启动时-已使用时间:"+usedTime);
        }
    }
    private void OnDestroy()
    {
        if (!foreverRun)
        {
            saveConfig();
        }
    }

    public void saveConfig()
    {
        //if (ConfigFileLost)
        //{
        //    return;
        //}
        Config config = new Config();
        usedTime += (long)DateTime.Now.Subtract(startTime).TotalSeconds;

        print("关闭-已使用时间:" + usedTime);
        string content = AESHelper.AesEncrypt(foreverKey + usedTime + "", aesKey) ;
        if (File.Exists(configFilePath))
        {
            File.Delete(configFilePath);
        }
        File.Create(configFilePath).Dispose();
        File.WriteAllText(configFilePath, content);
    }
    /// <summary>
    /// 是否可用
    /// </summary>
    /// <returns></returns>
    private bool LicenceValid() {
        //日期是否超时
        if (Overdue())
        {
            return false;
        }
        //时间是否超时
        if (GetRemainingTime()<=0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否未逾期
    /// </summary>
    /// <returns></returns>
    public bool Overdue() {
        if (
            startTime.Year<EndYear
            &&
            startTime.Month<EndMounth
            &&
            startTime.Day<EndDay
            )
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 许可使用的时间
    /// </summary>
    /// <returns></returns>
    private long useTime()
    {
        return (long)new DateTime(EndYear, EndMounth, EndDay).Subtract(new DateTime(StartYear, StartMounth, StartDay)).TotalSeconds;
    }
    /// <summary>
    /// 剩余时间
    /// </summary>
    /// <returns></returns>
    private long GetRemainingTime()
    {
        print("许可时间：" + useTime());
        return useTime() - usedTime;
    }

}
