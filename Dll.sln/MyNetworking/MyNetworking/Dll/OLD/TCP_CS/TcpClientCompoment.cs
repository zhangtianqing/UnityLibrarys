using Assets;
using Assets.Scripts.Framework.Communication;
using Assets.Scripts.Framework.Communication.TCP.TCP_CS;
using Assets.Scripts.Framework.Communication.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpClientCompoment : BaseCommuCompoment
{
    TcpClientDriver netClient;
    public override void StartService()
    {
        netClient = new TcpClientDriver(Configer.Instance.gameConfig.netSetting.universalNetConfig.targetAddress, Configer.Instance.gameConfig.netSetting.universalNetConfig.targetPort, Configer.Instance.gameConfig.netSetting.universalNetConfig.id, Configer.Instance.gameConfig.netSetting.universalNetConfig.PPTime, Configer.Instance.gameConfig.netSetting.universalNetConfig.keepConnect);
        netClient.onBind += OnBind;
        netClient.onMessage += OnMessage;
        netClient.Connect();

    }
    public override bool GetState()
    {
        if (netClient != null)
        {
            try
            {
                return netClient.Online();
            }
            catch (Exception)
            {
            }
        }
        return false;
    }

    void OnMessage(MsgMiddleware msgMiddleware)
    {
        //Debug.Log("QueueMsg:" + msgMiddleware.ToStringU()); ;
        lock (lockobj)
        {
            msgQ.Enqueue(msgMiddleware);
        }
    }

    void OnBind()
    {
        Debug.Log($"设定ID成功");
    }

    public override void StopService()
    {
        netClient?.Close();
    }

    public override void Update()
    {
        for (int i = 0; i < maxHandlerMsg; i++)
        {
            lock (lockobj)
            {
                if (msgQ.Count > 0)
                {
                    msgMiddleware1 = msgQ.Dequeue();

                    BaseCommuHandler baseCommuHandler = BaseCommuDisptch.Instance.GetHandle(msgMiddleware1.id);
                    if (baseCommuHandler != null)
                    {
                        baseCommuHandler.MsgHandler(msgMiddleware1.actionCode, msgMiddleware1.commutBase);
                    }
                }
                else
                {
                    break;
                }
            }

        }
    }
    public override void SendMsg(string id, ActionCode ac, string msg)
    {
        netClient.SendMsg(ac, msg);
    }
    MsgMiddleware msgMiddleware1 = null;
    int maxHandlerMsg = 1000;
    object lockobj = new object();
    Queue<MsgMiddleware> msgQ = new Queue<MsgMiddleware>();
}
