using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUnityDll.BaseClass.DataInput
{
    /// <summary>
    /// ��һ��Ϊ���ͷ�Դ��Ϣ������Ϊ����
    /// </summary>
    /// <param name="objects"></param>
    public delegate void MessageHandler(params object[] objects);//����ί���¼�  
    /// <summary>
    /// ��Ϣ����
    /// </summary>
    public class MsgInput : MonoBehaviour
    {
        /// <summary>
        /// ��һ������Ϊ����Դͷ,����Ϊ��
        /// </summary>
        public  MessageHandler MessageArrived;
    }
}