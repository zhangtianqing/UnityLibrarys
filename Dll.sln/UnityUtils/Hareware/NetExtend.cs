using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine;

namespace Dll.UnityUtils

{
    public  class NetExtend
    {
        public static bool IsOnline(TcpClient c)
        {
            return !((c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
        }
        static Coroutine testnet;
        static IEnumerator TestNetwork_Impl(Action<bool,string> checkEvent,bool alway=true)
        {
            do
            {
                UnityWebRequest bd = UnityWebRequest.Get("https://baidu.com");
                yield return bd.SendWebRequest();
                if (bd.result != UnityWebRequest.Result.Success)
                {
                    checkEvent?.Invoke(false, "网络异常-百度连接失败");
                }
                else
                {
                    checkEvent?.Invoke(true, "网络正常");
                }
            }
            while (alway);
            testnet = null;
        }
        /// <summary>
        /// 检测网络链接
        /// </summary>
        /// <param name="checkEvent">结果回调</param>
        /// <param name="alaway">是否一直执行</param>
        public static void CheckNet(Action<bool, string> checkEvent, bool alaway = true)
        {
            if (testnet!=null)
            {
                MonoController.Instance.StopCoroutine(testnet);
            }
            testnet = MonoController.Instance.StartCoroutine(TestNetwork_Impl(checkEvent, alaway));
        }
    }
}
