

using System.IO;
using UnityEngine;

namespace BaseUnityDll.Enums
{
    public enum UnityPositionEnum
    {
        StreamingAssetsPath,
        PersistentDataPath,
        DataPath
    }
    public class PosByEnum {
        public static string GetPosByPosEnum(UnityPositionEnum unityPositionEnum) {
            switch (unityPositionEnum)
            {
                case UnityPositionEnum.StreamingAssetsPath:
                    return Application.streamingAssetsPath;
                case UnityPositionEnum.PersistentDataPath:
                    return Application.persistentDataPath;
                case UnityPositionEnum.DataPath:
                    return Application.dataPath;
                default:
                    return ".";
            }
        }
        public static string GetPosByPosEnum(UnityPositionEnum unityPositionEnum, string filepath, string fileName) {
            return Path.Combine(GetPosByPosEnum(unityPositionEnum), filepath, fileName);
        }
    }
}
