using System;
using System.IO.Ports;
using System.Text;
using System.Timers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;


namespace Dll.UnityUtils.Hareware
{
    /// <summary>
    /// 串口 公共资源类
    /// </summary>
    public class SerialPortUtility
    {
        /// <summary>
        /// 串口是否已打开
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 初始化 串行端口资源
        /// </summary>
        private SerialPort mySerialPort = new SerialPort();

        /// <summary>
        /// 串口接收数据 位置
        /// </summary>
        private static int pSerialPortRecv = 0;

        /// <summary>
        /// 缓存区大小的长度
        /// 缓冲区可调大
        /// （接收数据处理定时器 内接收数据量 小于下面设置的值即可）
        /// </summary>
        private static int byteLength = 40960;

        /// <summary>
        /// 串口接收字节 缓存区大小
        /// </summary>
        private byte[] byteSerialPortRecv = new byte[byteLength];

        /// <summary>
        /// 串口 接收数据处理定时器
        /// </summary>
        private System.Timers.Timer SerialPortRecvTimer;

        /// <summary>
        /// 广播 收到的数据 事件
        /// </summary>
        public event EventHandler<SerialPortRecvEventArgs> ReceivedDataEvent;

        public event EventHandler ReConnectionEvent;//自动重连后，执行事件

        private bool IsAutoReconnectionStatus = false;//是否自动重连

        private int DelayTime { get; set; }//自动重连检测间隔时间

        /// <summary>
        /// 广播 收到的数据
        /// </summary>
        public class SerialPortRecvEventArgs : EventArgs
        {
            /// <summary>
            /// 广播 收到的串口数据
            /// </summary>
            public readonly byte[] RecvData = new byte[byteLength];

            /// <summary>
            /// 收到数据 的 长度
            /// </summary>
            public readonly int RecvDataLength;

            /// <summary>
            /// 将 收到的数据 转化成 待广播的数据
            /// </summary>
            public SerialPortRecvEventArgs(byte[] recvData, int recvDataLength)
            {
                recvData.CopyTo(RecvData, 0);
                RecvDataLength = recvDataLength;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SerialPortUtility()
        {
            IsOpen = false;
            AutoConnection();
        }

        /// <summary>
        /// 设置 串口配置
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        private void SetSerialPortConfig(string portName, int baudRate, int parity, int dataBits, int stopBits)
        {
            // 串口 参数设置
            mySerialPort.PortName = portName;
            mySerialPort.BaudRate = baudRate;
            switch (parity)
            {
                case 0:
                default:
                    mySerialPort.Parity = Parity.None;
                    break;

                case 1:
                    mySerialPort.Parity = Parity.Odd;
                    break;

                case 2:
                    mySerialPort.Parity = Parity.Even;
                    break;

                case 3:
                    mySerialPort.Parity = Parity.Mark;
                    break;

                case 4:
                    mySerialPort.Parity = Parity.Space;
                    break;
            }
            mySerialPort.DataBits = ((4 < dataBits) && (dataBits < 9)) ? dataBits : 8;
            switch (stopBits)
            {
                case 0:
                    mySerialPort.StopBits = StopBits.None;
                    break;

                case 1:
                default:
                    mySerialPort.StopBits = StopBits.One;
                    break;

                case 2:
                    mySerialPort.StopBits = StopBits.OnePointFive;
                    break;

                case 3:
                    mySerialPort.StopBits = StopBits.Two;
                    break;
            }
            mySerialPort.ReadTimeout = -1;
            mySerialPort.ReceivedBytesThreshold = 1;
            mySerialPort.RtsEnable = true;
            mySerialPort.DtrEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            // 串口 接收数据处理定时器 参数设置
            SerialPortRecvTimer = new System.Timers.Timer();
            SerialPortRecvTimer.Interval = 100;
            SerialPortRecvTimer.AutoReset = false;
            SerialPortRecvTimer.Elapsed += new ElapsedEventHandler(SPRecvTimer_Tick);
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="parity">校验位</param>
        /// <param name="stopBits">停止位</param>
        /// <returns>是否成功打开</returns>
        public bool OpenSerialPort(string portName, int baudRate, int dataBits, int parity, int stopBits, bool autoConnection = false)
        {
            try
            {

                SetSerialPortConfig(portName, baudRate, parity, dataBits, stopBits);
                mySerialPort.Open();
                IsOpen = true;
                return true;
            }
            catch (System.Exception)
            {
                IsOpen = false;
                return false;
            }
        }

        public void SetAutoConnection(bool auto, int delay)
        {
            IsAutoReconnectionStatus = auto;
            DelayTime = delay;
        }
        private void AutoConnection()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (IsAutoReconnectionStatus)
                    {
                        try
                        {
                            if (mySerialPort != null && !mySerialPort.IsOpen)
                            {
                                mySerialPort.Open();
                                IsOpen = true;
                                ReConnectionEvent?.Invoke(this, null);
                            }
                        }
                        catch (Exception)
                        {
                            IsOpen = false;
                            ReConnectionEvent?.Invoke(this, null);
                        }
                    }
                    Thread.Sleep(DelayTime);
                }
            });
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void CloseSerialPort()
        {
            try
            {
                mySerialPort.Close();
                IsOpen = false;
            }
            catch (System.Exception)
            {
                IsOpen = false;
                throw;
            }
        }

        /// <summary>
        /// 串口数据发送
        /// </summary>
        /// <param name="content">byte类型数据</param>
        public void SendData(byte[] content)
        {
            try
            {
                mySerialPort.Write(content, 0, content.Length);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 串口数据发送
        /// </summary>
        /// <param name="strContent">字符串数据</param>
        /// <param name="encoding">编码规则</param>
        public void SendData(string strContent, Encoding encoding)
        {
            try
            {
                byte[] content = strToHexByte(strContent);//encoding.GetBytes(strContent);
                mySerialPort.Write(content, 0, content.Length);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        private byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 数据处理定时器
        /// 定时检查缓冲区是否有数据，如果有数据则将数据处理并广播。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SPRecvTimer_Tick(object sender, EventArgs e)
        {
            byte[] TemporaryData = new byte[byteLength];
            int TemporaryDataLength = 0;

            UnityEngine.Debug.Log("SPRecvTimer_Tick");
            if (ReceivedDataEvent != null)
            {
                byteSerialPortRecv.CopyTo(TemporaryData, 0);
                TemporaryDataLength = pSerialPortRecv;
                UnityEngine.Debug.Log("SPRecvTimer_Tick-Data:" + BitConverter.ToString(TemporaryData));

                ReceivedDataEvent.Invoke(this, new SerialPortRecvEventArgs(TemporaryData, TemporaryDataLength));
                // 数据处理完后，将指针指向数据头，等待接收新的数据
                pSerialPortRecv = 0;
            }
        }

        /// <summary>
        /// 数据接收事件
        /// 串口收到数据后，关闭定时器，将收到的数据填入缓冲区，数据填入完毕后，开启定时器，等待下一次数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                UnityEngine.Debug.Log("DR");
                SerialPortRecvTimer.Stop();

                byte[] ReadBuf = new byte[mySerialPort.BytesToRead];
                mySerialPort.Read(ReadBuf, 0, ReadBuf.Length);
                ReadBuf.CopyTo(byteSerialPortRecv, pSerialPortRecv);
                pSerialPortRecv += ReadBuf.Length;

                SerialPortRecvTimer.Start();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前可用PortName
        /// </summary>
        /// <returns></returns>
        public List<SPParameterClass<string>> GetPortList()
        {
            try
            {
                List<SPParameterClass<string>> lst_sParameterClass = new List<SPParameterClass<string>>();
                foreach (string data in SerialPort.GetPortNames())
                {
                    SPParameterClass<string> i_sParameterClass = new SPParameterClass<string>();
                    i_sParameterClass.Name = data;
                    i_sParameterClass.Value = data;
                    lst_sParameterClass.Add(i_sParameterClass);
                }

                return lst_sParameterClass;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <returns></returns>
        public static List<SPParameterClass<int>> SetBaudRateValues()
        {
            try
            {
                List<SPParameterClass<int>> lst_sParameterClass = new List<SPParameterClass<int>>();
                foreach (SerialPortBaudRates rate in Enum.GetValues(typeof(SerialPortBaudRates)))
                {
                    SPParameterClass<int> i_sParameterClass = new SPParameterClass<int>();
                    i_sParameterClass.Name = ((int)rate).ToString();
                    i_sParameterClass.Value = (int)rate;
                    lst_sParameterClass.Add(i_sParameterClass);
                }

                return lst_sParameterClass;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }



    /// <summary>
    /// 串口参数类
    /// </summary>
    /// <typeparam name="T">value选择需要使用的值</typeparam>
    public class SPParameterClass<T>
    {
        /// <summary>
        /// 显示值
        /// </summary>
        string name;

        /// <summary>
        /// 显示值
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        T value;

        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }

    /// <summary>
    /// 串口波特率列表。
    /// 75,110,150,300,600,1200,2400,4800,9600,14400,19200,28800,38400,56000,57600,
    /// 115200,128000,230400,256000
    /// </summary>
    public enum SerialPortBaudRates
    {
        BaudRate_75 = 75,
        BaudRate_110 = 110,
        BaudRate_150 = 150,
        BaudRate_300 = 300,
        BaudRate_600 = 600,
        BaudRate_1200 = 1200,
        BaudRate_2400 = 2400,
        BaudRate_4800 = 4800,
        BaudRate_9600 = 9600,
        BaudRate_14400 = 14400,
        BaudRate_19200 = 19200,
        BaudRate_28800 = 28800,
        BaudRate_38400 = 38400,
        BaudRate_56000 = 56000,
        BaudRate_57600 = 57600,
        BaudRate_115200 = 115200,
        BaudRate_128000 = 128000,
        BaudRate_230400 = 230400,
        BaudRate_256000 = 256000
    }

}
