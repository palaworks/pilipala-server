using System;
using System.Text;

using System.IO;//系统IO命名空间(MCL using)
using System.Xml;//系统XML操作命名空间
using System.Collections;//泛型集合类
using System.Collections.Generic;//非泛型集合类
using System.Security.Cryptography;//hash/md5命名空间
using System.Net;//网页操作命名空间
using System.Drawing;//GDI+命名空间
using MySql.Data.MySqlClient;//MySql数据库
using System.Data;//ADO.NET
using System.Web.UI;
using System.Web;

namespace StdLib//StdLib1.11
{
    namespace DataHandler
    {
        /// <summary>
        /// 文件控制器
        /// </summary>
        public class FileHandler
        {
            /// <summary>
            /// 启动一个EXE文件
            /// </summary>
            /// <param name="filePath">文件所在本机物理地址</param>
            /// <returns>启动成功返回true</returns>
            public static bool runEXE(string filePath)
            {
                System.Diagnostics.Process.Start(filePath);
                return true;
            }

            /// <summary>
            /// 以字符串形式输出文件（重载一）（UTF8编码模式）
            /// </summary>
            /// <param name="url">文件所在的本地网络路径</param>
            /// <returns>返回字符串</returns>
            public string fileToStr(string url)
            {
                //读取url文件到文件尾，然后返回
                return FrameHandler.LibInformation.getStreamReader(url).ReadToEnd();
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
                return FrameHandler.LibInformation.getStreamReader(url, encodingType).ReadToEnd();
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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public bool createXml(xmlStr xmlStr)
            {
                try
                {
                    XmlDocument newDoc = new XmlDocument();//doc模式读写
                    XmlNode node_xml = newDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                    newDoc.AppendChild(node_xml);
                    XmlNode root = newDoc.CreateElement(xmlStr.rootName);//创建根节点
                    newDoc.AppendChild(root);//添加根节点

                    newDoc.Save(xmlStr.fileName + @"\" + xmlStr.xmlName + ".xml");
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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public bool addRealNode(xmlStr xmlStr)
            {
                try
                {
                    XmlNode parentNode = xDoc.SelectSingleNode(xmlStr.path);//父节点指定
                    XmlNode newNode = xDoc.CreateElement(xmlStr.nodeName);//创建新的子节点
                    XmlAttribute newAtt = xDoc.CreateAttribute(xmlStr.attName);//创建用于新的子节点的一个属性

                    newAtt.Value = xmlStr.attValue;//属性的值指定
                    newNode.Attributes.Append(newAtt);//添加属性到节点

                    newNode.InnerText = xmlStr.innerText;

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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public bool addEmptyNode(xmlStr xmlStr)
            {
                try
                {
                    XmlNode Pxn = xDoc.SelectSingleNode(xmlStr.path);
                    XmlNode Cxn = xDoc.CreateElement(xmlStr.nodeName);
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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public bool removeNode(xmlStr xmlStr)
            {
                try
                {
                    XmlNode baseNode = xDoc.SelectSingleNode(xmlStr.path);//指定父节点
                    XmlNodeList xnList = baseNode.ChildNodes;//初始化父节点的子节点列
                    foreach (XmlNode n in xnList)//遍历每一个节点
                    {
                        if (n.Name == xmlStr.nodeName)//判断节点名
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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回被读取的信息，传递未知的type返回"UnknownReadingType"，报错则返回null</returns>
            public string readInformation(xmlStr xmlStr)
            {
                try
                {
                    XmlNode xn = xDoc.SelectSingleNode(xmlStr.path);
                    switch (xmlStr.type)
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
            /// <param name="xmlStr">Xml文档信息通用结构体</param>
            /// <returns>通常返回被读取属性的值，报错则返回null</returns>
            public string readAttribute(xmlStr xmlStr)
            {
                try
                {
                    XmlNode xn = xDoc.SelectSingleNode(xmlStr.path);
                    return xn.Attributes[xmlStr.attName].Value;
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
        public class MySqlConnectionHandler
        {
            #region 注意
            /*
             * 内置查询方法不会关闭(销毁)任何一个数据库连接
             * 只会在安全性需要时清空HCommand，但不会关闭(销毁)
             * 调用关闭方法才会关闭相应连接或其他操作
             */
            #endregion
            private MySqlConnection HConnection;
            private MySqlCommand HCommand;

            /// <summary>
            /// close主连接
            /// </summary>
            /// <returns>成功返回ture，反之或报错返回false</returns>
            public bool HConnection_Close()
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
            public bool HConnection_Dispose()
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
            public bool HConnection_null()
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
            public bool HConnection_restart()
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
            public bool HCommand_Dispose()
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
            public bool HCommand_null()
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
            public bool HCommand_set(MySqlConnection MySqlConnection, string sql)
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
            public bool HCommand_set(string sql)
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
            /// <param name="connStr">用于参与连接数据库的文本结构</param>
            /// <returns>返回true，错误则返回false</returns>
            public bool start(connStr connStr)
            {
                //组建连接信息并创建连接
                HConnection = new MySqlConnection
                    (
                    "Data source=" + connStr.dataSource + ";port=" +
                    connStr.port + ";User Id=" + connStr.userName + ";password=" + connStr.password + ";"
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
            /// <param name="connStr">用于参与连接数据库的文本结构</param>
            /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
            public MySqlConnection getConnection(connStr connStr)
            {
                try
                {
                    //返回创建的连接
                    return new MySqlConnection
                        (//组建连接信息
                        "Data source=" + connStr.dataSource + ";port=" +
                        connStr.port + ";User Id=" + connStr.userName + ";password=" + connStr.password + ";"
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
                try
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
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 获取一张数据表（重载二）（适用于参数化查询）
            /// </summary>
            /// <param name="MySqlConnection">MySqlConnection连接实例</param>
            /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
            {
                try
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
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 获取一张数据表（重载三：HConnection介入）
            /// </summary>
            /// <param name="sql">SQL语句，用于查询数据表</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(string sql)
            {
                try
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
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 获取一张数据表（重载四：HConnection介入）（适用于参数化查询）
            /// </summary>
            /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(MySqlCommand MySqlCommand)
            {
                try
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
                catch
                {
                    return null;
                }
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
            public bool setColumnValue(MySqlConnection MySqlConnection, locateStr locateStr, string whereColumnValue, string targetColumnValue)
            {
                #region SQL字符串处理
                string sql = "UPDATE `" + locateStr.dataBaseName + "`.`" + locateStr.tableName
                           + "` SET `" + locateStr.targetColumnName + "`= '" + targetColumnValue
                           + "' WHERE `" + locateStr.whereColumnName + "`= '" + whereColumnValue + "';";
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
            public bool setColumnValue(locateStr locateStr, string whereColumnValue, string targetColumnValue)
            {
                #region SQL字符串处理
                string sql = "UPDATE `" + locateStr.dataBaseName + "`.`" + locateStr.tableName
                           + "` SET `" + locateStr.targetColumnName + "`= '" + targetColumnValue
                           + "' WHERE `" + locateStr.whereColumnName + "`= '" + whereColumnValue + "';";
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
            public object getColumnValue(MySqlConnection MySqlConnection, locateStr locateStr, string whereColumnValue)
            {
                #region SQL字符串处理
                string sql = "select " + locateStr.targetColumnName
                           + " from " + locateStr.dataBaseName + "." + locateStr.tableName
                           + " where " + locateStr.whereColumnName + " = " + whereColumnValue;
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
            public object getColumnValue(locateStr locateStr, string whereColumnValue)
            {
                #region SQL字符串处理
                string sql = "select " + locateStr.targetColumnName
                           + " from " + locateStr.dataBaseName + "." + locateStr.tableName
                           + " where " + locateStr.whereColumnName + " = " + whereColumnValue;
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
        }

    }

    namespace ViewHandler
    {
        namespace pilipala
        {
            interface IpalaRoot
            {
                /// <summary>
                /// 规定内置数据库名访问器
                /// </summary>
                string dataBaseName
                {
                    get;

                    set;
                }
            }

            /// <summary>
            /// 根级控制器
            /// </summary>
            public class palaRoot : IpalaRoot
            {
                private DataHandler.MySqlConnectionHandler MySqlConnectionHandler;
                private string DataBaseName;

                /// <summary>
                /// 内置数据库名访问器
                /// </summary>
                public string dataBaseName
                {
                    get
                    {
                        return DataBaseName;
                    }
                    set
                    {
                        DataBaseName = value;
                    }
                }

                /// <summary>
                /// 初始化对象（第一构造重载）
                /// </summary>
                /// <param name="MySqlConnectionHandler">数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public palaRoot(DataHandler.MySqlConnectionHandler MySqlConnectionHandler, string dataBaseName)
                {
                    this.MySqlConnectionHandler = MySqlConnectionHandler;
                    DataBaseName = dataBaseName;
                }
                /// <summary>
                /// 初始化对象（第二构造重载）
                /// </summary>
                /// <param name="connStr">连接文本结构体,用于创建数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public palaRoot(connStr connStr, string dataBaseName)
                {
                    MySqlConnectionHandler.start(connStr);
                    DataBaseName = dataBaseName;
                }

                /// <summary>
                /// 获得pilipala站点根计划
                /// </summary>
                /// <param name="plan_id">根计划id，用于指定被获取的根计划</param>
                /// <returns>返回根计划结构体palaPlan</returns>
                public palaPlan palaPlan(int plan_id)
                {
                    try
                    {
                        //新建根计划类型
                        palaPlan palaPlan = new palaPlan();

                        using (MySqlCommand MySqlCommand = new MySqlCommand("select * from " + DataBaseName + ".view_pala_root where plan_id = ?plan_id"))
                        {
                            MySqlCommand.Parameters.AddWithValue("?plan_id", plan_id);//添加参数
                            DataRow r = MySqlConnectionHandler.getRow(MySqlCommand);//获得行

                            //赋值
                            palaPlan.plan_id = Convert.ToInt32(r["plan_id"]);

                            palaPlan.site_devUrl = r["site_devUrl"].ToString();
                            palaPlan.site_finalUrl = r["site_finalUrl"].ToString();

                            palaPlan.site_isDev = Convert.ToBoolean(r["site_isDev"]);
                            palaPlan.site_isOpen = Convert.ToBoolean(r["site_isOpen"]);

                            //赋值完成后返回
                            return palaPlan;
                        }
                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }

                }
            }

            /// <summary>
            /// 站点函数
            /// </summary>
            public class palaFunction : Page, IpalaRoot
            {
                #region 字段、构造和接口实现

                private DataHandler.MySqlConnectionHandler MySqlConnectionHandler;
                private string DataBaseName;

                /// <summary>
                /// 内置数据库名访问器
                /// </summary>
                public string dataBaseName
                {
                    get
                    {
                        return DataBaseName;
                    }
                    set
                    {
                        DataBaseName = value;
                    }
                }

                /// <summary>
                /// 初始化对象（第一构造重载）
                /// </summary>
                /// <param name="MySqlConnectionHandler">数据库管理器实例，用于获取文章</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public palaFunction(DataHandler.MySqlConnectionHandler MySqlConnectionHandler, string dataBaseName)
                {
                    this.MySqlConnectionHandler = MySqlConnectionHandler;
                    DataBaseName = dataBaseName;
                }
                /// <summary>
                /// 初始化对象（第二构造重载）
                /// </summary>
                /// <param name="connStr">连接文本结构体,用于创建数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public palaFunction(connStr connStr, string dataBaseName)
                {
                    MySqlConnectionHandler.start(connStr);
                    DataBaseName = dataBaseName;
                }

                #endregion


                /// <summary>
                /// 随机取一个展示可用的文章数据（序列号和标题）
                /// </summary>
                /// <param name="post_id">文章序列号，用于排除某篇文章以不被随机，该值小于10000则视为进行不排除文章的随机方式</param>
                /// <returns>返回随机抓取到的文章数据</returns>
                public DataRow random(int post_id)
                {

                    //建立一个cmd对象，用于执行参数化查询
                    using
                        (/* 调用数据库存储过程，实现随机取样post_id和返回文章数据 */
                        MySqlCommand MySqlCommand = new MySqlCommand("call " + DataBaseName + ".random_post(?post_id)")
                        )
                    {
                        //添加参数
                        MySqlCommand.Parameters.AddWithValue("?post_id", post_id);
                        //获取单行数据，转为DataRow返回
                        return MySqlConnectionHandler.getRow(MySqlCommand);
                    }

                }

                /// <summary>
                /// 获取一个文章的插件数据
                /// </summary>
                /// <param name="post_id">文章序列号，用于指定被获取的文章</param>
                /// <returns>返回文章插件数据</returns>
                public pluginData pluginData(int post_id)
                {
                    //新建文章插件数据
                    pluginData pluginData = new pluginData();

                    using (MySqlCommand MySqlCommand = new MySqlCommand("select * from " + DataBaseName + ".pala_posts_plugin where post_id = ?post_id"))
                    {
                        MySqlCommand.Parameters.AddWithValue("?post_id", post_id);//添加参数

                        DataRow r = MySqlConnectionHandler.getRow(MySqlCommand);

                        //赋值文章列表单文章信息
                        pluginData.post_id = Convert.ToInt32(r["post_id"]);

                        pluginData.count_read = Convert.ToInt32(r["count_read"]);
                        pluginData.count_review = Convert.ToInt32(r["count_review"]);
                        pluginData.count_like = Convert.ToInt32(r["count_like"]);

                        pluginData.tagA = Convert.ToString(r["tagA"]);
                        pluginData.tagB = Convert.ToString(r["tagB"]);
                        pluginData.tagC = Convert.ToString(r["tagC"]);

                        pluginData.color_strip = Convert.ToString(r["color_strip"]);

                        return pluginData;
                    }
                }
            }

            /// <summary>
            /// Post获得器
            /// </summary>
            public class GetPost : Page, IpalaRoot
            {
                #region 字段、构造和接口实现

                private DataHandler.MySqlConnectionHandler MySqlConnectionHandler;
                private string DataBaseName;

                /// <summary>
                /// 内置数据库名访问器
                /// </summary>
                public string dataBaseName
                {
                    get
                    {
                        return DataBaseName;
                    }
                    set
                    {
                        DataBaseName = value;
                    }
                }

                /// <summary>
                /// 初始化对象（第一构造重载）
                /// </summary>
                /// <param name="MySqlConnectionHandler">数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public GetPost(DataHandler.MySqlConnectionHandler MySqlConnectionHandler, string dataBaseName)
                {
                    this.MySqlConnectionHandler = MySqlConnectionHandler;
                    DataBaseName = dataBaseName;
                }
                /// <summary>
                /// 初始化对象（第二构造重载）
                /// </summary>
                /// <param name="connStr">连接文本结构体,用于创建数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public GetPost(connStr connStr, string dataBaseName)
                {
                    MySqlConnectionHandler.start(connStr);
                    DataBaseName = dataBaseName;
                }

                #endregion


                /// <summary>
                /// 获取index页面普通文列数据
                /// </summary>
                /// <returns>返回普通文章List postData</returns>
                public List<postData> idx_comnPost()
                {
                    try
                    {
                        //定义List
                        List<postData> postDataList = new List<postData>();

                        //获得展示可用的文列文章postData表
                        using (DataTable postDataTable = MySqlConnectionHandler.getTable("select * from " + DataBaseName + ".view_idx_comn_plst"))
                        {
                            foreach (DataRow r in postDataTable.Rows)
                            {
                                //定义单postData来组成postData List
                                postData postData = new postData();

                                //赋值文章列表单文章信息
                                postData.post_id = Convert.ToInt32(r["post_id"]);
                                postData.post_title = Convert.ToString(r["post_title"]);
                                postData.post_summary = Convert.ToString(r["post_summary"]);
                                postData.post_archive = Convert.ToString(r["post_archive"]);

                                postData.date_created = Convert.ToDateTime(r["date_created"]);

                                postDataList.Add(postData);
                            }
                        }
                        return postDataList;
                    }
                    finally
                    {
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }

                }

                /// <summary>
                /// 获取index页面置顶文章数据 注意，只允许一个置顶文章出现在数据库中，一个以上的置顶文章会造成取文错误
                /// </summary>
                /// <returns>返回置顶文章postData</returns>
                public postData idx_topPost()
                {
                    try
                    {
                        postData postData = new postData();

                        //获得展示可用的置顶文章postData行
                        using (MySqlCommand MySqlCommand = new MySqlCommand("select * from " + DataBaseName + ".view_idx_top_plst"))
                        {

                            DataRow r = MySqlConnectionHandler.getRow(MySqlCommand);

                            //赋值文章列表单文章信息
                            postData.post_id = Convert.ToInt32(r["post_id"]);
                            postData.post_title = Convert.ToString(r["post_title"]);

                        }

                        return postData;
                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }

                }

                /// <summary>
                /// 获取index页面菜单文列数据
                /// </summary>
                /// <returns>返回菜单文列的List postData</returns>
                public List<postData> idx_menuPost()
                {
                    try
                    {
                        List<postData> postDataList = new List<postData>();

                        //获得展示可用的菜单文章postData表
                        using (DataTable postDataTable = MySqlConnectionHandler.getTable("select * from " + DataBaseName + ".view_idx_menu_plst"))
                        {
                            foreach (DataRow r in postDataTable.Rows)
                            {
                                //定义单postData来组成postData清单
                                postData e_postData = new postData();

                                //赋值文章列表单文章信息（1.1可用取代）
                                e_postData.post_id = Convert.ToInt32(r["post_id"]);
                                e_postData.post_title = Convert.ToString(r["post_title"]);

                                postDataList.Add(e_postData);
                            }
                        }

                        return postDataList;

                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }

                }

                /// <summary>
                /// 获取post页面的文章数据
                /// </summary>
                /// <param name="post_id">文章序列号，用于指定对哪篇文章进行操作</param>
                /// <returns>返回文章数据postData</returns>
                public postData pst_postData(int post_id)
                {
                    try
                    {
                        postData postData = new postData();

                        //建立一个cmd对象，用于执行参数化查询
                        using (MySqlCommand MySqlCommand = new MySqlCommand("select * from " + DataBaseName + ".view_pst_post_data where post_id = ?post_id"))
                        {
                            MySqlCommand.Parameters.AddWithValue("?post_id", post_id);//添加参数

                            //获得展示可用的文章postData
                            DataTable postDataTable = MySqlConnectionHandler.getTable(MySqlCommand);

                            foreach (DataRow r in postDataTable.Rows)
                            {
                                //赋值文章列表单文章信息
                                postData.post_id = Convert.ToInt32(r["post_id"]);
                                postData.post_title = Convert.ToString(r["post_title"]);
                                postData.post_summary = Convert.ToString(r["post_summary"]);
                                postData.post_content = Convert.ToString(r["post_content"]);
                                postData.post_archive = Convert.ToString(r["post_archive"]);
                                postData.post_isReadOnly = Convert.ToBoolean(r["post_isReadOnly"]);

                                postData.date_created = Convert.ToDateTime(r["date_created"]);
                                postData.date_changed = Convert.ToDateTime(r["date_changed"]);
                            }

                            return postData;
                        }

                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }

                }

            }

            /// <summary>
            /// Post编辑器
            /// </summary>
            public class SetPost : Page, IpalaRoot
            {
                #region 字段、构造和接口实现

                private DataHandler.MySqlConnectionHandler MySqlConnectionHandler;
                private string DataBaseName;

                /// <summary>
                /// 内置数据库名访问器
                /// </summary>
                public string dataBaseName
                {
                    get
                    {
                        return DataBaseName;
                    }
                    set
                    {
                        DataBaseName = value;
                    }
                }

                /// <summary>
                /// 初始化对象（第一构造重载）
                /// </summary>
                /// <param name="MySqlConnectionHandler">数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public SetPost(DataHandler.MySqlConnectionHandler MySqlConnectionHandler, string dataBaseName)
                {
                    this.MySqlConnectionHandler = MySqlConnectionHandler;
                    DataBaseName = dataBaseName;
                }
                /// <summary>
                /// 初始化对象（第二构造重载）
                /// </summary>
                /// <param name="connStr">连接文本结构体,用于创建数据库管理器实例</param>
                /// <param name="dataBaseName">pilipala数据库名</param>
                public SetPost(connStr connStr, string dataBaseName)
                {
                    MySqlConnectionHandler.start(connStr);
                    DataBaseName = dataBaseName;
                }

                #endregion


                /// <summary>
                /// 重设浏览计数
                /// </summary>
                /// <param name="post_id">文章序列号，用于确定是哪一篇文章被修改</param>
                /// <param name="value">计数被重设的值</param>
                /// <returns>操作成功返回true，失败则false</returns>
                public bool count_read(int post_id, int value)
                {
                    try
                    {
                        //初始化目标行定位数据
                        locateStr ls = new locateStr();
                        ls.dataBaseName = DataBaseName;
                        ls.tableName = "pala_posts_plugin";
                        ls.whereColumnName = "post_id";
                        ls.targetColumnName = "count_read";

                        return MySqlConnectionHandler.setColumnValue(ls, post_id.ToString(), value.ToString());

                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }


                }

                /// <summary>
                /// 重设点赞计数
                /// </summary>
                /// <param name="post_id">文章序列号，用于确定是哪一篇文章被修改</param>
                /// <param name="value">计数被重设的值</param>
                /// <returns>操作成功返回true，失败则false</returns>
                public bool count_like(int post_id, int value)
                {
                    try
                    {
                        //初始化目标行定位数据
                        locateStr ls = new locateStr();
                        ls.dataBaseName = DataBaseName;
                        ls.tableName = "pala_posts_plugin";
                        ls.whereColumnName = "post_id";
                        ls.targetColumnName = "count_like";

                        return MySqlConnectionHandler.setColumnValue(ls, post_id.ToString(), value.ToString());

                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }


                }

            }

            /// <summary>
            /// 网页状态获得器
            /// </summary>
            public class GetStatus : Page
            {
                /// <summary>
                /// 判断Cookie对象是否存在
                /// </summary>
                /// <param name="CookieName">被判断Cookie对象的名称</param>
                /// <returns>存在返回true，反之false</returns>
                public bool isCookiesExist(string CookieName)
                {
                    if (HttpContext.Current.Request.Cookies[CookieName] == null)//如果Cookie对象为null（不存在）
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                /// <summary>
                /// 判断Cookie对象是否存在（重载二：判断索引是否存在）
                /// </summary>
                /// <param name="CookieName">被判断Cookie对象的名称</param>
                /// <param name="keyName">索引名，属于被判断的Cookie</param>
                /// <returns>存在返回true，反之false</returns>
                public bool isCookiesExist(string CookieName, string keyName)
                {
                    if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
                    {
                        if (HttpContext.Current.Request.Cookies[CookieName][keyName].ToString() != "")//如果Cookie中的keyName索引键值为空字符串
                        {
                            return false;//不存在
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

                /// <summary>
                /// 读取Cookie对象到指定类型
                /// </summary>
                /// <typeparam name="T">指定泛型</typeparam>
                /// <param name="CookieName">被读取Cookie对象名</param>
                /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
                public T cookie<T>(string CookieName)
                {
                    if (isCookiesExist(CookieName) == true)//如果Cookie对象存在
                    {
                        if (HttpContext.Current.Request.Cookies[CookieName].Value == "")
                        {
                            return default(T);//如果Cookie对象值为空字符串（null），返回泛型默认值
                        }
                        else
                        {
                            if (HttpContext.Current.Request.Cookies[CookieName].Value == null)
                            {
                                return default(T);
                            }
                            else
                            {
                                //如果Cookie对象的索引值不为null，转换为泛型返回
                                return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName].Value, typeof(T));
                            }
                        }
                    }
                    else
                    {
                        return default(T);
                    }

                }
                /// <summary>
                /// 读取Cookie对象的指定索引值到指定类型
                /// </summary>
                /// <typeparam name="T">指定泛型</typeparam>
                /// <param name="CookieName">被读取Cookie对象名</param>
                /// <param name="keyName">索引名，属于当前Cookie对象</param>
                /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
                public T cookie<T>(string CookieName, string keyName)
                {
                    if (isCookiesExist(CookieName, keyName) == true)
                    {
                        //如果索引键值不为空字符串，转换为泛型返回
                        return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[CookieName][keyName].ToString(), typeof(T));
                    }
                    else
                    {
                        return default(T);
                    }

                }

                /// <summary>
                /// 读取Session对象到指定类型
                /// </summary>
                /// <typeparam name="T">指定泛型</typeparam>
                /// <param name="varName">变量名，属于Session对象，承担该操作</param>
                /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
                public T session<T>(string varName)
                {
                    //测试表明，Session对象始终存在，并且不为null，所以可以不加判断直接转换
                    return (T)Convert.ChangeType(HttpContext.Current.Session[varName], typeof(T));//转换为自定义类型
                }
            }

            /// <summary>
            /// 网页状态设置器
            /// </summary>
            public class SetStatus : Page
            {

                /// <summary>
                /// 设置Cookie值
                /// </summary>
                /// <param name="CookieName">Cookie名，承担该操作</param>
                /// <param name="value">设置值</param>
                /// <returns>设置成功返回true，反之false</returns>
                public bool setCookie(string CookieName, object value)
                {
                    HttpContext.Current.Response.Cookies[CookieName].Value = value.ToString();
                    return true;
                }
                /// <summary>
                /// 设置Cookie值（重载二：索引设置）
                /// </summary>
                /// <param name="CookieName">Cookie名，承担该操作</param>
                /// <param name="keyName">索引名，属于承担该操作的Cookie</param>
                /// <param name="value">设置值</param>
                /// <returns>设置成功返回true，反之false</returns>
                public bool setCookie(string CookieName, string keyName, object value)
                {
                    HttpContext.Current.Response.Cookies[CookieName][keyName] = value.ToString();
                    return true;
                }
                /// <summary>
                /// 设置Session对象的变量值
                /// </summary>
                /// <param name="varName">变量名，属于Session对象，承担该操作</param>
                /// <param name="value">设置值</param>
                /// <returns>设置成功返回true，反之false</returns>
                public bool setSession(string varName, object value)
                {
                    HttpContext.Current.Session[varName] = value;
                    return true;
                }

            }

        }
    }
}
