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

    #region ����״̬���� &&  ������Ϣ
    protected void SendMsg(ActionCode actionCode, string msg)
    {
        BaseCommuDisptch.Instance.SendMsg(id, actionCode, msg);
    }

    [SerializeField]
    [Header("����״̬")]
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
                //Debug.Log($"��ǰ����{transform.name}:����");
                break;
            case NetEvent.BindIdentity:
                online = true;
                //Debug.Log($"��ǰ����{transform.name}:����");
                break;
            default:
                break;
        }
    }
    // Start is called before the first frame update
    protected virtual void OnEnable()
    {
        BaseCommuDisptch.Instance.AddHandle(id, this);
        //Debug.Log($"ע����Ϣ������ID:{id}");
    }
    protected virtual void OnDisable()
    {
        BaseCommuDisptch.Instance.RemoveHandler(id);
    }
    #endregion
}
