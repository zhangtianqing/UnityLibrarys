using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SingletonClass<T>   where T : SingletonClass<T>,new()
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
                    }
                }
            }
            return _instance;
        }
    }
}
