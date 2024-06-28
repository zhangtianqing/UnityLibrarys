

using Dll.Framework.Config.ConfigClass;
using Dll.Framework.Config.ConfigLoader;
using UnityEngine;

[DisallowMultipleComponent]
/// <summary>
/// 配置管理器脚本
/// 自动 读取配置/保存配置 
/// 支持数据类型：int float string double bool enum
/// </summary>
public class Configer : MonoSingleton<Configer>
{

    /// <summary>
    /// 配置存取类型
    /// </summary>
    public ConfigType configType = ConfigType.XML;
    [SerializeField]
    /// <summary>
    /// 读取的游戏配置
    /// </summary>
    public GameConfig gameConfig;
    /// <summary>
    /// 配置加载器
    /// </summary>
    public IConfigLoader loader1;

    private void Awake()
    {
        gameConfig = Instance.gameConfig;
    }
    /// <summary>
    /// 配置初始化方法
    /// </summary>
    public override void Init()
    {
        //Debug.Log("Configer-初始化");
        //根据实现配置存取类型实例化对应的配置加载器
        switch (configType)
        {
            case ConfigType.XML:
                loader1 = new ConfigLoader_XML();
                break;
            case ConfigType.INI:
                Debug.Log("此加载方法仅支持2层加载，配置类持有多个子类，子类不得再持有子类");
                //throw new System.Exception("没有实现这个方法");
                loader1 = new ConfigLoader_INI();
                break;
            default:
                loader1 = new ConfigLoader_XML();
                break;
        }
        gameConfig = loader1.Load<GameConfig>();
        Debug.Log("Configer-初始化完成");
    }
    /// <summary>
    /// 退出时保存设置
    /// </summary>
    private void OnApplicationQuit()
    {
        if (gameConfig.defaultSetting.saveConfigWhenExit)
        {
            Debug.Log(loader1 == null);
            Debug.Log(gameConfig == null);
            loader1.Save(gameConfig);
        }

    }

    public T Load<T>()  where T : class, new()
    {
        T t= loader1.Load<T>();
        return default(T); 
    }
    public void Save<T>(T t) {
        loader1.Save(t);
    }
    public void Save()
    {
        if (loader1 != null)
        {
            Debug.Log("保存配置");
            loader1.Save(gameConfig);
        }
    }

    /// <summary>
    /// 配置存取类型
    /// </summary>
    public enum ConfigType
    {
        XML,
        INI
    }

}

