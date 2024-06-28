
using Assets.Scripts.Framework.Communication.TCP.TCP_CS;
using Scripts.Framework.Communication.TCP.TCP_CS.ProtocolPOJO.MsgObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts
{
    /// <summary>
    /// 为了队列处理
    /// </summary>
    public class MsgBase
    {
        public ActionCode actionCode { get; set; }
        public string commutBase { get; set; }
    }
    /// <summary>
    /// 为了队列处理
    /// </summary>
    public class MsgMiddleware
    {
        public string id;
        public ActionCode actionCode { get; set; }
        public string commutBase { get; set; }
    }
}
