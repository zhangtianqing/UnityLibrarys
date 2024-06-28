
using Assets.Scripts.Framework.Communication.Interface;
using Assets.Scripts.Framework.Communication.TCP.TCP_CS;
using Assets.Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Framework.Communication.TCP
{
    public class TcpServerCompoment : BaseCommuCompoment
    {
        //定义套接字
        Socket Server;

        EndPoint ServerBehind;
        int MAX_MESSAGE_FIRE = 30;
        bool systemClose = false;

        object lockClient = new object();
        object lockRecvMsg = new object();
        object lockSendMsg = new object();

        /// <summary>
        /// 消息队列
        /// </summary>
        Dictionary<string, Queue<MsgMiddleware>> queueRecvMsgs = new Dictionary<string, Queue<MsgMiddleware>>();
        Dictionary<string, Queue<MsgMiddleware>> queueSendMsgs = new Dictionary<string, Queue<MsgMiddleware>>();
        Queue<Action> queueEvent = new Queue<Action>();


        Dictionary<Socket, NetClient> Clients = new Dictionary<Socket, NetClient>();
        Dictionary<string, NetClient> idRecord = new Dictionary<string, NetClient>();

        MsgMiddleware msgMiddleware = null;
        BaseCommuHandler baseActionCodeClass = null;


        void EndAccept(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;
            Socket client = socket.EndAccept(asyncResult);
            try
            {
                NetClient netClient = new NetClient(client);
                netClient.onBind += ClientBindID;
                netClient.onMessage += OnClientMsg;
                netClient.onClose += RemoveClient;
                lock (lockClient)
                {
                    Clients.Add(client, netClient);
                }

                socket.BeginAccept(EndAccept, socket);
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(System.ObjectDisposedException))
                {
                    Clients[client].Close();
                }
                if (!systemClose)
                {
                    Server.BeginAccept(EndAccept, Server);
                }
            }
        }
        #region msgHandler
        public void OnClientMsg(MsgMiddleware msg)
        {

        }
        public void OnClientMsg(string id, ActionCode actionCode, string msg)
        {
            MsgMiddleware msgMiddleware = new MsgMiddleware() { id = id, actionCode = actionCode, commutBase = msg };

            if (!queueRecvMsgs.ContainsKey(id))
            {
                lock (lockRecvMsg)
                {
                    if (!queueRecvMsgs.ContainsKey(id))
                    {
                        queueRecvMsgs.Add(id, new Queue<MsgMiddleware>());
                        Debug.Log("初始化消息订阅:" + id);
                    }
                }
            }
            Debug.Log("接收消息入队：" + msgMiddleware/*.ToStringU()*/);
            queueRecvMsgs[id].Enqueue(msgMiddleware);
        }

        public override void SendMsg(string id, ActionCode ac, string msg)
        {
            MsgMiddleware msgMiddleware = new MsgMiddleware() { id = id, actionCode = ac, commutBase = msg };
            if (!queueSendMsgs.ContainsKey(id))
            {
                lock (lockSendMsg)
                {

                    if (!queueSendMsgs.ContainsKey(id))
                    {
                        queueSendMsgs.Add(id, new Queue<MsgMiddleware>());
                    }
                }
            }
            Debug.Log("发送消息入队：" + msgMiddleware/*.ToStringU()*/);
            queueSendMsgs[id].Enqueue(msgMiddleware);
        }

        /// <summary>
        /// 重复处理消息
        /// </summary>
        public override void Update()
        {
            lock (lockRecvMsg)
            {
                foreach (var item in queueRecvMsgs.Keys)
                {
                    for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
                    {
                        if (queueRecvMsgs[item].Count > 0)
                        {
                            msgMiddleware = queueRecvMsgs[item].Dequeue();
                            baseActionCodeClass = BaseCommuDisptch.Instance.GetHandle(msgMiddleware.id);

                            if (null != baseActionCodeClass)
                            {
                                baseActionCodeClass.MsgHandler(msgMiddleware.actionCode, msgMiddleware.commutBase);
                            }
                            else
                            {
                                Debug.Log("baseActionCodeClass Null!");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"{item}指定的消息队列没消息");
                            break;
                        }
                    }
                }
            }
            lock (lockRecvMsg)
            {
                int index = 0;
                foreach (var item in queueSendMsgs.Keys)
                {
                    if (idRecord.ContainsKey(item) && queueSendMsgs[item].Count > 0)
                    {

                        msgMiddleware = queueSendMsgs[item].Dequeue();
                        Debug.Log("发送消息:" + msgMiddleware/*.ToStringU()*/);
                        idRecord[item].Send(new MsgBase() { actionCode = msgMiddleware.actionCode, commutBase = msgMiddleware.commutBase });

                    }
                    else
                    {
                        queueSendMsgs[item].Clear();
                    }
                    index++;
                    if (index > MAX_MESSAGE_FIRE)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
            {
                if (queueEvent.Count > 0)
                {
                    queueEvent.Dequeue().Invoke();
                }
            }
            msgMiddleware = null;
            baseActionCodeClass = null;
        }
        public void ClientBindID(NetClient netClient)
        {
            if (idRecord.ContainsKey(netClient.id))
            {
                try
                {
                    idRecord[netClient.id].Send(new MsgBase() { actionCode = ActionCode.TestMsg, commutBase = "you are offline" });
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                idRecord[netClient.id].Close();
                idRecord.Remove(netClient.id);
            }

            idRecord.Add(netClient.id, netClient);
            queueEvent.Enqueue(() =>
            {
                FireEvent(netClient.id, NetEvent.BindIdentity, "身份绑定成功！");
            });
        }
        #endregion


        #region DefineMethod
        public override void StartService()
        {
            MAX_MESSAGE_FIRE = Configer.Instance.gameConfig.netSetting.universalNetConfig.handlerMsgCount;

            if (Server != null)
            {
                Debug.Log("Server Already Start");
                return;
            }

            ServerBehind = new IPEndPoint(IPAddress.Any, Configer.Instance.gameConfig.netSetting.universalNetConfig.localPort);

            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(ServerBehind);
            Server.Listen(0);
            Server.BeginAccept(EndAccept, Server);

            Debug.Log($"{nameof(TcpServerCompoment)}服务启动完成！端口:{((IPEndPoint)ServerBehind).Port}");
        }

        public override void StopService()
        {
            systemClose = true;

            Server.Dispose();
            Server.Close();

            lock (lockClient)
            {
                foreach (var item in Clients)
                {
                    try
                    {
                        item.Value.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("服务关闭时异常:" + e);
                    }
                }
            }
            Debug.Log("服务关闭完成");
        }
        #endregion

        //关闭连接
        public void RemoveClient(NetClient netClient)
        {

            queueEvent.Enqueue(() =>
            {
                FireEvent(netClient.id, NetEvent.Close, "Close");
            });
            Debug.Log($"{netClient.id}:NetEvent.Close 事件");

            lock (lockRecvMsg)
            {
                queueRecvMsgs.Remove(netClient.id);
            }
            lock (lockSendMsg)
            {
                queueSendMsgs.Remove(netClient.id);
            }
            lock (lockClient)
            {
                Clients.Remove(netClient.client);
            }
        }
    }
}
