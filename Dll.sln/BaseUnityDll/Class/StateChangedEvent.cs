using System;
using static BaseUnityDll.Class.BaseParmAlll;

namespace BaseUnityDll.Class
{
    public class StateChangedEvent
    {
        public bool enter = false;
        public Action<BaseParm> OnStart;
        public Action OnUpdate;
        public Action<BaseParm> OnExit;
        public Action<BaseParm> OnUpdateData;
    }
}
