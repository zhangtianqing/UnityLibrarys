
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ����Monoģ��
/// ���ã���û�м̳�mono������Կ���Э�̽���update�����
/// Mono�Ĺ�����
/// ���еĹ��캯���ڱ�new��ʱ����Խ���ִ��
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
