using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.DataInput
{
    public delegate void MessageHandler(params object[] objects);//定义委托事件  
    public class MsgInput : MonoBehaviour
    {
        /// <summary>
        /// 第一个参数为发送源头,可以为空
        /// </summary>
        public MessageHandler MessageArrived;
    }
}