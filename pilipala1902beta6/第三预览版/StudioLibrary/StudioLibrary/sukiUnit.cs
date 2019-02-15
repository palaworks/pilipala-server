using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;/* IO */
using System.Xml;/* XML */
using MySql.Data.MySqlClient;/* MySqlDB */
using System.Data;/* ADO.NET */

using LibStructs;

namespace sukiUnit
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// 以字符串形式输出文件（重载一）（UTF8编码模式）
        /// </summary>
        /// <param name="url">文件所在的本地网络路径</param>
        /// <returns>返回字符串</returns>
        public string fileToStr(string url)
        {
            //读取url文件到文件尾，然后返回
            return LibFrame.LibInformation.getStreamReader(url).ReadToEnd();
        }

        /// <summary>
        /// 以字符串形式输出文件（重载二）
        /// </summary>
        /// <param name="url">文件所在的本地网络路径</param>
        /// /// <param name="encodingType">解析文件所用的编码模式</param>
        /// <returns>返回字符串</returns>
        public string fileToStr(string url, string encodingType)
        {
            //读取url文件到文件尾，然后返回
            return LibFrame.LibInformation.getStreamReader(url, encodingType).ReadToEnd();
        }

        /// <summary>
        /// 以字符串形式输出文件（重载三）（UTF8编码模式）
        /// </summary>
        /// <param name="filePath">文件所在的本地物理路径</param>
        /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
        /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
        /// <returns>返回字符串</returns>
        public string fileToStr(string filePath, int bufferSize, bool useAsync)
        {

            using (
                StreamReader StreamReader = new StreamReader//流读取对象
                (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write, bufferSize, useAsync)//文件流对象
                , Encoding.GetEncoding("UTF8"))//指定编码模式
                    )
            {
                //以字符串形式输出文件
                return StreamReader.ReadToEnd().ToString();
            }
        }

        /// <summary>
        /// 以字符串形式输出文件（重载四）
        /// </summary>
        /// <param name="filePath">文件所在的本地物理路径</param>
        /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
        /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
        /// <param name="encodingType">解析文件所用的编码模式</param>
        /// <returns>返回字符串</returns>
        public string fileToStr(string filePath, int bufferSize, bool useAsync, string encodingType)
        {

            using (
                StreamReader StreamReader = new StreamReader//流读取对象
                (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write, bufferSize, useAsync)//文件流对象
                , Encoding.GetEncoding(encodingType))//指定编码模式
                    )
            {
                //以字符串形式输出文件
                return StreamReader.ReadToEnd().ToString();
            }

        }
    }

    /// <summary>
    /// XML文件读写类
    /// </summary>
    public class XmlHandler
    {
        //xml文档地址，默认为运行目录的"StdLibx.xml"
        private string xpath = Directory.GetCurrentDirectory() + @"\StdLibx.xml";
        static XmlDocument xDoc = new XmlDocument();

        /// <summary>
        /// 指定流的方法
        /// </summary>
        /// <param name="xStream">文件流地址</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool reStream(string xStream)
        {
            try
            {
                xpath = xStream;
                xDoc.Load(xpath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 创建Xml文档的方法（重载一）
        /// </summary>
        /// <param name="fileName">Xml文档被创建的目录</param>
        /// <param name="xmlName">Xml文档名</param>
        /// <param name="rootName">根节点名</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool createXml(string fileName, string xmlName, string rootName)
        {
            try
            {
                XmlDocument newDoc = new XmlDocument();//doc模式读写
                XmlNode node_xml = newDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                newDoc.AppendChild(node_xml);
                XmlNode root = newDoc.CreateElement(rootName);//创建根节点
                newDoc.AppendChild(root);//添加根节点

                newDoc.Save(fileName + @"\" + xmlName + ".xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 创建Xml文档的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool createXml(XmlSign XmlSign)
        {
            try
            {
                XmlDocument newDoc = new XmlDocument();//doc模式读写
                XmlNode node_xml = newDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                newDoc.AppendChild(node_xml);
                XmlNode root = newDoc.CreateElement(XmlSign.rootName);//创建根节点
                newDoc.AppendChild(root);//添加根节点

                newDoc.Save(XmlSign.fileName + @"\" + XmlSign.xmlName + ".xml");
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 添加实节点的方法（重载一）
        /// </summary>
        /// <param name="path">被指定的父节点</param>
        /// <param name="nodeName">新建的节点名</param>
        /// <param name="attName">节点的属性</param>
        /// <param name="attValue">节点的属性值</param>
        /// <param name="innerText">节点的子文本</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool addRealNode(string path, string nodeName, string attName, string attValue, string innerText)
        {
            try
            {
                XmlNode parentNode = xDoc.SelectSingleNode(path);//父节点指定
                XmlNode newNode = xDoc.CreateElement(nodeName);//创建新的子节点
                XmlAttribute newAtt = xDoc.CreateAttribute(attName);//创建用于新的子节点的一个属性

                newAtt.Value = attValue;//属性的值指定
                newNode.Attributes.Append(newAtt);//添加属性到节点

                newNode.InnerText = innerText;

                parentNode.AppendChild(newNode);//在父节点上添加该节点
                xDoc.Save(xpath);//保存到xpath
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 添加实节点的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool addRealNode(XmlSign XmlSign)
        {
            try
            {
                XmlNode parentNode = xDoc.SelectSingleNode(XmlSign.path);//父节点指定
                XmlNode newNode = xDoc.CreateElement(XmlSign.nodeName);//创建新的子节点
                XmlAttribute newAtt = xDoc.CreateAttribute(XmlSign.attName);//创建用于新的子节点的一个属性

                newAtt.Value = XmlSign.attValue;//属性的值指定
                newNode.Attributes.Append(newAtt);//添加属性到节点

                newNode.InnerText = XmlSign.innerText;

                parentNode.AppendChild(newNode);//在父节点上添加该节点
                xDoc.Save(xpath);//保存到xpath
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 添加空节点的方法（重载一）
        /// </summary>
        /// <param name="path">被指定的父节点</param>
        /// <param name="nodeName">新建的空节点名</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool addEmptyNode(string path, string nodeName)
        {
            try
            {
                XmlNode pxn = xDoc.SelectSingleNode(path);
                XmlNode nxn = xDoc.CreateElement(nodeName);
                pxn.AppendChild(nxn);
                xDoc.Save(xpath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 添加空节点的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool addEmptyNode(XmlSign XmlSign)
        {
            try
            {
                XmlNode Pxn = xDoc.SelectSingleNode(XmlSign.path);
                XmlNode Cxn = xDoc.CreateElement(XmlSign.nodeName);
                Pxn.AppendChild(Cxn);
                xDoc.Save(xpath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 删除被指定的父节点下子节点的方法（重载一）
        /// </summary>
        /// <param name="path">被指定的父节点</param>
        /// <param name="nodeName">被删的子节点名</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool removeNode(string path, string nodeName)
        {
            try
            {
                XmlNode baseNode = xDoc.SelectSingleNode(path);//指定父节点
                XmlNodeList xnList = baseNode.ChildNodes;//初始化父节点的子节点列
                foreach (XmlNode n in xnList)//遍历每一个节点
                {
                    if (n.Name == nodeName)//判断节点名
                    {
                        baseNode.RemoveChild(n);

                        xDoc.Save(xpath);
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除被指定的父节点下子节点的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回true，报错则返回false</returns>
        public bool removeNode(XmlSign XmlSign)
        {
            try
            {
                XmlNode baseNode = xDoc.SelectSingleNode(XmlSign.path);//指定父节点
                XmlNodeList xnList = baseNode.ChildNodes;//初始化父节点的子节点列
                foreach (XmlNode n in xnList)//遍历每一个节点
                {
                    if (n.Name == XmlSign.nodeName)//判断节点名
                    {
                        baseNode.RemoveChild(n);

                        xDoc.Save(xpath);
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 读取被指定的实节点的信息的方法（重载一）
        /// </summary>
        /// <param name="path">被指定的实节点</param>
        /// <param name="type">被读取的信息类型</param>
        /// <returns>通常返回被读取的信息，传递未知的type返回"UnknownReadingType"，报错则返回null</returns>
        public string readInformation(string path, string type)
        {
            try
            {
                XmlNode xn = xDoc.SelectSingleNode(path);
                switch (type)
                {
                    case "_name":
                        return xn.Name;
                    case "_value":
                        return xn.InnerText;
                    default:
                        return "UnknownReadingType";
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 读取被指定的实节点的信息的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回被读取的信息，传递未知的type返回"UnknownReadingType"，报错则返回null</returns>
        public string readInformation(XmlSign XmlSign)
        {
            try
            {
                XmlNode xn = xDoc.SelectSingleNode(XmlSign.path);
                switch (XmlSign.type)
                {
                    case "_name":
                        return xn.Name;
                    case "_value":
                        return xn.InnerText;
                    default:
                        return "UnknownReadingType";
                }
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 读取被指定的实节点的属性值的方法（重载一）
        /// </summary>
        /// <param name="path">被指定的实节点</param>
        /// <param name="attName">被读值的属性名</param>
        /// <returns>通常返回被读取属性的值，报错则返回null</returns>
        public string readAttribute(string path, string attName)
        {
            try
            {
                XmlNode xn = xDoc.SelectSingleNode(path);
                return xn.Attributes[attName].Value;
            }
            catch
            {
                return null;//方法中发生致命性错误，可能是由无法查找到节点属性导致
            }
        }
        /// <summary>
        /// 读取被指定的实节点的属性值的方法（重载二）
        /// </summary>
        /// <param name="XmlSign">Xml文档信息通用结构体</param>
        /// <returns>通常返回被读取属性的值，报错则返回null</returns>
        public string readAttribute(XmlSign XmlSign)
        {
            try
            {
                XmlNode xn = xDoc.SelectSingleNode(XmlSign.path);
                return xn.Attributes[XmlSign.attName].Value;
            }
            catch
            {
                return null;//方法中发生致命性错误，可能是由无法查找到节点属性导致
            }
        }

    }

    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class MySqlConnH
    {
        /* 
         * 内置查询方法不会关闭(销毁)任何一个数据库连接
         * 若存在安全性需要，HCommand会被设置为null，但不会关闭(销毁)
         * 只有调用相关方法时才会执行该方面的操作
         */

        private MySqlConnection HConnection;
        private MySqlCommand HCommand;

        /// <summary>
        /// close主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool closeHConnection()
        {
            try
            {
                switch (HConnection.State)
                {
                    case ConnectionState.Open:
                        HConnection.Close();
                        if (HConnection.State == ConnectionState.Closed)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case ConnectionState.Closed:
                        return true;

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// dispose主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool disposeHConnection()
        {
            try
            {
                switch (HConnection.State)
                {
                    case ConnectionState.Open:
                        HConnection.Dispose();
                        if (HConnection.State == ConnectionState.Closed)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case ConnectionState.Closed:
                        return true;

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 将主连接设置为null值
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool nullHConnection()
        {
            try
            {
                HConnection = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 重启主连接（须以主连接定义完成为前提）
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool restartHConnection()
        {
            try
            {
                switch (HConnection.State)
                {
                    case ConnectionState.Closed:
                        HConnection.Open();
                        if (HConnection.State == ConnectionState.Open)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case ConnectionState.Open:
                        return true;//注意，在连接打开时调用该方法再次打开也会返回true，这可能会带来安全性问题

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// dispose主命令行
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool disposeHCommand()
        {
            try
            {
                HCommand.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 将主命令行设置为null值
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool nullHCommand()
        {
            try
            {
                HCommand = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设置主命令行的sql语句（重载一）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
        /// </summary>
        /// <param name="MySqlConnection">要求主命令行执行的MySqlConnection连接实例</param>
        /// <param name="sql">被设置的sql语句</param>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool setHCommand(MySqlConnection MySqlConnection, string sql)
        {
            try
            {
                HCommand = new MySqlCommand(sql, MySqlConnection);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设置主命令行的sql语句（重载二：HConnection介入）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
        /// </summary>
        /// <param name="sql">被设置的sql语句</param>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool setHCommand(string sql)
        {
            try
            {
                HCommand = new MySqlCommand(sql, HConnection);
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 启动连接（重载一：HConnection介入）
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="port">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回true，错误则返回null</returns>
        public bool start(string dataSource, string port, string userName, string password)
        {
            //组建连接信息并创建连接
            HConnection = new MySqlConnection
                (
                "Data source=" + dataSource + ";port="
                + port + ";User Id=" + userName + ";password=" + password + ";"
                );
            return start(HConnection);
        }
        /// <summary>
        /// 启动连接（重载二：HConnection介入）
        /// </summary>
        /// <param name="ConnSign">连接签名</param>
        /// <returns>返回true，错误则返回false</returns>
        public bool start(ConnSign ConnSign)
        {
            //组建连接信息并创建连接
            HConnection = new MySqlConnection
                (
                "Data source=" + ConnSign.dataSource + ";port=" +
                ConnSign.port + ";User Id=" + ConnSign.user + ";password=" + ConnSign.password + ";"
                );
            return start(HConnection);
        }
        /// <summary>
        /// 启动连接（重载三）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <returns>返回true，错误则返回false</returns>
        public bool start(MySqlConnection MySqlConnection)
        {
            switch (MySqlConnection.State)
            {
                case ConnectionState.Closed:
                    MySqlConnection.Open();
                    if (MySqlConnection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case ConnectionState.Open:
                    return true;//注意，在连接打开时调用该方法再次打开也会返回true，这可能会带来安全性问题

                default:
                    return false;
            }
        }



        /// <summary>
        /// 获得数据行（重载一）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回查询结果</returns>
        public DataRow getRow(MySqlConnection MySqlConnection, string sql)
        {
            try
            {
                if (MySqlConnection.State == ConnectionState.Closed)//判断连接是否被关闭
                {
                    MySqlConnection.Open();//连接关闭则打开
                }
                using (MySqlDataAdapter myDA = new MySqlDataAdapter(sql, MySqlConnection))
                {
                    DataTable DataTable = new DataTable();
                    myDA.Fill(DataTable);

                    return DataTable.Rows[0];
                }
            }
            finally//释放资源
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 获得数据行（重载二）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回查询结果</returns>
        public DataRow getRow(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
        {
            try
            {
                if (MySqlConnection.State == ConnectionState.Closed)//判断连接是否被关闭
                {
                    MySqlConnection.Open();//连接关闭则打开
                }
                //将外来cmd设置为基于MySqlConnection执行
                MySqlCommand.Connection = MySqlConnection;
                using (MySqlDataAdapter myDA = new MySqlDataAdapter(MySqlCommand))
                {
                    DataTable DataTable = new DataTable();
                    myDA.Fill(DataTable);

                    return DataTable.Rows[0];
                }
            }
            finally//释放资源
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 获得数据行（重载三：HConnection介入）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回查询结果</returns>
        public DataRow getRow(string sql)
        {
            try
            {
                if (HConnection.State == ConnectionState.Closed)//判断连接是否被关闭
                {
                    HConnection.Open();//连接关闭则打开
                }
                using (MySqlDataAdapter myDA = new MySqlDataAdapter(sql, HConnection))
                {
                    DataTable DataTable = new DataTable();
                    myDA.Fill(DataTable);

                    return DataTable.Rows[0];
                }
            }
            finally//释放资源
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 获得数据行（重载四：HConnection介入）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回查询结果</returns>
        /// 
        public DataRow getRow(MySqlCommand MySqlCommand)
        {
            try
            {
                if (HConnection.State == ConnectionState.Closed)//判断连接是否被关闭
                {
                    HConnection.Open();//连接关闭则打开
                }
                //将外来cmd设置为基于HConnection执行
                MySqlCommand.Connection = HConnection;
                using (MySqlDataAdapter myDA = new MySqlDataAdapter(MySqlCommand))
                {
                    DataTable DataTable = new DataTable();
                    myDA.Fill(DataTable);

                    return DataTable.Rows[0];
                }
            }
            finally//释放资源
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 获得数据行（重载五：通过键值匹配，从数据表中获取数据行）
        /// </summary>
        /// <param name="DataTable">数据表实例</param>
        /// <param name="keyName">键名</param>
        /// <param name="keyValue">键值</param>
        /// <returns>返回获得的DataRow数据行实例，未检索到返回null</returns>
        public DataRow getRow(DataTable DataTable, string keyName, object keyValue)
        {
            foreach (DataRow DataRow in DataTable.Rows)
            {
                if (//全部转为string来判断是否相等，因为object箱结构不一样
                    DataRow[keyName].ToString() == keyValue.ToString()
                    )
                {
                    return DataRow;//返回符合被检索主键的行
                }
            }
            return null;//没找到返回bull
        }

        /// <summary>
        /// 抛出一个MySql连接（重载一）
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="port">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection getConnection(string dataSource, string port, string userName, string password)
        {
            try
            {
                //返回创建的连接
                return new MySqlConnection
                    (//组建连接信息
                    "Data source=" + dataSource + ";port="
                    + port + ";User Id=" + userName + ";password=" + password + ";"
                    );
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 抛出一个MySql连接（重载二）
        /// </summary>
        /// <param name="ConnSign">连接签名</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection getConnection(ConnSign ConnSign)
        {
            try
            {
                //返回创建的连接
                return new MySqlConnection
                    (//组建连接信息
                    "Data source=" + ConnSign.dataSource + ";port=" +
                    ConnSign.port + ";User Id=" + ConnSign.user + ";password=" + ConnSign.password + ";"
                    );
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取一张数据表（重载一）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="sql">用于查询数据表的SQL语句</param>
        /// <returns>返回一个DataTable对象，错误则返回null</returns>
        public DataTable getTable(MySqlConnection MySqlConnection, string sql)
        {
                //新建数据适配器
                MySqlDataAdapter myDA = new MySqlDataAdapter(sql, MySqlConnection);
                if (MySqlConnection.State == ConnectionState.Closed)//检测是否开启
                {
                    MySqlConnection.Open();
                }

                //新建数据表
                DataTable table = new DataTable();
                myDA.Fill(table);//填充数据到table

                return table;
        }
        /// <summary>
        /// 获取一张数据表（重载二）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回一个DataTable对象，错误则返回null</returns>
        public DataTable getTable(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
        {
                //将外来cmd设置为基于MySqlConnection执行
                MySqlCommand.Connection = MySqlConnection;
                //新建数据适配器，以外来cmd初始化
                MySqlDataAdapter myDA = new MySqlDataAdapter(MySqlCommand);
                if (MySqlConnection.State == ConnectionState.Closed)//检测是否开启
                {
                    MySqlConnection.Open();
                }

                //新建数据表
                DataTable table = new DataTable();
                myDA.Fill(table);//填充数据到table

                return table;
        }
        /// <summary>
        /// 获取一张数据表（重载三：HConnection介入）
        /// </summary>
        /// <param name="sql">SQL语句，用于查询数据表</param>
        /// <returns>返回一个DataTable对象，错误则返回null</returns>
        public DataTable getTable(string sql)
        {
                //新建数据适配器
                MySqlDataAdapter myDA = new MySqlDataAdapter(sql, HConnection);
                if (HConnection.State == ConnectionState.Closed)
                {
                    HConnection.Open();
                }

                //新建数据表
                DataTable table = new DataTable();
                myDA.Fill(table);//填充数据到DataTable

                return table;
        }
        /// <summary>
        /// 获取一张数据表（重载四：HConnection介入）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回一个DataTable对象，错误则返回null</returns>
        public DataTable getTable(MySqlCommand MySqlCommand)
        {
                //将外来cmd设置为基于HConnection执行
                MySqlCommand.Connection = HConnection;
                //新建数据适配器，以外来cmd初始化
                MySqlDataAdapter myDA = new MySqlDataAdapter(MySqlCommand);
                if (HConnection.State == ConnectionState.Closed)
                {
                    HConnection.Open();
                }

                //新建数据表
                DataTable table = new DataTable();
                myDA.Fill(table);//填充数据到DataTable

                return table;
        }

        /// <summary>
        /// 从数据表中提取取数据列
        /// </summary>
        /// <param name="DataTable">数据表实例</param>
        /// <param name="columnName">列名</param>
        /// <returns>返回非泛型List{object}实例，错误则返回null</returns>
        public List<object> getColumn(DataTable DataTable, string columnName)
        {
            try
            {
                List<object> list = new List<object>();
                foreach (DataRow DataRow in DataTable.Rows)
                {
                    list.Add(DataRow[columnName]);//将数据表中columnName列的所有行数据依次添加到list中
                }
                return list;//返回列
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 设置(替换)一个行的列值（字符串匹配）
        /// </summary>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="locateStr">用于定位行和列的结构体</param>
        /// <param name="whereColumnValue">定位列的列值</param>
        /// <param name="targetColumnValue">被改列的改值</param>
        /// <returns>操作成功返回true</returns>
        public bool setColumnValue(MySqlConnection MySqlConnection, Posi locateStr, string whereColumnValue, string targetColumnValue)
        {
            #region SQL字符串处理
            string sql = "UPDATE `" + locateStr.dataBase + "`.`" + locateStr.table
                       + "` SET `" + locateStr.targetColumn + "`= '" + targetColumnValue
                       + "' WHERE `" + locateStr.whereColumn + "`= '" + whereColumnValue + "';";
            #endregion
            HCommand = new MySqlCommand(sql, MySqlConnection);
            try
            {
                if (HCommand.Connection.State == ConnectionState.Closed)//查询连接状况
                {
                    HCommand.Connection.Open();//若连接被关闭则打开连接
                }

                if (HCommand.ExecuteNonQuery() > 0)
                { return true; }//查询成功返回true
                else
                { return false; }//查询失败返回false
            }
            finally//释放内存
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 设置(替换)一个行的列值（重载二：HConnection介入）（字符串匹配）
        /// </summary>
        /// <param name="locateStr">用于定位行和列的结构体</param>
        /// <param name="whereColumnValue">定位列的列值</param>
        /// <param name="targetColumnValue">被改列的改值</param>
        /// <returns>操作成功返回true</returns>
        public bool setColumnValue(Posi locateStr, string whereColumnValue, string targetColumnValue)
        {
            #region SQL字符串处理
            string sql = "UPDATE `" + locateStr.dataBase + "`.`" + locateStr.table
                       + "` SET `" + locateStr.targetColumn + "`= '" + targetColumnValue
                       + "' WHERE `" + locateStr.whereColumn + "`= '" + whereColumnValue + "';";
            #endregion
            HCommand = new MySqlCommand(sql, HConnection);
            try
            {
                if (HCommand.Connection.State == ConnectionState.Closed)//查询连接状况
                {
                    HCommand.Connection.Open();//若连接被关闭则打开连接
                }

                if (HCommand.ExecuteNonQuery() > 0)
                { return true; }//查询成功返回true
                else
                { return false; }//查询失败返回false
            }
            finally//释放内存
            {
                HCommand = null;
            }
        }

        /// <summary>
        /// 获得一个行的列值（字符串匹配）（重载一）,如果查询到多个行，则只返回第一行的数据
        /// </summary>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="locateStr">用于定位行和列的结构体</param>
        /// <param name="whereColumnValue">定位列的列值</param>
        /// <returns>操作成功返回true</returns>
        public object getColumnValue(MySqlConnection MySqlConnection, Posi locateStr, string whereColumnValue)
        {
            #region SQL字符串处理
            string sql = "select " + locateStr.targetColumn
                       + " from " + locateStr.dataBase + "." + locateStr.table
                       + " where " + locateStr.whereColumn + " = " + whereColumnValue;
            #endregion
            HCommand = new MySqlCommand(sql, MySqlConnection);
            try
            {
                if (HCommand.Connection.State == ConnectionState.Closed)//查询连接状况
                {
                    HCommand.Connection.Open();//若连接被关闭则打开连接
                }
                return HCommand.ExecuteScalar();
            }
            finally//释放内存
            {
                HCommand = null;
            }
        }
        /// <summary>
        /// 获得一个行的列值（字符串匹配）（重载二：HConnection介入）,如果查询到多个行，则只返回第一行的数据
        /// </summary>
        /// <param name="locateStr">用于定位行和列的结构体</param>
        /// <param name="whereColumnValue">定位列的列值</param>
        /// <returns>操作成功返回true</returns>
        public object getColumnValue(Posi locateStr, string whereColumnValue)
        {
            #region SQL字符串处理
            string sql = "select " + locateStr.targetColumn
                       + " from " + locateStr.dataBase + "." + locateStr.table
                       + " where " + locateStr.whereColumn + " = " + whereColumnValue;
            #endregion
            HCommand = new MySqlCommand(sql, HConnection);
            try
            {
                if (HCommand.Connection.State == ConnectionState.Closed)//查询连接状况
                {
                    HCommand.Connection.Open();//若连接被关闭则打开连接
                }
                return HCommand.ExecuteScalar();
            }
            finally//释放内存
            {
                HCommand = null;
            }
        }

        /// <summary>
        /// 建立参数化查询cmd对象
        /// </summary>
        /// <param name="sql">携带查询参数的SQL语句</param>
        /// <param name="paraList">查询参数列表</param>
        /// <returns>返回建立的参数化查询cmd对象</returns>
        public MySqlCommand paraQueryCmd(string sql, List<Para> paraList)
        {
            //建立cmd对象，用于执行参数化查询
            using (MySqlCommand MySqlCommand = new MySqlCommand(sql))
            {
                foreach (Para para in paraList)
                {
                    MySqlCommand.Parameters.AddWithValue(para.paraName, para.paraValue);//添加参数
                }
                return MySqlCommand;
            }
        }
    }
}