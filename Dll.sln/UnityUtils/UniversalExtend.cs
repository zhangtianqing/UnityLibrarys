using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
        //public static List<T> RandomSort<T>(this List<T> list)
        //{
        //    var random = new System.Random();
        //    var newList = new List<T>();
        //    foreach (var item in list)
        //    {
        //        newList.Insert(random.Next(newList.Count), item);
        //    }
        //    return newList;
        //}

        public static IEnumerator DelayToDo(float
            time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}