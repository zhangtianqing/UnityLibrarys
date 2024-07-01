using System.Collections.Generic;
using System.Text;

namespace Dll.UnityUtils
{
    public static class UniversalExtend
    {
        public static string ToStringU(this object o, bool getProperties = false)
        {
            Dictionary<string, object> k = ReflectUtil.GetFieldInfoValue(o, true, getProperties);
            StringBuilder sv = new StringBuilder();
            sv.Append($"{o.GetType().Name}(");
            foreach (var item in k.Keys)
            {
                sv.Append($"{item}:{k[item]},");
            }
            return sv.ToString() + ")";
        }

    }
}