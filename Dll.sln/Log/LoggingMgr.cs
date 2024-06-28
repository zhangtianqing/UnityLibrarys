using BaseUnityDll.Enum;
using Log.StorageImpl;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Log
{

    public class LoggingMgr : MonoSingleton<LoggingMgr>
    {
        List<ILoggingStorage> loggingHandlers = new List<ILoggingStorage>();
        public LogModel logModel1 = LogModel.Local;
        public UnityPositionEnum defaultLogPos = UnityPositionEnum.StreamingAssetsPath;
        public bool Log_NeedStackTrace = false;
        public virtual void Start()
        {
            if (logModel1 != LogModel.Local)
            {
                UnityEngine.Debug.LogError("错误选择了日志管理器，如果需要使用其他的方法，需要复写本方法自行实现");
                return;
            }
            AddLogModel(new LoggingStorage_Local(defaultLogPos));
            UnityEngine.Debug.Log("默认的日志管理器启动成功");
        }
        //void OnEnable()
        //{
        //    Application.logMessageReceivedThreaded += HandleLog;
        //}
        //void OnDisable()
        //{
        //    Application.logMessageReceivedThreaded -= HandleLog;
        //}
        public void AddLogModel(ILoggingStorage logModel)
        {
            loggingHandlers.Add(logModel);
        }
        public static void WriteLog(string v)
        {
            Debug.Log(v);
            Instance.Write(v);
        }
        private readonly string[] logFormats = new string[] {
        "{0}: {1} : {2}"+Environment.NewLine+" {3}" +Environment.NewLine,
        "{0}: {1} : {2}"+Environment.NewLine};
        //private void HandleLog(string condition, string stackTrace, LogType type)
        //{
        //    if (Log_NeedStackTrace)
        //    {
        //        Write(string.Format(logFormats[0], DateTime.Now.ToString(), type.ToString(), condition, stackTrace));
        //    }
        //    else
        //    {
        //        Write(string.Format(logFormats[1], DateTime.Now.ToString(), type.ToString(), condition));
        //    }

        //}
        protected void Write(string v)
        {
            foreach (ILoggingStorage logHandler in loggingHandlers)
            {
                try
                {
                    logHandler.WriteLog(v);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("日志储存失败:" + e);
                }
            }
        }
        void OnDestory()
        {
            foreach (ILoggingStorage logHandler in loggingHandlers)
            {
                logHandler.Dispose();
            }
        }
    }
}
