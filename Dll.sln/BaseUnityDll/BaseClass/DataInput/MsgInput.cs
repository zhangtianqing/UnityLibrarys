using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUnityDll.BaseClass.DataInput
{
    /// <summary>
    /// 第一个为发送方源信息，后面为数据
    /// </summary>
    /// <param name="objects"></param>
    public delegate void MessageHandler(params object[] objects);//定义委托事件  
    /// <summary>
    /// 消息输入
    /// </summary>
    public class MsgInput : MonoBehaviour
    {
        /// <summary>
        /// 第一个参数为发送源头,可以为空
        /// </summary>
        public  MessageHandler MessageArrived;
    }
}