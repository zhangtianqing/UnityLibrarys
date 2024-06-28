using Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Framework.Communication.TCP.TCP_CS
{
    public interface ITcpServerMethod
    {
        void RecvMsg(MsgMiddleware msgMiddleware);
        void RemoveClient(NetClient netClient);
        void BindClientForClient(string id, NetClient netClient);
    }
}
