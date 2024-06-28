using BaseUnityDll.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Log.StorageImpl
{

    public class LoggingStorage_Local : ILoggingStorage
    {
        Thread logThread;
        /// <summary>
        /// 使用Unity的目录
        /// </summary>
        /// <param name="unityPositionEnum"></param>
        public LoggingStorage_Local(UnityPositionEnum unityPositionEnum, string prefix = "Log", string fileName = null)
        {
            string path = "";
            switch (unityPositionEnum)
            {
                case UnityPositionEnum.StreamingAssetsPath:
                    path = Application.streamingAssetsPath;
                    break;
                case UnityPositionEnum.PersistentDataPath:
                    path = Application.persistentDataPath;
                    break;
                case UnityPositionEnum.DataPath:
                    path = Application.dataPath;
                    break;
                default:
                    path = Application.streamingAssetsPath;
                    break;
            }
            saveFilePath = Path.Combine(path, prefix);
            if (string.IsNullOrEmpty(fileName))
            {
                saveFilePath = Path.Combine(saveFilePath, $"{GetDataFormatStr()}.log");
            }
            else
            {
                saveFilePath = Path.Combine(saveFilePath, $"{fileName}.log");
            }
            logThread = new Thread(LogThreading);
            logThread.IsBackground = true;
            logThread.Start();
        }
        Queue<string> strings = new Queue<string>();
        private void LogThreading()
        {
            if (streamWriter == null)
            {
                streamWriter = File.AppendText(saveFilePath);
            }
            string temp = null;
            int handMaxEveryDefault = 10000;
            int handMaxEvery = handMaxEveryDefault;
            int handTimeMs = 100;

            StringBuilder stringBuilder = new StringBuilder();
            while (true)
            {
                if (strings.Count>0)
                {
                    lock (strings)
                    {
                        while (strings.Count > 0 && handMaxEveryDefault-- > 0)
                        {
                            temp = strings.Dequeue();
                            stringBuilder.AppendLine(temp);
                        }
                    }
                    
                    if (stringBuilder.Length>0)
                    {
                        streamWriter.WriteLine(stringBuilder.ToString());
                    }

                    stringBuilder.Clear();
                    handMaxEvery = handMaxEveryDefault;
                }
                Thread.Sleep(handTimeMs);
            }
        }

        string saveFilePath = "";
        StreamWriter streamWriter;
        public void WriteLog(string log)
        {
            strings.Enqueue(log);
        }

        public void Dispose()
        {
            logThread.Join();

            streamWriter?.Dispose();
        }
        /// <summary>
        /// 如果现在是 2024 年 6 月 19 日 15 时 30 分 45 秒，那么输出的字符串将会是 "20240619-153045"
        /// </summary>
        /// <returns></returns>
        string GetDataFormatStr()
        {
            // 获取当前时间
            DateTime now = DateTime.Now;

            // 获取年份
            string year = now.Year.ToString();

            // 获取月份
            string month = now.Month.ToString("d2"); // 使用 "d2" 格式化字符串，确保月份始终为两位数

            // 获取日期
            string day = now.Day.ToString("d2"); // 使用 "d2" 格式化字符串，确保日期始终为两位数

            // 获取小时
            string hour = now.Hour.ToString("d2"); // 使用 "d2" 格式化字符串，确保小时始终为两位数

            // 获取分钟
            string minute = now.Minute.ToString("d2"); // 使用 "d2" 格式化字符串，确保分钟始终为两位数

            // 获取秒钟
            string second = now.Second.ToString("d2"); // 使用 "d2" 格式化字符串，确保秒钟始终为两位数

            // 将年月日时分秒拼接为一个字符串
            string dateTimeString = $"{year}{month}{day}-{hour}{minute}{second}";
            return dateTimeString;
        }
    }
}
