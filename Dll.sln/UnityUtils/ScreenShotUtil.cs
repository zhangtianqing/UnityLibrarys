using System;
using System.Collections;
using UnityEngine;

namespace Dll.UnityUtils
{
    internal class ScreenShotUtil
    {
       public static string[] path = new string[] { Application.streamingAssetsPath + "/ScreenShot/ScreenShot_Ori_{0}.png", Application.streamingAssetsPath + "/ScreenShot/ScreenShot_Com_{0}.png" };
        public static void ShotOri(Action<Texture2D> callback)
        {
            Shot(callback, null, Camera.main);
        }
        public static void ShotComp(Action<Texture2D> callback)
        {
            Shot(null, callback, Camera.main);
        }

        public static void Shot(Action<Texture2D> callbackOri, Action<Texture2D> callbackComp, Camera _camera,bool saveOriPng=false, bool compress = false, bool saveCompressPng = false, float compressRate = 0.5f)
        {
            MonoController.Instance.StartCoroutine(IE(callbackOri, callbackComp, _camera, saveOriPng, compress, saveCompressPng, compressRate));
        }

        static IEnumerator IE(Action<Texture2D> callbackOri, Action<Texture2D> callbackComp, Camera _camera, bool saveOriPng, bool compress, bool saveCompressPng, float compressRate)
        {
            int width = Screen.width;
            int height = Screen.height;
            //对指定相机进行 RenderTexture
            RenderTexture renTex = new RenderTexture(width, height, 16);
            _camera.targetTexture = renTex;
            _camera.Render();


            //读取像素
            Texture2D tex = new Texture2D(width, height);
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();
            //读取目标相机像素结束，渲染恢复原先的方式
            _camera.targetTexture = null;
            callbackOri?.Invoke(tex);

            string screenshotNum = DateTime.Now.ToLongTimeStamp().ToString();
            //保存读取的结果（原图）
            if (saveOriPng)
            {
                string originalPath = string.Format(path[0], screenshotNum);
                System.IO.File.WriteAllBytes(originalPath, tex.EncodeToPNG());
            }

            if (compress)
            {
                //压缩
                Texture2D tex_CompressedSize = new Texture2D((int)(width / compressRate), (int)(height / compressRate), TextureFormat.ARGB32, false);
                for (int i = 0; i < tex_CompressedSize.width; i++)//为竖向画面的 从下到上的像素颜色赋值，对应原始Texture2D的从左到右的像素颜色
                {
                    for (int j = 0; j < tex_CompressedSize.height; j++)
                    {
                        tex_CompressedSize.SetPixel(i, j, tex.GetPixel((int)((i) * compressRate - compressRate / 2), (int)((j) * compressRate - compressRate / 2)));
                    }
                }
                tex_CompressedSize.Apply();
                callbackComp?.Invoke(tex_CompressedSize);
                if (saveCompressPng)
                {
                    //保存读取的结果（压缩）
                    string compressedPath = string.Format(path[1], screenshotNum);//Application.dataPath + "/ScreenShot_Com" + GetTimeStamp() + "_YS.png";
                    System.IO.File.WriteAllBytes(compressedPath, tex_CompressedSize.EncodeToPNG());
                }
            }
            yield return null;
        }
    }

    public enum TextureShotRotation {
        Horizontal=0,
        Vertical
    }
}
