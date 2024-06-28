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
        /// Ѱ��ͨ������Դ����¼�
        /// </summary>
        protected virtual void Awake()
        {
            GetComponent<MsgInput>().MessageArrived += MessageHandle;
        }
        /// <summary>
        /// ��һ������Ϊ����Դͷ,����Ϊ��
        /// </summary>
        protected virtual void MessageHandle(params object[] objs)
        {
            remoteIPEndPoint = (IPEndPoint)objs[0];
            Debug.Log(((IPEndPoint)objs[0]).ToString());
            Debug.Log(BitConverter.ToString(((byte[])objs[1])));
        }

    }
}