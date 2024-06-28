
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Dll.Framework.Config.ConfigLoader
{
    public class ConfigLoader_XML : IConfigLoader

    {
        private string fileName;
        private string filePath;
        private string dirpath;
        public ConfigLoader_XML()
        {
            fileName = GetDefaultFileName();
            dirpath = Application.streamingAssetsPath;
            filePath = Path.Combine(dirpath, fileName);
        }
        public ConfigLoader_XML(string filename, string dirPathOfStreamingAsset = "")
        {
            this.fileName = filename;
            if (dirPathOfStreamingAsset == "")
            {
                dirpath = Application.streamingAssetsPath;
            }
            else
            {
                dirpath = Path.Combine(Application.streamingAssetsPath, dirPathOfStreamingAsset);
            }
            filePath = Path.Combine(dirpath, fileName);
        }
        public override T Load<T>()
        {
            if (!File.Exists(filePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                File.Create(filePath).Close();
                Save(new T());
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(filePath));
            XmlNode rootNode = xmlDocument.SelectSingleNode($"/{typeof(T).Name}");
            T t = new T();
            SetValue(t, rootNode.ChildNodes);
            return t;
        }

        void SetValue(object t, XmlNodeList xmlL)
        {
            Dictionary<string, FieldInfo> keyValuePairs = t.GetType().GetFields().ToDictionary(item => item.Name, item => item);
            foreach (XmlNode item in xmlL)
            {
                if (keyValuePairs.ContainsKey(item.Name))
                {
                    if (keyValuePairs[item.Name].FieldType.IsGenericType)
                    {
                        var listType = (typeof(List<>)).MakeGenericType(keyValuePairs[item.Name].FieldType.GetGenericArguments()[0]);
                        dynamic list = Activator.CreateInstance(listType);

                        if (CheckType_IsObject(keyValuePairs[item.Name].FieldType.GetGenericArguments()[0]))
                        {
                            for (int i = 0; i < item.ChildNodes.Count; i++)
                            {
                                dynamic dynamic1 = Activator.CreateInstance(keyValuePairs[item.Name].FieldType.GetGenericArguments()[0]);
                                SetValue(dynamic1, item.ChildNodes[i].ChildNodes);
                                list.Add(dynamic1);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < item.ChildNodes.Count; i++)
                            {
                                dynamic tmp = Convert.ChangeType(item.ChildNodes[i], keyValuePairs[item.Name].FieldType.GetGenericArguments()[0]);
                                list.Add(tmp);
                            }
                        }
                        keyValuePairs[item.Name].SetValue(t, list);
                    }
                    else
                    if (keyValuePairs[item.Name].FieldType.IsEnum)
                    {
                        keyValuePairs[item.Name].SetValue(t, int.Parse(item.InnerText.ToLower()));
                    }
                    else
                    {
                        switch (keyValuePairs[item.Name].FieldType.Name)
                        {
                            case "String":
                                keyValuePairs[item.Name].SetValue(t, item.InnerText);
                                break;
                            case "Boolean":
                                keyValuePairs[item.Name].SetValue(t, bool.Parse(item.InnerText.ToLower()));
                                break;
                            case "Single":
                                keyValuePairs[item.Name].SetValue(t, float.Parse(item.InnerText.ToLower()));
                                break;
                            case "Int32":
                                keyValuePairs[item.Name].SetValue(t, int.Parse(item.InnerText.ToLower()));
                                break;
                            case "Double":
                                keyValuePairs[item.Name].SetValue(t, double.Parse(item.InnerText.ToLower()));
                                break;
                            default:
                                if (keyValuePairs[item.Name].FieldType.BaseType.Name == "Object")
                                {
                                    dynamic obj = keyValuePairs[item.Name].FieldType.Assembly.CreateInstance(keyValuePairs[item.Name].FieldType.FullName);
                                    SetValue(obj, item.ChildNodes);
                                    keyValuePairs[item.Name].SetValue(t, obj);
                                }
                                break;
                        }
                    }
                }
            }
        } /// <summary>
          /// 创建List
          /// </summary>
          /// <param name="type">泛型类型</param>
          /// <returns></returns>
        public static dynamic CreateList(Type type)
        {
            Type listType = typeof(List<>);
            //指定泛型的具体类型
            Type newType = listType.MakeGenericType(new Type[] { type });
            //创建一个list返回
            return Activator.CreateInstance(newType, new object[] { });
        }
        public override bool Save(object t)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                xmlDoc.AppendChild(xmlDeclaration);

                XmlElement rootE = xmlDoc.CreateElement(t.GetType().Name);
                xmlDoc.AppendChild(rootE);

                SaveValue(t, rootE, xmlDoc);

                File.WriteAllText(filePath, FormatXml(xmlDoc.InnerXml));
                Console.WriteLine("保存数据成功");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(t.GetType().FullName + ":" + e);
                return false;
            }
            return true;
        }
        void SaveValue(object obj, XmlElement rootE, XmlDocument xmlDoc)
        {
            System.Reflection.FieldInfo[] fieldinfos = obj.GetType().GetFields();//System.Reflection.BindingFlags.Instance | 
            foreach (var item in fieldinfos)
            {
                XmlNode xmlElement = xmlDoc.CreateElement(item.Name);

                if (item.FieldType.IsEnum)
                {
                    xmlElement.InnerText = $"{System.Convert.ToInt32(Enum.Parse(item.FieldType, item.GetValue(obj).ToString()) as Enum)}";
                }
                else
                if (item.FieldType.IsGenericType)
                {
                    object subObj = item.GetValue(obj);
                    if (subObj != null)
                    {
                        int count = Convert.ToInt32(subObj.GetType().GetProperty("Count").GetValue(subObj));
                        for (int i = 0; i < count; i++)
                        {
                            object subO = subObj.GetType().GetProperty("Item").GetValue(subObj, new object[] { i });
                            XmlNode xmlElementSub = xmlDoc.CreateElement(item.FieldType.GetGenericArguments()[0].Name);

                            SaveValue(subO, (XmlElement)xmlElementSub, xmlDoc);
                            xmlElement.AppendChild(xmlElementSub);
                        }
                    }
                }
                else if (CheckType_IsObject(item))
                {
                    dynamic o2;
                    if (null == item.GetValue(obj))
                    {
                        o2 = item.FieldType.Assembly.CreateInstance(item.FieldType.FullName);
                    }
                    else
                    {
                        o2 = item.GetValue(obj);
                    }
                    SaveValue(o2, (XmlElement)xmlElement, xmlDoc);
                }
                else
                {
                    xmlElement.InnerText = item.GetValue(obj) != null ? item.GetValue(obj).ToString() : "";
                }

                if (item.GetCustomAttributes(typeof(HeaderAttribute), true).Length > 0)
                {
                    XmlAttribute wktAttribute = xmlDoc.CreateAttribute(/*nameof(HeaderAttribute)*/"说明");
                    if (item.FieldType.IsEnum)
                    {
                        string[] strs = item.FieldType.GetEnumNames();
                        Array ints = item.FieldType.GetEnumValues();
                        StringBuilder sv = new StringBuilder();
                        sv.Append(((HeaderAttribute)item.GetCustomAttributes(typeof(HeaderAttribute), true)[0]).header);
                        sv.Append("|填入数字: ");
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (i == 0)
                            {
                                sv.Append($"{strs[i]}({(int)Enum.Parse(item.FieldType, strs[i])}) ");
                            }
                            else
                            {
                                sv.Append($",{strs[i]}({(int)Enum.Parse(item.FieldType, strs[i])}) ");
                            }
                        }
                        wktAttribute.Value = sv.ToString(); ;
                    }
                    else
                    {
                        wktAttribute.Value = ((HeaderAttribute)item.GetCustomAttributes(typeof(HeaderAttribute), true)[0]).header;
                    }
                    xmlElement.Attributes.Append(wktAttribute);
                }
                rootE.AppendChild(xmlElement);
            }
        }
        bool CheckType_IsObject(FieldInfo f)
        {
            return CheckType_IsObject(f.FieldType);
        }
        bool CheckType_IsObject(Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return false;
                case "Boolean":
                    return false;
                case "Single":
                    return false;
                case "Int32":
                    return false;
                case "Double":
                    return false;
                default:
                    return true;
            }
        }
        private static string FormatXml(object xml)
        {
            XmlDocument xd;
            if (xml is XmlDocument)
            {
                xd = xml as XmlDocument;
            }
            else
            {
                xd = new XmlDocument();
                xd.LoadXml(xml as string);
            }
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '	';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

    }
}