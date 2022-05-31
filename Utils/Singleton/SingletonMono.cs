// CSharp单例
using UnityEngine;
public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T _instance;
    private static object o=new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<T>();

                lock (o)
                {
                    if (_instance == null)
                    {
                	        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
		            
                    }
                }
            }
            return _instance;
        }
    }
}
