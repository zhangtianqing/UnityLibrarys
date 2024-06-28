using Assets.Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System;
using UnityEngine;

public abstract class BaseCommuHandler : MonoBehaviour
{
    public string id;
    public virtual void MsgHandler(ActionCode ac, string msgMiddleware)
    {
        Debug.Log($" {nameof(BaseCommuHandler)}:Recvmsg:{msgMiddleware}");
        SendMsg(ac, "i am recv!");
    }

    #region 基础状态控制 &&  发送消息
    protected void SendMsg(ActionCode actionCode, string msg)
    {
        BaseCommuDisptch.Instance.SendMsg(id, actionCode, msg);
    }

    [SerializeField]
    [Header("在线状态")]
    protected bool online = false;
    public void EventHandler(NetEvent netEvent, string msg)
    {
        switch (netEvent)
        {
            case NetEvent.ConnectSucc:
                break;
            case NetEvent.ConnectFail:
                break;
            case NetEvent.Close:
                online = false;
                //Debug.Log($"当前对象{transform.name}:下线");
                break;
            case NetEvent.BindIdentity:
                online = true;
                //Debug.Log($"当前对象{transform.name}:上线");
                break;
            default:
                break;
        }
    }
    // Start is called before the first frame update
    protected virtual void OnEnable()
    {
        BaseCommuDisptch.Instance.AddHandle(id, this);
        //Debug.Log($"注册消息监听，ID:{id}");
    }
    protected virtual void OnDisable()
    {
        BaseCommuDisptch.Instance.RemoveHandler(id);
    }
    #endregion
}
