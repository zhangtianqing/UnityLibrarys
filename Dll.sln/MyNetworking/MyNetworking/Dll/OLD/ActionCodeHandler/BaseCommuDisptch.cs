
using Assets.Scripts.Framework.Communication;
using Assets.Scripts.Framework.Communication.TCP.TCP_CS;
using Assets.Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCommuDisptch : MonoSingleton<BaseCommuDisptch>
{
    Dictionary<string, BaseCommuHandler> keys = new Dictionary<string, BaseCommuHandler>();
    CommuCompment baseCommuCompoment;
    private void Start()
    {
        baseCommuCompoment = FindObjectOfType<CommuCompment>();
    }
    public void AddHandle(string id, BaseCommuHandler baseActionCodeClass)
    {
        if (!keys.ContainsKey(id))
        {
            keys.Add(id, baseActionCodeClass);
        }
    }
    public void RemoveHandler(string id)
    {
        if (keys.ContainsKey(id))
        {
            keys.Remove(id);
        }
    }
    public BaseCommuHandler GetHandle(string id)
    {
        if (keys.ContainsKey(id))
        {
            return keys[id];
        }
        return null;
    }
    public void SendMsg(string id, ActionCode actionCode, string msg)
    {
        baseCommuCompoment.SendMsg(id, actionCode, msg);
    }

}
