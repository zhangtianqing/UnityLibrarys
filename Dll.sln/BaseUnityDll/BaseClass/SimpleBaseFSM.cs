using BaseUnityDll.Class;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static BaseUnityDll.Class.BaseParmAlll;
using static UnityEngine.CullingGroup;

namespace BaseUnityDll.BaseClass
{
    public class SimpleBaseFSM<T> : MonoBehaviour where T : Enum
    {
        public T currentState;
        /// <summary>
        /// CurrentState TargetState
        /// </summary>
        public  Action<T,T> stateChange;

        protected virtual void Awake() {
            InitStates();
        }
        protected virtual void Start()
        {

            //切换到目标状态
            SwitchState((T)Enum.ToObject(typeof(T), -1));
        }
        Dictionary<T, StateChangedEvent> stateCtrl = new Dictionary<T, StateChangedEvent>();
        void InitStates()
        {
            MethodInfo methodInfo = null;
            string[] enums= Enum.GetNames(currentState.GetType());
            foreach (var item in enums)
            {
                T t = (T)Enum.Parse(currentState.GetType(), item);
                stateCtrl.Add(t, new StateChangedEvent());
                StateChangedEvent stateChangedEvent = stateCtrl[t];

                methodInfo = Find($"{t.ToString()}_{nameof(stateChangedEvent.OnStart)}");
                if (methodInfo != null)
                {
                    stateChangedEvent.OnStart = (Action<BaseParm>)Delegate.CreateDelegate(typeof(Action<BaseParm>), this, methodInfo);
                }

                methodInfo = Find($"{t.ToString()}_{nameof(stateChangedEvent.OnUpdate)}");
                if (methodInfo != null)
                {
                    stateChangedEvent.OnUpdate = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo);
                }

                methodInfo = Find($"{t.ToString()}_{nameof(stateChangedEvent.OnUpdateData)}");
                if (methodInfo != null)
                {
                    stateChangedEvent.OnUpdateData = (Action<BaseParm>)Delegate.CreateDelegate(typeof(Action<BaseParm>), this, methodInfo);
                }

                methodInfo = Find($"{t.ToString()}_{nameof(stateChangedEvent.OnExit)}");
                if (methodInfo != null)
                {
                    stateChangedEvent.OnExit = (Action<BaseParm>)Delegate.CreateDelegate(typeof(Action<BaseParm>), this, methodInfo);
                }
            }

        }
        protected virtual void Update() {
            if (stateCtrl.TryGetValue(currentState, out StateChangedEvent stateChangedEvent)) stateChangedEvent.OnUpdate?.Invoke();
        }

        //外部控制targetState的状态
        public void SwitchStateProxy(T targetState, BaseParm inputPara = null) => SwitchState(targetState, inputPara);
        protected void SwitchState(T targetState, BaseParm inputPara = null)
        {
            BaseParm baseParmExit = null;
            BaseParm baseParmStart = null;
            if (inputPara != null)
            {
                BaseParm<BaseParm, BaseParm> baseParmData = (BaseParm<BaseParm, BaseParm>)inputPara;
                baseParmExit = baseParmData.v1;
                baseParmStart = baseParmData.v2;
            }
            if (!EqualityComparer<T>.Default.Equals(currentState, targetState)) //避免重复切换
            {

                stateChange?.Invoke(currentState, targetState);
                Debug.LogWarning($"SwitchState:from {currentState} to {targetState}");
                try
                {
                    if (stateCtrl.TryGetValue(currentState,out  StateChangedEvent stateChangedEvent ))
                    {
                        stateChangedEvent.OnExit?.Invoke(baseParmExit);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"SwitchState Error:{e}");
                }
                currentState = targetState;
                try
                {
                    if (stateCtrl.TryGetValue(currentState, out StateChangedEvent stateChangedEvent))
                    {
                        stateChangedEvent.OnStart?.Invoke(baseParmStart);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"SwitchState Error:{e}");
                }
            }
            else
            {
                if (stateCtrl.TryGetValue(currentState, out StateChangedEvent stateChangedEvent))
                {

                    Debug.Log($"UpdateData: {currentState}");
                    stateChangedEvent.OnUpdateData?.Invoke(baseParmStart);
                }
            }
        }
        /// <summary>
        /// 获取当前枚举的状态是否存在
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected StateChangedEvent GetStateChangedEvent(T t) => stateCtrl[t];
        /// <summary>
        /// 根据方法名找到当前类的方法
        /// </summary>
        /// <param name="methodName">State_Method</param>
        MethodInfo Find(string methodName)
        {
            return this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

    }
}
