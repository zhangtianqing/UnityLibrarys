using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Scripts.Framework.Communication.UDP.Script
{
    public class UDPClient
    {

        public static void SendUDPMessageDefault(string sendStr = "", string ip = "127.0.0.1", int remotePort = 9091)
        {
            SendUDPMessage(Encoding.Default, sendStr, ip, remotePort);
        }
        public static void SendUDPMessageUTF8(string sendStr = "", string ip = "127.0.0.1", int remotePort = 9091)
        {
            SendUDPMessage(Encoding.UTF8, sendStr, ip, remotePort);
        }

        public static void SendByte(byte[] bytes, string ip = "127.0.0.1", int remotePort = 9091)
        {
            Send(bytes, ip, remotePort);
        }

        #region PrivateMethod
        private static void SendUDPMessage(Encoding encoding, string sendStr = "", string ip = "127.0.0.1", int remotePort = 9091)
        {
            if (sendStr == null)
            {
                sendStr = "";
            }
            Send(encoding.GetBytes(sendStr), ip, remotePort);
        }
        private static void Send(byte[] sendData, string remoteIP, int remotePort)
        {
            using (UdpClient client = new UdpClient())
            {
                //Debug.Log($"remoteIP:{remoteIP},remotePort:{remotePort},sendData:{BitConverter.ToString(sendData)}");
                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);//实例化一个远程端点 
                client.Send(sendData, sendData.Length, remotePoint);//将数据发送到远程端点 
            }
        }
        #endregion
    }
}