

using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Framework.Communication.TCP.TCP_CS
{

    public class NetClient
    {
        public Socket client;
        public string id = "";
        int countTimeInit = 5;
        int countTime = 0;
        ByteArray readBuff;
        //写入队列
        Queue<ByteArray> writeQueue;
        //是否正在关闭
        bool isClosing = false;
        //连上的时间戳
        long lastRecvTime = 0;
        public Action<NetClient> onBind;
        Action close;
        public Action<NetClient> onClose;
        public Action<string, ActionCode, string> onMessage;

        public NetClient(Socket cl)
        {
            client = cl;
            client.NoDelay = true;

            readBuff = new ByteArray();
            //写入队列
            writeQueue = new Queue<ByteArray>();
            //是否正在关闭
            isClosing = false;
            lastRecvTime = DateTime.Now.ToLongTimeStamp();
            countTimeInit = Configer.Instance.gameConfig.netSetting.universalNetConfig.PPTime;
            countTime = countTimeInit;
            CheckStatus();
            Receive();
            close += Close;
        }
        #region 发送数据
        public void Send(MsgBase msgBase)
        {
            //状态判断
            if (client == null || !client.Connected)
            {
                return;
            }
            if (isClosing)
            {
                return;
            }

            //协议
            byte[] actionCode = BitConverter.GetBytes((Int32)msgBase.actionCode);
            //数据编码
            byte[] bodyBytes = Encoding.UTF8.GetBytes(msgBase.commutBase);
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
                client?.BeginSend(sendBytes, 0, sendBytes.Length,
                    0, SendCallback, client);
            }
        }

        //Send回调
        private void SendCallback(IAsyncResult ar)
        {
            //获取state、EndSend的处理
            Socket socket = (Socket)ar.AsyncState;
            //状态判断
            if (socket == null || !socket.Connected)
            {
                return;
            }
            try
            {
                //EndSend
                int count = socket.EndSend(ar);
                //获取写入队列第一条数据            
                ByteArray ba;
                lock (writeQueue)
                {
                    ba = writeQueue.First();
                }
                //完整发送
                ba.startIndex += count;
                if (ba.length == 0)
                {
                    lock (writeQueue)
                    {
                        writeQueue.Dequeue();
                        if (writeQueue.Count > 0)
                        {
                            ba = writeQueue.First();
                        }
                        else
                        {
                            ba = null;
                        }
                    }
                }
                //继续发送
                if (ba != null)
                {
                    lastRecvTime = DateTime.Now.ToLongTimeStamp();
                    Debug.Log("继续发送");
                    socket.BeginSend(ba.bytes, ba.startIndex, ba.length,
                        0, SendCallback, socket);
                }
                //正在关闭
                else if (isClosing)
                {
                    close?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                close?.Invoke();
            }

        }
        #endregion
        #region 接收消息
        public void Receive()
        {
            //Debug.Log("开始接收消息");
            client.BeginReceive(readBuff.bytes, readBuff.writeIdx,
                    readBuff.remain, 0, ReceiveCallback, client);
        }
        //Receive回调
        private void ReceiveCallback(IAsyncResult ar)
        {
            //Debug.Log("接收消息回调");
            Socket socket = (Socket)ar.AsyncState;
            //获取接收数据长度
            int count = socket.EndReceive(ar);
            readBuff.writeIdx += count;
            //处理二进制消息
            OnReceiveData();
            //继续接收数据
            if (readBuff.remain < 8)
            {
                readBuff.MoveBytes();
                readBuff.ReSize(readBuff.length * 2);
            }

            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx,
                    readBuff.remain, 0, ReceiveCallback, socket);
            try
            {

            }
            catch (Exception e)
            {
                Debug.LogError("故障断开:" + id + "," + e.ToString());
                close?.Invoke();
            }

        }

        //数据处理
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

            lastRecvTime = DateTime.Now.ToLongTimeStamp();

            Debug.Log($"ID:{id},AC:{ac} RecvMsg:{Encoding.UTF8.GetString(BodyB)}");
            countTime = countTimeInit;
            //添加到消息队列
            if (ac == ActionCode.Ping && Configer.Instance.gameConfig.netSetting.universalNetConfig.ResponsePing)
            {
                PingMsgHandler(Encoding.UTF8.GetString(BodyB));
            }
            else if (ac == ActionCode.BindIdentity)
            {
                RecvMsgObject.Recv_Stra recv_Stra = Newtonsoft.Json.JsonConvert.DeserializeObject<RecvMsgObject.Recv_Stra>(Encoding.UTF8.GetString(BodyB));
                id = recv_Stra.msg;
                onBind?.Invoke(this);
            }
            else
            {
                //身份绑定完成才可通讯
                if (id != "")
                {
                    onMessage?.Invoke(id, ac, Encoding.UTF8.GetString(BodyB));
                }
                else
                {
                    Debug.Log("身份未绑定");
                }
            }
            //继续读取消息
            if (readBuff.length > 6)
            {
                OnReceiveData();
            }
        }

        #endregion
        #region 特殊处理
        public void Close()
        {
            Debug.Log("关闭！");
            //状态判断
            if (client == null || !client.Connected)
            {
                return;
            }
            //还有数据在发送
            if (writeQueue.Count > 0)
            {
                isClosing = true;
            }
            //没有数据在发送
            else
            {
                client.Dispose();
                client.Close();
                onClose?.Invoke(this);
                client = null;
            }
        }

        async void CheckStatus()
        {

            countTime = countTimeInit;
            while (countTime >= 0 && !isClosing)
            {
                await Task.Delay(1000);
                countTime -= 1;
                Debug.Log("CheckStatus:" + countTime);
            }
            close?.Invoke();
            if (!isClosing)
            {
                Debug.Log($"连接{id}:心跳异常");
            }
            else
            {
                Debug.Log($"连接{id}:服务关闭 断开连接");
            }
        }
        void PingMsgHandler(string str)
        {
            //Debug.Log("PingMsgHandler:" + str);
            Send(new MsgBase() { actionCode = ActionCode.Pong, commutBase = str });
        }
        #endregion
    }
}
