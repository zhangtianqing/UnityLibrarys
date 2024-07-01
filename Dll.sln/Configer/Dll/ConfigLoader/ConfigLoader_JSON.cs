using Dll.Framework.Config.ConfigLoader;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Dll.Framework.Config.ConfigLoader
{
    public class ConfigLoader_JSON : IConfigLoader
    {
        private string fileName;
        private string filePath;
        private string dirpath;
        public ConfigLoader_JSON() {

            fileName = GetDefaultFileName();
            dirpath = Application.streamingAssetsPath;
            filePath = Path.Combine(dirpath, fileName);
        }
        public override A Load<A>()
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<A>(json);
        }

        public override bool Save(object t)
        {
            try
            {
                string json = JsonConvert.SerializeObject(t);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return false;
        }
    }
}
