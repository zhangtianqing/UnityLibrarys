using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.DataInput
{
    public delegate void MessageHandler(params object[] objects);//����ί���¼�  
    public class MsgInput : MonoBehaviour
    {
        /// <summary>
        /// ��һ������Ϊ����Դͷ,����Ϊ��
        /// </summary>
        public MessageHandler MessageArrived;
    }
}