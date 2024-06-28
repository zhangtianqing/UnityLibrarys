using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Framework.Communication.TCP.TCP_CS.CoreScripts
{
    //事件
    public enum NetEvent
    {
        ConnectSucc = 1,
        BindIdentity = 2,
        ConnectFail = 3,
        Close = 4,
    }
}
