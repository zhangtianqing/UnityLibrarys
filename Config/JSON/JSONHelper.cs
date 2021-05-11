using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Script.Tools.Config.JSON
{
    public class JSONHelper<T>
    {
        /// <summary>
        /// 自定义模型获取对应路径下的JSON配置文件对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T GetJsonOBJECT(string path) {
            if (path==null||path.Contains(""))
            {
                path = Application.dataPath + "/StreamingAssets/config.json";
            }
            return  JsonUtility.FromJson<T>(File.ReadAllText(path, Encoding.UTF8));
        }
    }

    //class CustomModel
    //{
    //    public string key1;
    //    public string key2;
    //}
}