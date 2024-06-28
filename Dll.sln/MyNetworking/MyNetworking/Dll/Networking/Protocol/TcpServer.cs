using Assets.Scripts.Framework.Communication.TCP.TCP_CS;
using Dll.Networking;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyNetworking.Dll.Networking.Protocol
{
    internal class TcpServer : BaseCommu
    {
        TcpServerConfig tcpServerConfig;
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
        public TcpServer() { }
        public TcpServer(TcpServerConfig tcpServerConfig) {
            this.tcpServerConfig = tcpServerConfig;
        }
        public override void SetConfig(ConfigBase configBase)
        {
            this.tcpServerConfig = (TcpServerConfig)configBase;
        }

        public override void StartServer()
        {
            if (ServerBehind!=null)
            {
                ServerBehind.
            }
        }

        public override void StopServer()
        {
        }

        protected override void SendMsg(BaseQueueObject baseQueueObject)
        {
        }
    }
    public class TcpServerConfig : ConfigBase { 
    
    }
}
