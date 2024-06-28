using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace Assets.Scripts.Utils
{
    public static class UnityUtils
    {

        /// <summary>
        /// UnityUtils.IsXRDevicePresent()
        /// </summary>
        /// <returns></returns>
        public static bool IsXRDevicePresent()
        {
#if UNITY_2020_1_OR_NEWER
            return XRSettings.isDeviceActive;
#elif UNITY_2017_2_OR_NEWER
      return XRDevice.isPresent;
#else
      return VRDevice.isPresent;
#endif
        }
        public static void KeyDownToDo(this MonoBehaviour monoBehaviour, KeyCode keyCode, UnityAction unityAction)
        {
            if (Input.GetKeyDown(keyCode))
            {
                unityAction?.Invoke();
            }
        }
        public static T GetOrAddComponent<T>(this MonoBehaviour mb) where T : Component
        {
            T component = mb.GetComponent<T>();
            if (component == null)
            {
                component = mb.gameObject.AddComponent<T>();
            }
            return component;
        }
        public static T FindOrAddComponent<T>(this MonoBehaviour mb) where T : Component
        {
            T component = mb.GetComponent<T>();
            if (component == null)
            {
                component = UnityEngine.Object.FindObjectOfType<T>();
            }
            if (component == null)
            {
                component = mb.gameObject.AddComponent<T>();
            }
            return component;
        }
        public static async void Timer(this MonoBehaviour monoBehaviour, float CTime, UnityAction action)
        {

            await Task.Delay((int)(CTime * 1000));
            action?.Invoke();
        }

        #region 文件名比较
        //调用DLL
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string param1, string param2);

        //前后文件名进行比较。
        public static int FileNameCompare(string name1, string name2)
        {
            if (null == name1 && null == name2)
            {
                return 0;
            }
            if (null == name1)
            {
                return -1;
            }
            if (null == name2)
            {
                return 1;
            }
            return StrCmpLogicalW(name1, name2);
        }
        #endregion

    }
}
