using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 文件IO加载工具类
/// 
/// 使用方法：
///     设定：
///         LoadType枚举中添加加载大类型
///         初始化方法(LoadFiles())内添加对应的后缀以及存放路径
///     调用：
///         List<string> fileList=GetFilesList(LoadType.Video);
/// </summary>
/// <typeparam name="T"></typeparam>
public static class LoadResources<T>
{

    private static string fileName = "Resources/"; //IO方式加载的路径


    private static DirectoryInfo dirFar;
    private static List<string> fileList = new List<string>();

    private static Dictionary<LoadType, string> fileTypeSuffix = new Dictionary<LoadType, string>();
    private static Dictionary<LoadType, string> folderTypeSuffix = new Dictionary<LoadType, string>();

    static LoadResources()
    {
        //初始化 后缀集合与存放路径
        fileTypeSuffix.Add(LoadType.Texture, "jpg|png|gif");
        folderTypeSuffix.Add(LoadType.Texture, "images");
        
        fileTypeSuffix.Add(LoadType.Video, "mp4|mov|mkv ");
        folderTypeSuffix.Add(LoadType.Video, "videos");

    }

    public enum LoadType
    {
        Texture,
        Video
    }

    private static LoadType currentType;

    public static List<string> GetFilesList(LoadType type)
    {
        if (fileList.Count>0)     fileList.Clear();
        currentType = type;

        GetAllFile(
            new DirectoryInfo(
                (
                    Application.streamingAssetsPath +
                    fileName +
                    folderTypeSuffix[currentType]
                ).Replace("\\", "/")
                )
            );
        return fileList;
    }
    private static void GetAllFile(DirectoryInfo directory)
    {
        //文件夹
        FileSystemInfo[] fil = directory.GetFileSystemInfos();

        foreach (FileSystemInfo i in fil)
        {
            if (i is DirectoryInfo)
            {
                // 如果是文件夹，递归查找
                GetAllFile((DirectoryInfo)i);
            }
            else
            {
                //文件
                string str = i.FullName;
                string suffix = str.Substring(str.LastIndexOf('.'), str.Length - 1).ToLower();

                //过滤
                foreach (var item in fileTypeSuffix[currentType].Split('|'))
                {
                    if (suffix == item)
                    {
                        fileList.Add(str);
                        break;
                    }
                }
            }
        }
    }
}
