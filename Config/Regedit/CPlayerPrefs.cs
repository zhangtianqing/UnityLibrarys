
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注册表配置读写器 建议展厅行业运行前删除屏幕配置
/// </summary>
public class CPlayerPrefs : PlayerPrefs
{
    #region 默认键值记录与打印
    /// <summary>
    /// Unity 注册表默认信息配置
    /// </summary>
    public static CommonKey[] regeditCommonKeys = new CommonKey[] {
        new CommonKey(){ key="Screenmanager Fullscreen mode Default",type=typeof(int).Name,desc="默认全屏模式" },
        new CommonKey(){ key="Screenmanager Fullscreen mode",type=typeof(int).Name,desc="全屏模式"  },

        new CommonKey(){ key="Screenmanager Resolution Use Native Default",type=typeof(int).Name ,desc="默认使用Native方法" },
        new CommonKey(){ key="Screenmanager Resolution Use Native",type=typeof(int).Name,desc="使用Native方法"  },

        new CommonKey(){ key="Screenmanager Resolution Height Default",type=typeof(int).Name ,desc="默认高度" },
        new CommonKey(){ key="Screenmanager Resolution Height",type=typeof(int).Name,desc="高度"  },
        new CommonKey(){ key="Screenmanager Resolution Width Default",type=typeof(int).Name,desc="默认宽度"  },
        new CommonKey(){ key="Screenmanager Resolution Width",type=typeof(int).Name,desc="宽度"  },

        new CommonKey(){ key="Screenmanager Stereo 3D",type=typeof(int).Name},

        //new CommonKey(){ key="",type=typeof(float).Name  },

        new CommonKey(){ key="unity.player_session_count",type=typeof(string).Name,desc="启动次数"  },
        new CommonKey(){ key="unity.player_sessionid",type=typeof(string).Name ,desc="会话id" }
    };

    /// <summary>
    /// 打印默认的注册表信息
    /// </summary>
    public static void PrintDefaultValue() {
        Debug.Log("开始读取注册表信息");
        foreach (var item in regeditCommonKeys)
        {
            if (item.type == typeof(int).Name)
            {
                Debug.Log($"{item.desc}:{GetInt(item.key)}");
            }
            else if (item.type == typeof(string).Name)
            {
                Debug.Log($"{item.desc}:{GetString(item.key)}");
            }
            else
            if (item.type == typeof(float).Name)
            {
                Debug.Log($"{item.desc}:{GetFloat(item.key)}");
            }
        }
        Debug.Log("结束读取注册表信息");
    }
    #endregion

    /// <summary>
    /// 删除键
    /// </summary>
    /// <param name="key">键值</param>
    public static new void DeleteKey(string key)
    {
        try
        {
            if (HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        catch (Exception e)
        {
            throw new Exception("键值输入是否正确");
        }
    }

    #region JsonData
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="key">键值</param>
    public static T GetData<T>(string key)
    {
        try
        {
            T data = HasKey(key) ? JsonUtility.FromJson<T>(PlayerPrefs.GetString(key)) : default(T);
            return data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
    /// <summary>
    /// 覆盖当前数据
    /// </summary>
    /// <param name="key"> 键值</param>
    /// <param name="data"> 数据</param>
    public static void Save<T>(string key, T data)
    {
        try
        {
            string d = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, d);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }

    /// <summary>
    /// 储存得数据是集合 
    ///在此基础上增加数据
    /// </summary>
    /// <param name="key">键值</param>
    /// <param name="data">数据</param>
    public static void SaveAdd<T>(string key, T data)
    {
        try
        {
            List<T> list = HasKey(key) ? JsonUtility.FromJson<List<T>>(PlayerPrefs.GetString(key)) : new List<T>();
            if (list == null)
            {
                Debug.LogError("请检查类型是否正确 " + typeof(T));
                return;
            }
            list.Add(data);
            Save(key, list);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }

    #endregion
}

/// <summary>
/// 注册表信息记录类
/// </summary>
public class CommonKey
{
    public string key;
    /// <summary>
    /// typeof(int).Name ==> REG_DWORD
    /// typeof(float).Name ==> 不正确的REG_DWORD32
    /// typeof(string).Name ==> REG_BINARY
    /// </summary>
    public string type;
    public string desc
    {
        get
        {
            if (descV == "")
            {
                return key;
            }
            else
            {
                return descV;
            }
        }
        set
        {
            this.descV = value;
        }
    }
    private string descV = "";

}