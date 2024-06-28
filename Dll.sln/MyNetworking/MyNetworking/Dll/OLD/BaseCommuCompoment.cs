
using Assets.Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Framework.Communication
{
    public abstract class BaseCommuCompoment
    {
        protected Action<string, ActionCode, string> msg;
        public abstract void StartService();
        public abstract void StopService();
        public abstract void Update();
        public virtual bool GetState() { return false; }


        public virtual void SendMsg(string id, ActionCode ac, string msg)
        {

        }

        protected void FireEvent(string id, NetEvent netEvent, String err)
        {
            Debug.Log($"FireEvent:{id}:{netEvent}:{err}");
            BaseCommuHandler baseCommuHandler = BaseCommuDisptch.Instance.GetHandle(id);
            if (null != baseCommuHandler)
            {
                baseCommuHandler.EventHandler(netEvent, err);
            }
        }

    }
}
