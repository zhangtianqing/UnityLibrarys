using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

/// <summary>  
/// UDP����������  
/// </summary> 
public class UDPServers : MonoBehaviour
{
        public delegate void MessageHandler(string Message);//����ί���¼�  
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
        /// �����˿�����  
        /// </summary>  
        public int PortName;


        public static UDPServers UDPServer { get; private set; }

        public void Thread_Listen()
        {
            //����һ���߳̽���Զ��������������Ϣ  
            Thread myThread = new Thread(ReceiveData);
            myThread.IsBackground = true;
            myThread.Start();
        }
        /// <summary>  
        /// ��������  
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
                    //�ر�udpClient ʱ�˾������쳣  
                    byte[] receiveBytes = ReceiveUdpClient.Receive(ref remote);
                    string receiveMessage = Encoding.GetString(receiveBytes, 0, receiveBytes.Length);
                    Debug.Log(string.Format("{0}����{1}:{2}", DateTime.Now.ToString(), remote, receiveMessage));
                    MessageArrived(receiveMessage);
                }
                catch
                {
                    //break;
                }
            }
        }

    
}