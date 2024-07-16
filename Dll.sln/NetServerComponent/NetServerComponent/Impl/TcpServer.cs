using NetServerComponent.Base;
using NetServerComponent.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NetServerComponent.Impl.TcpServer;

namespace NetServerComponent.Impl
{
    public class TcpServer : NetInterface
    {
        Socket socket;
        public int maxConnect=int.MaxValue;
        public int port=8889;
        public int timeout;
        public string bind="0.0.0.0";
        public uint clientIndex =0;
        public Dictionary<uint, Client> clients=new Dictionary<uint, Client>();
        public void StartService()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(bind), port));
            socket.Listen(maxConnect);
            socket.BeginAccept(OnAccept,socket);
        }

        private void OnAccept(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState ;
            Socket c= s.EndAccept(ar);
            Client client = new Client();
            client.id = clientIndex++;
            client.socket = c;

            clients.Add(client.id, client);

            //c.BeginReceive(OnRecvMsg);

            
        }

        public void StopService()
        {
        }
        public void SendMsg(MsgData msg)
        {
        }
        
        public void SetOnMsg(Action<MsgData> action)
        {
        }
        public class Client {
            public uint id;
            public Socket socket;
        }
    }
}
