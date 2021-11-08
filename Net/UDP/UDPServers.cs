using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

/// <summary>  
/// UDP服务器对象  
/// </summary> 
public class UDPServers : MonoBehaviour
{
        public delegate void MessageHandler(string Message);//定义委托事件  
        public event MessageHandler MessageArrived;
        public UDPServers()
        {
            PortName = 8080;//tomcat
            Encoding = Encoding.UTF8;
        }

        public UDPServers(Encoding encoding,int port) {
            Encoding = encoding;
            PortName = port;
        }

        public UdpClient ReceiveUdpClient;

        public Encoding Encoding;
        /// <summary>  
        /// 侦听端口名称  
        /// </summary>  
        public int PortName;


        public static UDPServers UDPServer { get; private set; }

        public void Thread_Listen()
        {
            //创建一个线程接收远程主机发来的信息  
            Thread myThread = new Thread(ReceiveData);
            myThread.IsBackground = true;
            myThread.Start();
        }
        /// <summary>  
        /// 接收数据  
        /// </summary>  
        private void ReceiveData()
        {
            IPEndPoint local = new IPEndPoint(IPAddress.Any,PortName);// MyIPAddress, PortName);
            ReceiveUdpClient = new UdpClient(local);
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    //关闭udpClient 时此句会产生异常  
                    byte[] receiveBytes = ReceiveUdpClient.Receive(ref remote);
                    string receiveMessage = Encoding.GetString(receiveBytes, 0, receiveBytes.Length);
                    Debug.Log(string.Format("{0}来自{1}:{2}", DateTime.Now.ToString(), remote, receiveMessage));
                    MessageArrived(receiveMessage);
                }
                catch
                {
                    //break;
                }
            }
        }

    
}