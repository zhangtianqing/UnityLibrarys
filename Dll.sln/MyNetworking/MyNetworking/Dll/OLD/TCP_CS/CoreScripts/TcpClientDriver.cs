
using Dll.UnityUtils;
using Newtonsoft.Json;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Framework.Communication.TCP_CS.CoreScripts
{
    public class TcpClientDriver
    {
        public Action<MsgMiddleware> onMessage;
        public Action onClose;
        public Action onBind;
        public Action onConnect;

        TcpClient tcpClient;
        IPEndPoint remoteEndPoint;
        public string id = "";

        bool keepConnect = false;
        bool isClosing = false;

        ByteArray readBuff;
        //写入队列
        Queue<ByteArray> writeQueue;
        int countDownTime = 5;
        bool isConnecting = false;

        public TcpClientDriver(string ip, int port, string id, int countDownTime, bool keepConnect = true)
        {
            this.id = id;
            this.keepConnect = keepConnect;

            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);


            this.countDownTime = countDownTime;

            readBuff = new ByteArray();
            //写入队列
            writeQueue = new Queue<ByteArray>();
            //是否正在关闭
            isClosing = false;

            onClose += OnClose;
        }

        private void OnClose()
        {
            Debug.Log("OnClose");
            if (keepConnect && !isConnecting)
            {
                Connect();
            }
        }

        public bool Online()
        {

            if (tcpClient == null || tcpClient.Connected)
            {
                return false;
            }
            Debug.Log("在线监测");
            return NetExtend.IsOnline(tcpClient);
        }
        public void Connect()
        {

            Debug.Log("连接，是否在关闭中:" + isClosing);
            try
            {
                isConnecting = true;
                Debug.Log((tcpClient != null));
                if (!isClosing)
                {
                    tcpClient = new TcpClient();
                    tcpClient.NoDelay = true;
                    Debug.Log("开始连接");
                    tcpClient.BeginConnect(remoteEndPoint.Address, remoteEndPoint.Port, ConnectCallback, tcpClient);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }


        }

        private void ConnectCallback(IAsyncResult ar)
        {

            try
            {
                isConnecting = false;
                Debug.Log("连接回调");
                TcpClient tcp = (TcpClient)ar.AsyncState;
                tcp.EndConnect(ar);


                if (tcp.Connected && NetExtend.IsOnline( tcp))
                {
                    Debug.Log("连接成功");
                    onConnect?.Invoke();
                    cacheTime = countDownTime;
                    HeartBeath();
                    SendMsg(ActionCode.BindIdentity, "{ \"msg\":\"" + id + "\"}");
                    Read();
                }
                else
                {
                    Debug.Log("连接失败，再次连接");
                    onClose?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.Log("连接失败，再次连接");
                onClose?.Invoke();
                Debug.LogError(e);
            }

        }
        int cacheTime = 0;
        async void HeartBeath()
        {
            cacheTime = countDownTime;
            while (!isClosing && cacheTime >= 0)
            {
                await Task.Delay(1000);
                cacheTime -= 1;
                Debug.Log("HeartBeath:" + cacheTime);
                SendMsg(ActionCode.Ping, "C");
            }
            Debug.Log("HeartBeathFinish");
            onClose?.Invoke();
        }
        void Read()
        {
            try
            {
                Debug.Log("接收消息");
                cacheTime = countDownTime;
                NetworkStream networkStream = tcpClient.GetStream();
                //判断有数据再读，否则Read会阻塞线程。后面的业务逻辑无法处理
                networkStream.BeginRead(readBuff.bytes, readBuff.writeIdx,
                readBuff.remain, ReceiveCallback, networkStream);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                onClose?.Invoke();
            }

        }
        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (!NetExtend.IsOnline(tcpClient))
                {
                    throw new Exception("不在线");
                }
                Debug.Log("接收消息异步回调");
                NetworkStream networkStream = (NetworkStream)ar.AsyncState;
                int count = networkStream.EndRead(ar);
                readBuff.writeIdx += count;
                //处理二进制消息
                OnReceiveData();
                //继续接收数据
                if (readBuff.remain < 8)
                {
                    readBuff.MoveBytes();
                    readBuff.ReSize(readBuff.length * 2);
                }
                cacheTime = countDownTime;

                //判断有数据再读，否则Read会阻塞线程。后面的业务逻辑无法处理
                networkStream.BeginRead(readBuff.bytes, readBuff.writeIdx,
                readBuff.remain, ReceiveCallback, networkStream);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                onClose?.Invoke();
            }

        }
        private void OnReceiveData()
        {
            //消息长度 不够4字节读取不了目标长度，直接等待
            if (readBuff.length <= 4)
            {
                return;
            }

            //获取消息体长度
            int bodyLength = BitConverter.ToInt32(readBuff.bytes, readBuff.startIndex);
            //收到的长度不满足
            if (readBuff.length < bodyLength)
                return;
            //偏移到协议
            readBuff.startIndex += 4;
            //解析协议名
            ActionCode ac = (ActionCode)BitConverter.ToInt32(readBuff.bytes, readBuff.startIndex);

            readBuff.startIndex += 4;
            //解析协议体
            int bodyCount = bodyLength - 4;
            byte[] BodyB = new byte[bodyCount];
            Array.Copy(readBuff.bytes, readBuff.startIndex, BodyB, 0, bodyCount);
            readBuff.startIndex += bodyCount;
            readBuff.CheckAndMoveBytes();

            cacheTime = countDownTime;
            Debug.Log($"RecvMsg:{ac},{Encoding.UTF8.GetString(BodyB)}");
            //添加到消息队列
            if (ac == ActionCode.Ping)
            {
                if (Configer.Instance.gameConfig.netSetting.universalNetConfig.ResponsePing)
                {
                    PingMsgHandler(Encoding.UTF8.GetString(BodyB));
                }
            }
            //else if (ac == ActionCode.BindIdentity)
            //{
            //    MsgObjects.StrMsg recv_Stra = JsonConvert.DeserializeObject<MsgObjects.StrMsg>(Encoding.UTF8.GetString(BodyB));
            //    id = recv_Stra.msg;
            //    onBind?.Invoke();
            //}
            else if (ac == ActionCode.None)
            {
                Debug.Log($"None: RecvMsg:{Encoding.UTF8.GetString(BodyB)}");
            }
            else
            {
                onMessage?.Invoke(new MsgMiddleware() { id = id, actionCode = ac, commutBase = Encoding.UTF8.GetString(BodyB) });
            }
            //继续读取消息
            if (readBuff.length > 6)
            {
                OnReceiveData();
            }
        }

        public void Close()
        {
            keepConnect = false;
            if (tcpClient == null)//状态判断
            {
                return;
            }
            if (writeQueue.Count > 0) //还有数据在发送
            {
                Debug.Log("还有数据在发送");
                isClosing = true;
            }
            else//没有数据在发送
            {
                tcpClient.Close();
                tcpClient.Dispose();
                onClose?.Invoke();
            }
        }
        void PingMsgHandler(string str)
        {
        }
        public void SendMsg(ActionCode ac, string commutBase)
        {
            if (!NetExtend.IsOnline(tcpClient) || isClosing)
            {
                //Debug.Log("未连接或关闭中");

                return;
            }
            Debug.Log($"SendCommuMsg:{ac}:{commutBase}");
            //协议
            byte[] actionCode = BitConverter.GetBytes((Int32)ac);
            //数据编码
            byte[] bodyBytes = Encoding.UTF8.GetBytes(commutBase);
            //Int32 4个字节
            int len = 4 + bodyBytes.Length;
            //消息长度字节+消息
            byte[] sendBytes = new byte[4 + len];
            //组装长度
            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            //组装协议号
            Array.Copy(actionCode, 0, sendBytes, 4, actionCode.Length);
            //组装消息体
            Array.Copy(bodyBytes, 0, sendBytes, 8 /*2+4*/, bodyBytes.Length);
            //写入队列
            ByteArray ba = new ByteArray(sendBytes);
            int count = 0;
            lock (writeQueue)
            {
                writeQueue.Enqueue(ba);
                count = writeQueue.Count;
            }
            //send
            if (count == 1)
            {
                tcpClient.GetStream().BeginWrite(sendBytes, 0, sendBytes.Length, WriteCallback, tcpClient.GetStream());
            }
        }
        void WriteCallback(IAsyncResult ar)
        {
            try
            {
                Debug.Log("发送消息异步回调");
                //获取state、EndSend的处理
                NetworkStream network = (NetworkStream)ar.AsyncState;
                network.EndWrite(ar);
                writeQueue.Dequeue();

                //获取写入队列第一条数据            
                ByteArray ba;
                lock (writeQueue)
                {
                    if (writeQueue.Count > 0)
                    {
                        ba = writeQueue.First();
                    }
                    else
                    {
                        ba = null;
                    }
                }
                //继续发送
                if (ba != null)
                {
                    Debug.Log("继续发送");
                    tcpClient.GetStream().BeginWrite(ba.bytes, 0, ba.bytes.Length, WriteCallback, tcpClient.GetStream());
                }
                //正在关闭
                else if (isClosing)
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                onClose?.Invoke();
            }
        }
    }
}
