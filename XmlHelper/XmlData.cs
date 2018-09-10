using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace XmlHelper
{
    /// <summary>
    /// note：1、根节点必须是root，属性可以多 但是不能少 
    ///       2、必须有group标签分组， group 组必须要有id属性 如：<group id = "GroupTwo" ></group>
    ///       3、group 下面的标签必须有  id key  help description 属性 如:<test  id="testOne" key="conStr"  value ="local"  help="数据库连接字符串" description="数据库连接字符串">dasdadad</test>
    /// </summary>
    public class XmlData
    {
        internal static Dictionary<string, List<XmlData>> Data = new Dictionary<string, List<XmlData>>();
        /// <summary>
        /// key的值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// id的值
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// value 的值
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// help 的值
        /// </summary>
        public String Help { get; set; }
        /// <summary>
        /// description的值
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        ///根据组名称获取组中的内容
        /// </summary>
        /// <param name="GroupKey">组的id</param>
        /// <returns>该组所有节点的值</returns>
        public IList<XmlData> this[String GroupKey]
        {
            get
            {
                if (Data.ContainsKey(GroupKey))
                    return Data[GroupKey];
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupKey">组的id</param>
        /// <param name="key">标签的key值</param>
        /// <returns>当前标签的数据</returns>
        public XmlData this[String GroupKey, String key]
        {
            get
            {
                if (Data.ContainsKey(GroupKey) && Data[GroupKey].Any())
                {
                    var nodeData = Data[GroupKey].Where(x => x.Key == key).FirstOrDefault();
                    if (nodeData != null)
                        return nodeData;
                    else
                        return null;
                }
                return null;
            }
        }
    }
}
