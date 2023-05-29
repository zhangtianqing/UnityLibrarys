
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注册表配置读写器 建议展厅行业运行前删除屏幕配置
/// </summary>
public class CPlayerPrefs : PlayerPrefs
{


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

    /// <summary>
    /// 删除所有数据
    /// </summary>
    public static new void DeleteAll()
    {
        try
        {
            PlayerPrefs.DeleteAll();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// 是否存在键值
    /// </summary>
    /// <param name="key">键值</param>
    /// <returns>bool</returns>
    private static new bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
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
