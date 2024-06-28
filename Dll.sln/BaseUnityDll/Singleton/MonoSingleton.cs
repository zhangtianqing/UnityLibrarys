
using UnityEngine;
/// <summary>
/// Unity Mono脚本 单例泛型实例化
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    /// <summary>
    /// 持有的单例对象
    /// </summary>
    protected static T _instance;
    /// <summary>
    /// 实例化锁
    /// </summary>
    static object o = new object();
    /// <summary>
    /// 脚本初始化 标识
    /// </summary>
    bool ScriptInit = false;
    /// <summary>
    /// 单例对象获取
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (o)
                {
                    //是否其他脚本 调用的实例方法在锁定期间 已经初始化完成
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            //实例化脚本
                            _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                            //调用初始化方法

                        }
                        _instance.InitHandler();
                    }

                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        InitHandler();
        _instance = this as T;
    }
    /// <summary>
    /// 此处通过状态标识来决定时否要初始化
    /// </summary>
    protected  void InitHandler()
    {
        if (ScriptInit)
        {
            return;
        }
        ScriptInit = true;
        OnInstace();
    }
    /// <summary>
    /// 留给子类重写的初始化方法
    /// </summary>
    protected virtual void OnInstace() { }
}