using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Dll.UnityUtils
{
    /// <summary>
    /// 对BitConverter进行方法扩展
    /// </summary>
    public static class BitConverterExtend
    {
        /// <summary>
        /// 得到数值的网络字节序（大端模式）的字节数组
        /// </summary>
        public static byte[] GetBytes_Network(this short num)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
        }
        public static byte[] GetBytes_Network(this ushort num)
        {
            return ((short)num).GetBytes_Network();  //这样比反转数组快
        }
        public static byte[] GetBytes_Network(this int num)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
        }
        public static byte[] GetBytes_Network(this uint num)
        {
            return ((int)num).GetBytes_Network();
        }
        public static byte[] GetBytes_Network(this long num)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num));
        }
        public static byte[] GetBytes_Network(this ulong num)
        {
            return ((long)num).GetBytes_Network();
        }
        public static byte[] GetBytes_Network(this float num)
        {
            return BitConverter.GetBytes(num).Reverse().ToArray();
        }
        /// <summary>
        /// 大小端转换
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static byte[] GetBytes_Network(this double num)
        {
            return BitConverter.GetBytes(num).Reverse().ToArray();
        }
        /// <summary>
        /// UTF-8 编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetBytes_UTF8(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        /// <summary>
        /// 将网络字节序（大端模式）的字节数组转成本地字节序（小端模式）的数值
        /// </summary>
        public static short ToInt16_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(networkBytes, startIndex));  //传入引用和起始索引，减少一个数组拷贝
        }
        public static ushort ToUInt16_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return (ushort)(networkBytes[startIndex++] << 8 | networkBytes[startIndex]); //直接自己按位操作，这样最快
        }
        public static int ToInt32_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(networkBytes, startIndex));
        }
        public static uint ToUInt32_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return (uint)(networkBytes[startIndex++] << 24 | networkBytes[startIndex++] << 16
                | networkBytes[startIndex++] << 8 | networkBytes[startIndex]);
        }
        public static long ToInt64_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(networkBytes, startIndex));
        }
        public static ulong ToUInt64_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return (ulong)(networkBytes[startIndex++] << 56 | networkBytes[startIndex++] << 48
                | networkBytes[startIndex++] << 40 | networkBytes[startIndex++] << 32
                | networkBytes[startIndex++] << 24 | networkBytes[startIndex++] << 16
                | networkBytes[startIndex++] << 8 | networkBytes[startIndex]);
        }
        public static float ToFloat_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return BitConverter.ToSingle(CopyAndReverse(networkBytes, startIndex, 4), 0);
        }
        /// <summary>
        /// 拷贝反转
        /// </summary>
        static byte[] CopyAndReverse(byte[] networkBytes, int startIndex, int len)
        {
            byte[] bs = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bs[len - 1 - i] = networkBytes[startIndex + i];  //反转拷贝
            }
            return bs;
        }
        public static double ToDouble_ByNetworkBytes(this byte[] networkBytes, int startIndex)
        {
            return BitConverter.ToDouble(CopyAndReverse(networkBytes, startIndex, 8), 0);
        }
        public static string GetString_UTF8(this byte[] value, int startIndex, int count)
        {
            return Encoding.UTF8.GetString(value, startIndex, count);
        }
        public static string GetString_UTF8(this byte[] value)
        {
            return Encoding.UTF8.GetString(value, 0, value.Length);
        }
        /// <summary>
        /// 判断字节是否按序相等
        /// </summary>
        /// <param name="value">源值</param>
        /// <param name="check">检测值</param>
        /// <returns></returns>
        public static bool ByteCheck(this byte[] value, byte[] check)
        {
            if (value == null || check == null || value.Length != check.Length)
            {
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != check[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
