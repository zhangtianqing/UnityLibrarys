public class ClassSingleton<T> where T : ClassSingleton<T>, new()
{
    private static T _instance;
    private static object o = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (o)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        _instance.InitHandler();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 脚本初始化 标识
    /// </summary>
    bool ScriptInit = false;
    /// <summary>
    /// 此处通过状态标识来决定时否要初始化
    /// </summary>
    private void InitHandler()
    {
        if (ScriptInit)
        {
            return;
        }
        ScriptInit = true;
        Init();
    }
    /// <summary>
    /// 留给子类重写的初始化方法
    /// </summary>
    protected virtual void Init() { }
}
