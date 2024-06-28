
using Dll.Framework.Configer.ConfigClass.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Scripts.Framework.Communication.UDP.Script
{

    /// <summary>
    /// 自动上线 广播端
    /// 通过对这个远程端口广播密钥确定本服务的ip地址并完成通讯
    /// </summary>
    public class OnLineKeeper : MonoBehaviour
    {
        [SerializeField]
        OnLineConfig onLineConfig { get { return Configer.Instance.gameConfig.netSetting.netSetting_OnLineConfig; } }
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"MechineName::{Dns.GetHostName()}");
            Debug.Log($"{onLineConfig == null}:");
            offsetTime = onLineConfig.onlineDeteTime;

            OnlineCheck();
        }
        float lastTime;
        float offsetTime;
        private void Update()
        {
            if (lastTime + offsetTime < Time.time)
            {
                lastTime = Time.time;
                OnlineCheck();
            }
        }

        private void OnlineCheck()
        {
            UpdateBroadcast();
            Debug.Log($"{this.GetType().Name}:" + "上线广播");
            foreach (var item in broadcastAddress)
            {
                Debug.Log($"{this.GetType().Name}:" + "目标广播地址:" + item.ToString());
                UDPClient.SendByte(Encoding.ASCII.GetBytes(onLineConfig.onlineStr), item.ToString(), onLineConfig.onlinePort);
            }
        }

        List<IPAddress> broadcastAddress = new List<IPAddress>();
        void UpdateBroadcast()
        {
            broadcastAddress.Clear();
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < ips.Length; i++)
            {
                if (i != 0 && ips[i].ToString().IndexOf(":") == -1)
                {
                    try
                    {
                        IPAddress iPAddress = GetSubnetMask(ips[i]);
                        broadcastAddress.Add(GetBroadcast(ips[i], iPAddress));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        /// <summary> 
        /// 获得广播地址 
        /// </summary> 
        /// <param name="ipAddress">IP地址</param> 
        /// <param name="subnetMask">子网掩码</param> 
        /// <returns>广播地址</returns> 
        IPAddress GetBroadcast(IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] ipAdd = ipAddress.GetAddressBytes();
            byte[] subnet = subnetMask.GetAddressBytes();
            for (int i = 0; i < ipAdd.Length; i++)
            {
                ipAdd[i] = (byte)(~subnet[i] | ipAdd[i]);
            }
            return new IPAddress(ipAdd);
        }
        /// <summary>
        /// 获取子网掩码
        /// </summary>
        /// <param name="ipAdd">IP地址</param>
        /// <returns></returns>
        IPAddress GetSubnetMask(IPAddress ipAdd)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (ipAdd.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            //return IPAddress.Parse("255.255.255.0");
            throw new ArgumentException($"没有发现该{ipAdd}对应的子网掩码");
        }

    }
}