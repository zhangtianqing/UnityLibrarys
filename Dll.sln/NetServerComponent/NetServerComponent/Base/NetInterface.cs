using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServerComponent.Base
{
    public interface NetInterface
    {
        void SetOnMsg(Action<MsgData> action);
        void SendMsg(MsgData msg);
        void StartService();
        void StopService();
    }
}
