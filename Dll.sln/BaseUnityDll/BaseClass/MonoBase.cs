using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Core.Frame
{
    public class MonoBase : MonoBehaviour
    {


        private class QuitEvent : UnityEvent { }
        private QuitEvent quitEvent = new QuitEvent();

        protected void OnApplicationQuit()
        {
            quitEvent?.Invoke();
        }
        protected virtual void AddQuitEvent(UnityAction unityAction)
        {
            quitEvent.AddListener(unityAction);
        }
        protected virtual void ReMoveQuitEvent(UnityAction unityAction)
        {
            quitEvent.RemoveListener(unityAction);
        }
        protected virtual void ReMoveAllQuitEvent()
        {
            quitEvent.RemoveAllListeners();
        }

        protected Coroutine DelayToDo(float time, UnityAction unityAction)
        {
            Coroutine coroutine= StartCoroutine(DelayToDoIE(time, unityAction));
            return coroutine;
        }
        IEnumerator DelayToDoIE(float time, UnityAction unityAction)
        {
            yield return new WaitForSeconds(time);
            unityAction?.Invoke();
        }
    }
}
