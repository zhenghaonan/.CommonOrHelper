using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace XmlHelper
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class Xml
    {
        /// <summary>
        /// 初始化xml文档
        /// </summary>
        /// <param name="filePath"></param>
        public static void InintXmlData(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                DataSet ds = new DataSet();
                // StringReader read = new StringReader(xmlDoc.SelectSingleNode("root").SelectSingleNode("Db").OuterXml);
                XmlNode xmlNode = xmlDoc.SelectSingleNode("root");
                if (xmlNode == null) throw new Exception("请配置根节点root");
                XmlNodeList nodes = xmlNode.SelectNodes("group");
                if (nodes.Count > 0)
                {
                    string groupKey = string.Empty;
                    foreach (var node in nodes)
                    {
                        XmlElement element = (XmlElement)node;
                        XmlAttributeCollection collection = element.Attributes;
                        if (collection.Count > 0)
                        {
                            for (int i = 0; i < collection.Count; i++)
                            {

                                if (collection[i].Name == "id")
                                {
                                    groupKey = collection[i].Value;
                                    int count = element.ChildNodes.Count;
                                    List<XmlData> xmls = new List<XmlData>();
                                    if (count > 0)
                                    {
                                     
                                        for (int j = 0; j < count; j++)
                                        {
                                            XmlData xml = new XmlData();
                                            xml.Text = element.ChildNodes[j].InnerText;
                                            XmlAttributeCollection childrenatttr = element.ChildNodes[j].Attributes;
                                            if (childrenatttr.Count > 0)
                                            {
                                                
                                                for (int k = 0; k < childrenatttr.Count; k++)
                                                {
                                                    if (childrenatttr[k].Name == "key")
                                                        xml.Key = childrenatttr[k].Value;
                                                    if (childrenatttr[k].Name == "value")
                                                        xml.Value = childrenatttr[k].Value;
                                                    if (childrenatttr[k].Name == "help")
                                                        xml.Help = childrenatttr[k].Value;
                                                    if (childrenatttr[k].Name == "description")
                                                        xml.Description = childrenatttr[k].Value;
                                                }
                                                xmls.Add(xml);
                                            }

                                        }
                                    }
                                    if (XmlData.Data.ContainsKey(groupKey))
                                        XmlData.Data[groupKey].AddRange(xmls);
                                    else
                                        XmlData.Data.Add(groupKey, xmls);
                                }
                            }
                        }


                    }

                }              
            }
            else throw new Exception($"{filePath}不存在");
        }
    }
}
