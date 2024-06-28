using Assets.Scripts.Framework.Communication;
using Assets.Scripts.Framework.Communication.TCP;
using Dll.Framework.Config.ConfigClass.EnumConfig;
using System;
using UnityEngine;

[RequireComponent(typeof(BaseCommuDisptch))]
public class CommuCompment : MonoSingleton<CommuCompment>
{
    BaseCommuCompoment commuCompoment;
    private void OnEnable()
    {
        //根据服务类型启动不同的服务
        switch (Configer.Instance.gameConfig.netSetting.universalNetConfig.commuType)
        {
            case CommuType.TCP_C:
                break;
            case CommuType.UDP_S:
                break;
            case CommuType.Serial:
                break;
            case CommuType.WS:
                break;
            case CommuType.TCP_S:
                commuCompoment = new TcpServerCompoment();
                break;
            case CommuType.UDP_C:
                break;
            default:
                break;
        }

        commuCompoment?.StartService();
    }
    private void Update()
    {
        commuCompoment?.Update();
    }
    private void OnDisable()
    {

        commuCompoment?.StopService();
    }


    public void SendMsg(string id, ActionCode ac, string msg)
    {
        commuCompoment?.SendMsg(id, ac, msg);
    }
}