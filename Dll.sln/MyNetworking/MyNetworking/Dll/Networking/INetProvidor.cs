using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetworking.Dll.Networking
{
    public interface INetProvidor
    {
        void StartServer();
        void SetConfig(ConfigBase configBase);
        void StopServer();
        void Update();
    }
}
