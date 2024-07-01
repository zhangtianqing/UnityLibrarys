using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUnityDll.Class
{

    public class BaseParmAlll
    {
        /// <summary>
        /// 约定 至少传入两个参数，先一个为入参数，后一个为出参数
        /// </summary>
        public class BaseParm
        {


        }
        public class BaseParm<T> : BaseParm
        {
            public T v1;
            public BaseParm(T t) : base()
            {
                this.v1 = t;
            }
        }

        public class BaseParm<T, T1> : BaseParm
        {
            public T v1;
            public T1 v2;
            public BaseParm(T t, T1 t1) : base()
            {
                this.v1 = t;
                this.v2 = t1;
            }
        }

        public class BaseParm<T, T1, T2> : BaseParm
        {
            public T v1;
            public T1 v2;
            public T2 v3;
            public BaseParm(T t, T1 t1, T2 t2) : base()
            {
                this.v1 = t;
                this.v2 = t1;
                this.v3 = t2;
            }
        }
        public class BaseParm<T, T1, T2, T3> : BaseParm
        {
            public T v1;
            public T1 v2;
            public T2 v3;
            public T3 v4;
            public BaseParm(T t, T1 t1, T2 t2, T3 t3) : base()
            {
                this.v1 = t;
                this.v2 = t1;
                this.v3 = t2;
                this.v4 = t3;
            }
        }
        public class BaseParm<T, T1, T2, T3, T4> : BaseParm
        {
            public T v1;
            public T1 v2;
            public T2 v3;
            public T3 v4;
            public T4 v5;
            public BaseParm(T t, T1 t1, T2 t2, T3 t3, T4 t4) : base()
            {
                this.v1 = t;
                this.v2 = t1;
                this.v3 = t2;
                this.v4 = t3;
                this.v5 = t4;
            }
        }
    }
}
