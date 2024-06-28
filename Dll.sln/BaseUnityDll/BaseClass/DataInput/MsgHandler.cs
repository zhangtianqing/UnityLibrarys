using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Scripts.DataInput
{
    public class MsgHandler : MonoBehaviour
    {
        protected IPEndPoint remoteIPEndPoint;
        /// <summary>
        /// 寻找通用输入源帮绑定事件
        /// </summary>
        protected virtual void Awake()
        {
            GetComponent<MsgInput>().MessageArrived += MessageHandle;
        }
        /// <summary>
        /// 第一个参数为发送源头,可以为空
        /// </summary>
        protected virtual void MessageHandle(params object[] objs)
        {
            remoteIPEndPoint = (IPEndPoint)objs[0];
            Debug.Log(((IPEndPoint)objs[0]).ToString());
            Debug.Log(BitConverter.ToString(((byte[])objs[1])));
        }

    }
}