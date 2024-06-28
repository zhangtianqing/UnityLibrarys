using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dll.UnityUtils
{
    public class ReflectUtil
    {
        // Update is called once per frame
        /// <summary>
        /// C#获取一个接口在所在的程序集中的所有子类类型的名称
        /// </summary>
        /// <param name="interfaceMethod">给定的类型</param>
        /// <returns>所有子类类型的名称</returns>
        public static List<string> GetSubClassNames(Type interfaceMethod)
        {
            return GetSubClassTypesByInterface(interfaceMethod).Select(i => i.Name).ToList();
        }

        /// <summary>
        /// 获取对象字段和属性，默认不获取属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="getField">是否获取字段 默认获取</param>
        /// <param name="getProperties">是否获取属性 默认不获取</param>
        /// <param name="bindingFlags">通用匹配类型 https://blog.csdn.net/weixin_56814032/article/details/127134310 </param>
        /// <returns>字段名称与对应的值</returns>
        public static Dictionary<string, object> GetFieldInfoValue(object obj, bool getField = true, bool getProperties = false, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            if (getField)
            {
                FieldInfo[] fieldInfos = obj.GetType().GetFields(bindingFlags);
                foreach (var item in fieldInfos)
                {
                    keyValuePairs.Add(item.Name, item.GetValue(obj));
                }
            }
            if (getProperties)
            {
                PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bindingFlags);
                foreach (var item in propertyInfos)
                {
                    keyValuePairs.Add(item.Name, item.GetValue(obj));
                }
            }
            return keyValuePairs;
        }

        public static void Demo()
        {
            List<Type> types = GetSubClassTypesByInterface(typeof(IAutoMap));
            foreach (var item in types)
            {
                Debug.Log(item.FullName);

            }
        }
        public interface IAutoMap
        {
        }
        /// <summary>
        /// C#获取一个接口在所在的程序集中的所有子类
        /// </summary>
        /// <param name="parentType">接口</param>
        /// <returns>所有子类的名称</returns>
        public static List<Type> GetSubClassTypesByInterface(Type parentType)
        {
            List<Type> subTypeList = new List<Type>();
            var assembly = parentType.Assembly;//获取当前接口所在的程序集``
            var assemblyAllTypes = assembly.GetTypes();//获取该程序集中的所有类型
            foreach (var itemType in assemblyAllTypes)//遍历所有类型进行查找
            {
                var baseInteface = itemType.GetInterfaces();
                if (baseInteface != null && baseInteface.Length > 0)
                {
                    foreach (var itemInterface in baseInteface)
                    {
                        if (itemInterface.Name == parentType.Name)
                        {
                            subTypeList.Add(itemType);//加入子类集合中
                            break;
                        }
                    }
                }
            }
            return subTypeList;//获取所有子类类型的名称
        }
        /// <summary>
        /// 获取对应注解所在的程序集中的所有 注解了对应类的类型
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        public static List<Type> GetSubClassTypeByAttribute(Type Attribute)
        {
            List<Type> subTypeList = new List<Type>();
            var assembly = Attribute.Assembly;//获取当前接口所在的程序集``
            var assemblyAllTypes = assembly.GetTypes();//获取该程序集中的所有类型
            foreach (var itemType in assemblyAllTypes)//遍历所有类型进行查找
            {
                if (null != itemType.GetCustomAttribute(Attribute))
                {
                    subTypeList.Add(itemType);//加入子类集合中
                }
            }
            return subTypeList;//获取所有子类类型的名称
        }
    }
}