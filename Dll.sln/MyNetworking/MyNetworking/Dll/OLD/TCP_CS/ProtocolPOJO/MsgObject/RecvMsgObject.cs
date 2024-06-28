

using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO.MsgObject;

namespace Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO
{
    public class RecvMsgObject
    {
        public class Recv_Stra : CommutBase
        {
            public string msg { get; set; }

            public override string ToString()
            {
                return $"{{{nameof(msg)}={msg}}}";
            }
        }

    }
}
