using System;

namespace BaseUnityDll.Class
{
    [Serializable]
    public class TObject<T>
    {
        public T v1;
    }
    [Serializable]
    public class TObject<T, T1>
    {
        public T v1;
        public T1 v2;
    }
    [Serializable]
    public class TObject<T, T1, T2>
    {
        public T v1;
        public T1 v2;
        public T2 v3;
    }
    [Serializable]
    public class TObject<T, T1, T2,T3>
    {
        public T v1;
        public T1 v2;
        public T2 v3;
        public T3 v4;
    }
}
