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
    /// ��ù㲥��ַ 
    /// </summary> 
    /// <param name="ipAddress">IP��ַ</param> 
    /// <param name="subnetMask">��������</param> 
    /// <returns>�㲥��ַ</returns> 
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
    /// ��ȡ��������
    /// </summary>
    /// <param name="ipAdd">IP��ַ</param>
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
        throw new ArgumentException($"û�з��ָ�{ipAdd}��Ӧ����������");
    }


        /// <summary>
        /// ����mac��ַ���������͡�����״̬�ж��Ƿ��м��������������
        /// </summary>
        /// <param name="networkInterfaceType">ָ������ӿڵ�����</param>
        /// <param name="mac">mac��ַ</param>
        /// <param name="checkMac">�Ƿ���Ҫƥ��mac��ַ</param>
        /// <param name="operationalStatus">ָ������ӿ���Ҫ���ڵ�״̬</param>
        /// <returns>���ڷ��أ�������Ϊ��</returns>
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