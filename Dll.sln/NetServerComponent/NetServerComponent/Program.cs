using NetServerComponent.Base;
using NetServerComponent.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServerComponent
{
    public class Program
    {
        static NetInterface netInterface;
        static void Main(string[] args)
        {
            netInterface = new TcpServer();
            netInterface.SetOnMsg(OnMsg);
            netInterface.StartService();
            Console.ReadKey();
        }

        private static void OnMsg(MsgData data)
        {
            Console.Write(data.data);
        }
    }
    
    
    
   

}
