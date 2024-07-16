using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
namespace Dll.UnityUtils.Hareware
{
    public class LocalNetworkUtil
{
    public static List<IPAddress> GetBroadcasts()
    {

        List<IPAddress> broadcastAddress = new List<IPAddress>();
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
        return broadcastAddress;
    }
    /// <summary> 
    /// 获得广播地址 
    /// </summary> 
    /// <param name="ipAddress">IP地址</param> 
    /// <param name="subnetMask">子网掩码</param> 
    /// <returns>广播地址</returns> 
    public static IPAddress GetBroadcast(IPAddress ipAddress, IPAddress subnetMask)
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
    public static IPAddress GetSubnetMask(IPAddress ipAdd)
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


        /// <summary>
        /// 根据mac地址、网络类型、连接状态判断是否有激活的适配器存在
        /// </summary>
        /// <param name="networkInterfaceType">指定网络接口的类型</param>
        /// <param name="mac">mac地址</param>
        /// <param name="checkMac">是否需要匹配mac地址</param>
        /// <param name="operationalStatus">指定网络接口需要处在的状态</param>
        /// <returns>存在返回，不存在为空</returns>
        public static NetworkInterface GetMac(NetworkInterfaceType networkInterfaceType, byte[] mac, bool checkMac, OperationalStatus operationalStatus = OperationalStatus.Up)
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var iface in interfaces)
        {

            //Debug.Log($"{ iface.Id} { iface.Name} { iface.Description}  { iface.GetIPProperties().ToString() }  { iface.NetworkInterfaceType} { iface.OperationalStatus} { iface.Speed} { BitConverter.ToString(iface.GetPhysicalAddress().GetAddressBytes())}");
            if (iface.OperationalStatus == OperationalStatus.Up
                && iface.NetworkInterfaceType == networkInterfaceType
                && (checkMac ? mac.ByteCheck(iface.GetPhysicalAddress().GetAddressBytes()) : true))
            {
                return iface;
            }
        }
        return null;
    }
    public static NetworkInterface GetMac(NetworkInterfaceType networkInterfaceType, OperationalStatus operationalStatus = OperationalStatus.Up)
    {
        return GetMac(networkInterfaceType, null, false, operationalStatus);
    }
    public static NetworkInterface GetMacForWireless()
    {
        return GetMac(NetworkInterfaceType.Wireless80211, null, false, OperationalStatus.Up);
    }
}
}