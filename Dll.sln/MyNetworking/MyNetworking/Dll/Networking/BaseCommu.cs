using MyNetworking.Dll.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dll.Networking
{
    public abstract class BaseCommu : INetProvidor
    {
        public string id="0";
        public Action<LogType,string> Log;
        /// <summary>
        /// 接收消息
        /// </summary>
        public Action<BaseQueueObject> onMsg;
        /// <summary>
        /// 状态监听
        /// </summary>
        public Action<CommuState> onStateChanged;
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="baseQueueObject.msgType">消息内容类型</param>
        /// <param name="baseQueueObject.msgCode">消息代码</param>
        /// <param name="baseQueueObject.msgBase">消息</param>
        public void Send(BaseQueueObject baseQueueObject)=> outputMsgQueue.Enqueue(baseQueueObject);

        public abstract void SetConfig(ConfigBase configBase);
        public abstract void StartServer();
        public abstract void StopServer();
        protected abstract void SendMsg(BaseQueueObject baseQueueObject);

        public void Update()
        {
            HandlerMsg();
            HandlerEvent();
        }
        int HandlerMax=50;
        ConcurrentQueue<BaseQueueObject> InputMsgQueue = new ConcurrentQueue<BaseQueueObject>();
        ConcurrentQueue<BaseQueueObject> outputMsgQueue = new ConcurrentQueue<BaseQueueObject>();
        ConcurrentQueue<CommuState> eventQueue = new ConcurrentQueue<CommuState>();
        
        void HandlerMsg()
        {
            if (!InputMsgQueue.IsEmpty)
            {
                for (int i = 0; i < HandlerMax; i++)
                {
                    if (InputMsgQueue.TryDequeue(out BaseQueueObject result))
                    {
                        onMsg?.Invoke(result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (!outputMsgQueue.IsEmpty)
            {
                for (int i = 0; i < HandlerMax; i++)
                {
                    if (outputMsgQueue.TryDequeue(out BaseQueueObject result))
                    {
                        SendMsg(result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        void HandlerEvent() {

            if (!eventQueue.IsEmpty)
            {
                for (int i = 0; i < HandlerMax; i++)
                {
                    if (eventQueue.TryDequeue(out CommuState result))
                    {
                        onStateChanged?.Invoke(result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        protected void LogInfo(string msg) => Log?.Invoke(LogType.Log, msg);
        protected void LogWarning(string msg) => Log?.Invoke(LogType.Warning, msg);
        protected void LogError(string msg) => Log?.Invoke(LogType.Error, msg);
        protected void LogAssert(string msg) => Log?.Invoke(LogType.Assert, msg);

    }

}
public class BaseQueueObject {
    public MsgType msgType= MsgType.Object;
    public MsgCode msgCode= MsgCode.None;
    public MsgBase msgBase;
}
public class BaseProtocol { 

}
public class MsgBase {
    public int code = 0;
}
public class Msg_String : MsgBase {
    public string data = "";
}
public class Msg_Bytes : MsgBase {
    public byte[] data;

}
public enum MsgCode { 
    HeartBeat,
    None
}
public enum MsgType { 
    Object,
    String,
    Bytes
}
public enum CommuState { 
    DisConnect,
    Connecting,
    Commuing,
    Closeing,
    Error,
    None
}