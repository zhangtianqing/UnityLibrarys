
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 公共Mono模块
/// 作用：让没有继承mono的类可以开启协程进行update真更新
/// Mono的管理者
/// 类中的构造函数在被new的时候可以进行执行
/// </summary>
public class MonoController : MonoSingleton<MonoController>
{
    public event UnityAction updateEvent;
    public event UnityAction destoryEvent;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //MethodInfo[] methodInfos =typeof(BaseUnityLiftEvent_Lift) .GetMethods( BindingFlags.NonPublic);
        //foreach (var item in methodInfos)
        //{
        //    Debug.Log(item.Name);
        //}
    }
    void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    Coroutine enumerator1 = null;
    public void StartIE(IEnumerator enumerator) {
        if (enumerator!=null)
        {
            StopIE(enumerator);
        }
        enumerator1= StartCoroutine(enumerator);
    }

    public void StopIE(IEnumerator enumerator) {
        if (enumerator1!=null)
        {
            StopCoroutine(enumerator);
        }
    }

    private void OnDestroy()
    {
        destoryEvent?.Invoke();
    }

}
