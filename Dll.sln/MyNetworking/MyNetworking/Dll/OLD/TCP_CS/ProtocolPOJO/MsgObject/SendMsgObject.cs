
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO.MsgObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO
{

    public class SendMsgObject
    {

        public class Send_None
        {

            public class Data : CommutBase
            {
                public int code { get; set; }

            }
        }
        public class Send_Ping
        {
            public class Data : CommutBase
            {
                public string ping { get; set; }
            }
        }
    }
}
