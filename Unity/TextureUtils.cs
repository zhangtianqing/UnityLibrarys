

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureUtils
{
    /// <summary>
    /// 将Texture2D保存到桌面
    /// </summary>
    /// <param name="spng">源</param>
    /// <returns></returns>
    public static bool SaveTexture2DToPNG(Texture2D spng)
    {
        return SaveTexture2DToPNG(spng, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyyMMddHHmmssffff"));
    }
    /// <summary>
    /// 将Texture2D保存到 指定位置
    /// </summary>
    /// <param name="spng">源</param>
    /// <param name="contents">路径</param>
    /// <param name="pngName">文件名-无后缀</param>
    /// <returns></returns>
    public static bool SaveTexture2DToPNG(Texture2D spng, string contents, string pngName)
    {
        Debug.Log($"save {contents}");
        Texture2D png = new Texture2D(spng.width, spng.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, spng.width, spng.height), 0, 0);
        byte[] bytes = spng.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        return true;
    }
    /// <summary>
    /// 将RenderTexture保存到桌面
    /// </summary>
    /// <param name="_renderTex"></param>
    /// <returns></returns>
    public static bool SaveRenderTextureToPNG(RenderTexture _renderTex)
    {
        return SaveRenderTextureToPNG(_renderTex, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyyMMddHHmmssffff"));
    }
    /// <summary>
    /// 将RenderTexture保存到指定位置
    /// </summary>
    /// <param name="rt">源</param>
    /// <param name="contents">路径</param>
    /// <param name="pngName">文件名-无后缀</param>
    public static bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;

    }
    /// <summary>
    /// 在指定目录下读取材质(*.BMP|*.JPG|*.GIF|*.PNG)
    /// </summary>
    /// <param name="dir">目标路径</param>
    /// <returns></returns>
    public static List<Texture2D> getTexture(string dir)
    {
        List<string> filePaths = new List<string>();
        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');
        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取Application.dataPath文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles(dir, ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }
        List<Texture2D> texture2s = new List<Texture2D>();
        for (int i = 0; i < filePaths.Count; i++)
        {
            Texture2D tx = new Texture2D(100, 100);
            tx.LoadImage(getImageByte(filePaths[i]));
            texture2s.Add(tx);
        }
        return texture2s;
    }
    /// <summary>  
    /// 根据图片路径返回图片的字节流byte[]  
    /// </summary>  
    /// <param name="imagePath">图片路径</param>  
    /// <returns>返回的字节流</returns>  
    private static byte[] getImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }
}
