using Newtonsoft.Json;
using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO;
using UnityEngine;

namespace Assets.Scripts.Framework.Communication.ActionCodeHandler
{
    /// <summary>
    /// Demo 处理器复写
    /// </summary>
    public class HandlerImpl_SyncClient : BaseCommuHandler
    {
        public float GameTime = 25;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log(KeyCode.L);
                BaseCommuDisptch.Instance.SendMsg(id, ActionCode.Lock, "");
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log(KeyCode.U);
                BaseCommuDisptch.Instance.SendMsg(id, ActionCode.Unlock, JsonConvert.SerializeObject(new RecvMsgObject.Recv_Stra() { msg = GameTime + "" }));
            }
        }
    }
}
