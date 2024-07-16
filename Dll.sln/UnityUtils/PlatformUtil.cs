public class PlatformUtil
{
    public static string BasePath
    {
        get
        {
            //使用StreamingAssets路径注意AB包打包时 勾选copy to streamingAssets
#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
                return Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
                return Application.dataPath + "!/assets/";
#else
            return "unknown";
#endif
        }
    }
    //各个平台下的主包名称 --- 用以加载主包获取依赖信息
    public static string MainABName
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
                return "StandaloneWindows";
#elif UNITY_IPHONE
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#else
            return "unknown";
#endif
        }
    }
}