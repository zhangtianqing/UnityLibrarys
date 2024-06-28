using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;
using Scripts.DataInput;

namespace Scripts.Framework.Communication.UDP.Script
{
    /// <summary>  
    /// UDP����������  
    /// </summary> 
    public class UDPServer : MsgInput
    {

        /// <summary>  
        /// �����˿�����  
        /// </summary>  
        public int PortName = 9998;

        public bool UseSocketToolTest = false;



        UdpClient ReceiveUdpClient;
        [SerializeField]
        private Encoding encoding = Encoding.UTF8;
        Thread myThread;
        public void Thread_Listen()
        {
            myThread = new Thread(ReceiveData);
            //����һ���߳̽���Զ��������������Ϣ  
            myThread.IsBackground = true;
            myThread.Start();
            Debug.Log("UDPServer Started");
        }
        /// <summary>  
        /// ��������  
        /// </summary>  
        private void ReceiveData()
        {
            IPEndPoint local = new IPEndPoint(IPAddress.Any, PortName);// MyIPAddress, PortName);
            ReceiveUdpClient = new UdpClient(local);
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = ReceiveUdpClient.Receive(ref remote);
                    Debug.Log($"recv:{remote}:{BitConverter.ToString(receiveBytes)}");
                    if (receiveBytes.Length == 1)
                    {
                        MessageArrived.Invoke(remote, receiveBytes);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
        private void Update()
        {
        }
        private void Start()
        {
            Thread_Listen();
        }
        Queue<byte[]> queue = new Queue<byte[]>(0);
        private void OnApplicationQuit()
        {
            ReceiveUdpClient?.Dispose();
            ReceiveUdpClient?.Close();
            myThread?.Abort();
        }


    }
}