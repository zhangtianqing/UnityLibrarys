using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPClients
{
    public static void SendUDPMessage(Encoding encoding, string sendStr = "", string ip = "127.0.0.1", int remotePort = 9091)
    {
        if (sendStr == null)
        {
            sendStr = "";
        }
        using (UdpClient client = new UdpClient())
        {

            IPAddress remoteIP = IPAddress.Parse(ip); //���跢�͸����IP
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);//ʵ����һ��Զ�̶˵� 

            byte[] sendData = encoding.GetBytes(sendStr);
            client.Send(sendData, sendData.Length, remotePoint);//�����ݷ��͵�Զ�̶˵� 
        }
    }
    public static void SendUDPMessageDefault(string ip = "127.0.0.1", int remotePort = 9091, string sendStr = "")
    {
        SendUDPMessage(Encoding.Default, sendStr, ip, remotePort);
    }
    public static void SendUDPMessageUTF8(string ip = "127.0.0.1", int remotePort = 9091, string sendStr = "")
    {
        SendUDPMessage(Encoding.UTF8, sendStr, ip, remotePort);
    }
}
