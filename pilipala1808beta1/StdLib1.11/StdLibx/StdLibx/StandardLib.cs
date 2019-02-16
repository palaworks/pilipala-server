using System;
using System.Text;

using System.IO;//系统IO命名空间(MCL using)
using System.Xml;//系统XML操作命名空间(MCL using)
using System.Collections;//泛型集合类命名空间
using System.Collections.Generic;//非泛型集合命名空间
using System.Security.Cryptography;//hash/md5命名空间
using System.Net;//网页操作命名空间
using System.Drawing;//GDI+命名空间
using MySql.Data.MySqlClient;//MySql数据库命名空间
using System.Data;//ADO.NET类结构访问命名空间
using System.Web.UI;
using System.Data.SqlClient;//SQLserver

//将StdLib全部引用
using StdLib;
using StdLib.DataHandler;
using StdLib.FrameHandler;
using StdLib.LogicHandler;
using StdLib.ViewHandler.WebSite;

namespace StdLib//StdLib1.09
{
    namespace DataHandler
    {
        /// <summary>
        /// 程序启动器
        /// </summary>
        public static class exeLoader
        {

            /// <summary>
            /// 第一重载，启动1个应用
            /// </summary>
            /// <param name="file1">应用目录</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public static bool run(string file1)
            {
                try
                {
                    System.Diagnostics.Process.Start(file1);
                    return true;//成功返回true
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 第二重载，启动2个应用
            /// </summary>
            /// <param name="file1">应用目录1</param>
            /// <param name="file2">应用目录2</param>
            /// <returns>通常返回true，报错则返回false</returns>
            public static bool run(string file1, string file2)
            {
                try
                {
                    System.Diagnostics.Process.Start(file1);
                    System.Diagnostics.Process.Start(file2);
                    return true;
                }
                catch
                {
                    return false;
                }
            }


        }

        /// <summary>
        /// 组标准Xml数据格式读写类
        /// </summary>
        public sealed class XmlHandler
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
            /// 创建Xml文档的方法（第一重载）
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
            /// 创建Xml文档的方法（第二重载）
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
            /// 添加实节点的方法（第一重载）
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
            /// 添加实节点的方法（第二重载）
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
            /// 添加空节点的方法（第一重载）
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
            /// 添加空节点的方法（第二重载）
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
            /// 删除被指定的父节点下子节点的方法（第一重载）
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
            /// 删除被指定的父节点下子节点的方法（第二重载）
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
            /// 读取被指定的实节点的信息的方法（第一重载）
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
            /// 读取被指定的实节点的信息的方法（第二重载）
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
            /// 读取被指定的实节点的属性值的方法（第一重载）
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
            /// 读取被指定的实节点的属性值的方法（第二重载）
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
        /// MSSql数据库操作类（试行）
        /// </summary>
        public class MSSqlConnectionHandler
        {
            private SqlConnection HConnection;
            private SqlCommand HCommand;

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
            /// 设置主命令行的sql语句（第一重载）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
            /// </summary>
            /// <param name="SqlConnection">要求主命令行执行的SqlConnection连接实例</param>
            /// <param name="sql">被设置的sql语句</param>
            /// <returns>成功返回ture，反之或报错返回false</returns>
            public bool HCommand_set(SqlConnection SqlConnection, string sql)
            {
                try
                {
                    HCommand = new SqlCommand(sql, SqlConnection);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 设置主命令行的sql语句（第二重载：HConnection介入）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
            /// </summary>
            /// <param name="sql">被设置的sql语句</param>
            /// <returns>成功返回ture，反之或报错返回false</returns>
            public bool HCommand_set(string sql)
            {
                try
                {
                    HCommand = new SqlCommand(sql, HConnection);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 启动连接（第一重载：HConnection介入）
            /// </summary>
            /// <param name="dataSource">数据源</param>
            /// <param name="port">端口</param>
            /// <param name="userName">用户名</param>
            /// <param name="password">密码</param>
            /// <returns>返回true，错误则返回null</returns>
            public bool start(string dataSource, string port, string userName, string password)
            {
                //组建连接信息并创建连接
                HConnection = new SqlConnection
                    (
                    "Data source=" + dataSource + ";port="
                    + port + ";User Id=" + userName + ";password=" + password + ";"
                    );
                return start(HConnection);
            }
            /// <summary>
            /// 启动连接（第二重载：HConnection介入）
            /// </summary>
            /// <param name="connStr">用于参与连接数据库的文本结构</param>
            /// <returns>返回true，错误则返回false</returns>
            public bool start(connStr connStr)
            {
                //组建连接信息并创建连接
                HConnection = new SqlConnection
                    (
                    "Data source=" + connStr.dataSource + ";port=" +
                    connStr.port + ";User Id=" + connStr.userName + ";password=" + connStr.password + ";"
                    );
                return start(HConnection);
            }
            /// <summary>
            /// 启动连接（第三重载）
            /// </summary>
            /// <param name="SqlConnection">SqlConnection连接实例</param>
            /// <returns>返回true，错误则返回false</returns>
            public bool start(SqlConnection SqlConnection)
            {
                switch (SqlConnection.State)
                {
                    case ConnectionState.Closed:
                        SqlConnection.Open();
                        if (SqlConnection.State == ConnectionState.Open)
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
            /// 执行一个查询（第一重载）
            /// </summary>
            /// <param name="SqlConnection">SqlConnection连接实例</param>
            /// <param name="sql">SQL语句</param>
            /// <returns>返回查询结果，错误则返回null</returns>
            public SqlDataReader search(SqlConnection SqlConnection, string sql)
            {
                try
                {
                    HCommand = new SqlCommand(sql, SqlConnection);

                    if (HCommand.Connection.State == ConnectionState.Closed)//判断连接是否被关闭
                    {
                        HCommand.Connection.Open();//连接关闭则打开
                    }
                    return HCommand.ExecuteReader();//返回查询结果
                }
                catch
                {
                    return null;//抛出null
                }
                finally//释放资源
                {
                    SqlConnection.Close();
                    HCommand = null;
                }
            }
            /// <summary>
            /// 执行一个查询（第二重载：HConnection介入）
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>返回查询结果，错误则返回Exception e的文本信息</returns>
            public SqlDataReader search(string sql)
            {
                try
                {
                    HCommand = new SqlCommand(sql, HConnection);

                    if (HCommand.Connection.State == ConnectionState.Closed)//判断连接是否被关闭
                    {
                        HCommand.Connection.Open();//连接关闭则打开
                    }
                    return HCommand.ExecuteReader();//返回查询结果
                }
                catch
                {
                    return null;//抛出null
                }
                finally//释放资源
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }

            /// <summary>
            /// 抛出一个Sql连接（第一重载）
            /// </summary>
            /// <param name="dataSource">数据源</param>
            /// <param name="port">端口</param>
            /// <param name="userName">用户名</param>
            /// <param name="password">密码</param>
            /// <returns>返回一个SqlConnection对象，错误则返回null</returns>
            public SqlConnection getConnection(string dataSource, string port, string userName, string password)
            {
                try
                {
                    //返回创建的连接
                    return new SqlConnection
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
            /// 抛出一个Sql连接（第二重载）
            /// </summary>
            /// <param name="connStr">用于参与连接数据库的文本结构</param>
            /// <returns>返回一个SqlConnection对象，错误则返回null</returns>
            public SqlConnection getConnection(connStr connStr)
            {
                try
                {
                    //返回创建的连接
                    return new SqlConnection
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
            /// 获取一张数据表（第一重载）
            /// </summary>
            /// <param name="SqlConnection">SqlConnection连接实例</param>
            /// <param name="sql">用于查询数据表的SQL语句</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(SqlConnection SqlConnection, string sql)
            {
                try
                {
                    SqlDataAdapter mda = new SqlDataAdapter(sql, SqlConnection);
                    if (SqlConnection.State == ConnectionState.Closed)//检测是否开启
                    {
                        SqlConnection.Open();
                    }

                    DataTable table = new DataTable();
                    mda.Fill(table);//填充数据到table

                    return table;
                }
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 获取一张数据表（第二重载：HConnection介入）
            /// </summary>
            /// <param name="sql">用于查询数据表的SQL语句</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(string sql)
            {
                try
                {
                    if (HConnection.State == ConnectionState.Closed)
                    {
                        HConnection.Open();
                    }

                    SqlDataAdapter mda = new SqlDataAdapter(sql, HConnection);
                    DataTable table = new DataTable();
                    mda.Fill(table);//填充数据到table

                    return table;
                }
                catch
                {
                    return null;
                }
                finally//释放内存
                {
                    HConnection.Close();
                }
            }

            /// <summary>
            /// 获取数据列
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
            /// 获取数据行
            /// </summary>
            /// <param name="DataTable">数据表实例</param>
            /// <param name="primaryKeyName">主键名</param>
            /// <param name="primaryKey">主键</param>
            /// <returns>返回获得的DataRow数据行实例，未检索到或错误返回null</returns>
            public DataRow getRow(DataTable DataTable, string primaryKeyName, object primaryKey)
            {
                try
                {
                    foreach (DataRow DataRow in DataTable.Rows)
                    {
                        if (//全部转为string来判断是否相等，因为object箱结构不一样
                            DataRow[primaryKeyName].ToString() == primaryKey.ToString()
                            &&
                            DataRow != null
                            )
                        {
                            return DataRow;//返回符合被检索主键的行
                        }
                    }
                    return null;//没找到返回bull
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// 更改指定行的指定列数据
            /// </summary>
            /// <param name="SqlConnection">SqlConnection连接实例</param>
            /// <param name="dataBaseName">被操作表所在的数据库</param>
            /// <param name="tableName">被操作表</param>
            /// <param name="primaryKeyName">作为索引的主键</param>
            /// <param name="columnName">参与更改任务的列</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(SqlConnection SqlConnection, string dataBaseName, string tableName, string primaryKeyName, string columnName, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + dataBaseName + "`.`" + tableName
                            + "` SET `" + columnName + "`= '" + value
                            + "' WHERE `" + primaryKeyName + "`= '" + primaryKeyValue + "';";
                #endregion
                HCommand = new SqlCommand(sql, SqlConnection);
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第二重载）
            /// </summary>
            /// <param name="SqlConnection">SqlConnection连接实例</param>
            /// <param name="selectStr">用于定位查询操作对象的文本结构</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(SqlConnection SqlConnection, selectStr selectStr, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + selectStr.dataBaseName + "`.`" + selectStr.tableName
                            + "` SET `" + selectStr.columnName + "`= '" + value
                            + "' WHERE `" + selectStr.primaryKeyName + "`= '" + primaryKeyValue + "';";
                #endregion
                HCommand = new SqlCommand(sql, SqlConnection);
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第三重载：HConnection介入）
            /// </summary>
            /// <param name="dataBaseName">被操作表所在的数据库</param>
            /// <param name="tableName">被操作表</param>
            /// <param name="primaryKeyName">作为索引的主键</param>
            /// <param name="columnName">参与更改任务的列</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(string dataBaseName, string tableName, string primaryKeyName, string columnName, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + dataBaseName + "`.`" + tableName
                            + "` SET `" + columnName + "`= '" + value
                            + "' WHERE `" + primaryKeyName + "`= '" + primaryKeyValue + "';";
                #endregion
                HCommand = new SqlCommand(sql, HConnection);
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第四重载：HConnection介入）
            /// </summary>
            /// <param name="selectStr">用于定位查询操作对象的文本结构</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(selectStr selectStr, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + selectStr.dataBaseName + "`.`" + selectStr.tableName
                            + "` SET `" + selectStr.columnName + "`= '" + value
                            + "' WHERE `" + selectStr.primaryKeyName + "`= '" + primaryKeyValue + "';";
                #endregion
                HCommand = new SqlCommand(sql, HConnection);
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }
        }

        /// <summary>
        /// MySql数据库操作类
        /// </summary>
        public class MySqlConnectionHandler
        {
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
            /// 设置主命令行的sql语句（第一重载）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
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
            /// 设置主命令行的sql语句（第二重载：HConnection介入）（注意：此方法可能会引起未知的ACID问题，建议仅供调试使用）
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
            /// 启动连接（第一重载：HConnection介入）
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
            /// 启动连接（第二重载：HConnection介入）
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
            /// 启动连接（第三重载）
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
            /// 执行一个查询（第一重载）
            /// </summary>
            /// <param name="MySqlConnection">MySqlConnection连接实例</param>
            /// <param name="sql">SQL语句</param>
            /// <returns>返回查询结果，错误则返回null</returns>
            public MySqlDataReader search(MySqlConnection MySqlConnection, string sql)
            {
                try
                {
                    HCommand = new MySqlCommand(sql, MySqlConnection);

                    if (HCommand.Connection.State == ConnectionState.Closed)//判断连接是否被关闭
                    {
                        HCommand.Connection.Open();//连接关闭则打开
                    }
                    return HCommand.ExecuteReader();//返回查询结果
                }
                catch
                {
                    return null;//抛出null
                }
                finally//释放资源
                {
                    MySqlConnection.Close();
                    HCommand = null;
                }
            }
            /// <summary>
            /// 执行一个查询（第二重载：HConnection介入）
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>返回查询结果，错误则返回Exception e的文本信息</returns>
            public MySqlDataReader search(string sql)
            {
                try
                {
                    HCommand = new MySqlCommand(sql, HConnection);

                    if (HCommand.Connection.State == ConnectionState.Closed)//判断连接是否被关闭
                    {
                        HCommand.Connection.Open();//连接关闭则打开
                    }
                    return HCommand.ExecuteReader();//返回查询结果
                }
                catch
                {
                    return null;//抛出null
                }
                finally//释放资源
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }

            /// <summary>
            /// 抛出一个MySql连接（第一重载）
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
            /// 抛出一个MySql连接（第二重载）
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
            /// 获取一张数据表（第一重载）
            /// </summary>
            /// <param name="MySqlConnection">MySqlConnection连接实例</param>
            /// <param name="sql">用于查询数据表的SQL语句</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(MySqlConnection MySqlConnection, string sql)
            {
                try
                {
                    MySqlDataAdapter mda = new MySqlDataAdapter(sql, MySqlConnection);
                    if (MySqlConnection.State == ConnectionState.Closed)//检测是否开启
                    {
                        MySqlConnection.Open();
                    }

                    DataTable table = new DataTable();
                    mda.Fill(table);//填充数据到table

                    return table;
                }
                catch
                {
                    return null;
                }
            }
            /// <summary>
            /// 获取一张数据表（第二重载：HConnection介入）
            /// </summary>
            /// <param name="sql">用于查询数据表的SQL语句</param>
            /// <returns>返回一个DataTable对象，错误则返回null</returns>
            public DataTable getTable(string sql)
            {
                try
                {
                    if (HConnection.State == ConnectionState.Closed)
                    {
                        HConnection.Open();
                    }

                    MySqlDataAdapter mda = new MySqlDataAdapter(sql, HConnection);
                    DataTable table = new DataTable();
                    mda.Fill(table);//填充数据到table

                    return table;
                }
                catch
                {
                    return null;
                }
                finally//释放内存
                {
                    HConnection.Close();
                }
            }

            /// <summary>
            /// 获取数据列
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
            /// 使用匹配键值的方法获取数据行
            /// </summary>
            /// <param name="DataTable">数据表实例</param>
            /// <param name="keyName">键名</param>
            /// <param name="keyValue">键值</param>
            /// <returns>返回获得的DataRow数据行实例，未检索到或错误返回null</returns>
            public DataRow getRow(DataTable DataTable, string keyName, object keyValue)
            {
                try
                {
                    foreach (DataRow DataRow in DataTable.Rows)
                    {
                        if (//全部转为string来判断是否相等，因为object箱结构不一样
                            DataRow[keyName].ToString() == keyValue.ToString()
                            &&
                            DataRow != null
                            )
                        {
                            return DataRow;//返回符合被检索主键的行
                        }
                    }
                    return null;//没找到返回bull
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// 更改指定行的指定列数据
            /// </summary>
            /// <param name="MySqlConnection">MySqlConnection连接实例</param>
            /// <param name="dataBaseName">被操作表所在的数据库</param>
            /// <param name="tableName">被操作表</param>
            /// <param name="primaryKeyName">作为索引的主键</param>
            /// <param name="columnName">参与更改任务的列</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(MySqlConnection MySqlConnection, string dataBaseName, string tableName, string primaryKeyName, string columnName, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + dataBaseName + "`.`" + tableName
                            + "` SET `" + columnName + "`= '" + value
                            + "' WHERE `" + primaryKeyName + "`= '" + primaryKeyValue + "';";
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第二重载）
            /// </summary>
            /// <param name="MySqlConnection">MySqlConnection连接实例</param>
            /// <param name="selectStr">用于定位查询操作对象的文本结构</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(MySqlConnection MySqlConnection, selectStr selectStr, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + selectStr.dataBaseName + "`.`" + selectStr.tableName
                            + "` SET `" + selectStr.columnName + "`= '" + value
                            + "' WHERE `" + selectStr.primaryKeyName + "`= '" + primaryKeyValue + "';";
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第三重载：HConnection介入）
            /// </summary>
            /// <param name="dataBaseName">被操作表所在的数据库</param>
            /// <param name="tableName">被操作表</param>
            /// <param name="primaryKeyName">作为索引的主键</param>
            /// <param name="columnName">参与更改任务的列</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(string dataBaseName, string tableName, string primaryKeyName, string columnName, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + dataBaseName + "`.`" + tableName
                            + "` SET `" + columnName + "`= '" + value
                            + "' WHERE `" + primaryKeyName + "`= '" + primaryKeyValue + "';";
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }
            /// <summary>
            /// 更改指定行的指定列数据（第四重载：HConnection介入）
            /// </summary>
            /// <param name="selectStr">用于定位查询操作对象的文本结构</param>
            /// <param name="primaryKeyValue">被操作行的主键键值</param>
            /// <param name="value">被操作行的被操作列所要添加的值</param>
            /// <returns>执行成功返回true，报错则返回false</returns>
            public bool alterData(selectStr selectStr, string primaryKeyValue, string value)
            {
                #region SQL字符串处理
                //定位到数据库的数据表
                //更新列值
                //通过主键检索确定行
                string sql = "UPDATE `" + selectStr.dataBaseName + "`.`" + selectStr.tableName
                            + "` SET `" + selectStr.columnName + "`= '" + value
                            + "' WHERE `" + selectStr.primaryKeyName + "`= '" + primaryKeyValue + "';";
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
                catch
                {
                    return false;
                }
                finally//释放内存
                {
                    HConnection.Close();
                    HCommand = null;
                }
            }
        }

    }

    namespace ViewHandler
    {
        namespace WebSite
        {

            /// <summary>
            /// 文章处理及输出控制类
            /// </summary>
            public sealed class PageHandler : Page
            {
                private DataHandler.MySqlConnectionHandler MySqlConnectionHandler;
                /// <summary>
                /// 初始化Post对象（第一构造重载）
                /// </summary>
                /// <param name="MySqlConnectionHandler">用于获取Post信息的MySqlConnectionHandler实例</param>
                public PageHandler(DataHandler.MySqlConnectionHandler MySqlConnectionHandler)
                {
                    this.MySqlConnectionHandler = MySqlConnectionHandler;
                }
                /// <summary>
                /// 初始化Post对象（第二构造重载）
                /// </summary>
                /// <param name="connStr">用于创建Post实例的数据库连接文本结构体</param>
                public PageHandler(connStr connStr)
                {
                    MySqlConnectionHandler.start(connStr);
                }

                /// <summary>
                /// 获取文章全部信息
                /// </summary>
                /// <param name="postTableName">用于存储文章的数据表</param>
                /// <returns>返回带有文章信息的非泛型集合类，报错则返回null</returns>
                public List<postData> getData(string postTableName)
                {
                    try
                    {
                        List<postData> LstPd = new List<postData>();
                        DataTable dt = MySqlConnectionHandler.getTable("select * from " + postTableName);

                        foreach (DataRow DataRow in dt.Rows)
                        {
                            postData pd = new postData();
                            //考虑到数据库中存在文章非必要信息为null的情况，所以此处全部使用Convert
                            pd.post_id = Convert.ToInt32(DataRow["post_id"]);
                            pd.post_title = Convert.ToString(DataRow["post_title"]);
                            pd.post_summary = Convert.ToString(DataRow["post_summary"]);
                            pd.post_content = Convert.ToString(DataRow["post_content"]);
                            pd.post_archive = Convert.ToString(DataRow["post_archive"]);
                            pd.post_isShow = Convert.ToBoolean(DataRow["post_isShow"]);
                            pd.post_type = Convert.ToString(DataRow["post_type"]);
                            pd.post_author_id = Convert.ToInt32(DataRow["post_author_id"]);

                            pd.date_created = Convert.ToDateTime(DataRow["date_created"]);
                            pd.date_changed = Convert.ToDateTime(DataRow["date_changed"]);

                            pd.count_browse = Convert.ToInt32(DataRow["count_browse"]);
                            pd.count_review = Convert.ToInt32(DataRow["count_review"]);
                            pd.count_like= Convert.ToInt32(DataRow["count_like"]);

                            pd.tagA = Convert.ToString(DataRow["tagA"]);
                            pd.tagB = Convert.ToString(DataRow["tagB"]);
                            pd.tagC = Convert.ToString(DataRow["tagC"]);

                            pd.path_cover = Convert.ToString(DataRow["path_cover"]);
                            pd.color_strip = Convert.ToString(DataRow["color_strip"]);

                            LstPd.Add(pd);//将数据表中columnName列的所有行数据依次添加到list中
                        }
                        return LstPd;
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        MySqlConnectionHandler.HConnection_Close();
                        MySqlConnectionHandler.HCommand_null();
                        MySqlConnectionHandler.HCommand_Dispose();
                    }
                }

                /// <summary>
                /// 根据ID输出文章标题
                /// </summary>
                /// <param name="LstPd">带有文章信息的非泛型集合类</param>
                /// <param name="post_ID">文章ID</param>
                /// <returns>返回文章标题，报错则返回null</returns>
                public string showPostTitle(List<postData> LstPd, int post_ID)
                {
                    foreach (postData pd in LstPd)
                    {
                        if (pd.post_id == post_ID)
                        {
                            Response.Write(pd.post_title);
                            return pd.post_title;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// 根据ID输出文章概要
                /// </summary>
                /// <param name="LstPd">带有文章信息的非泛型集合类</param>
                /// <param name="post_ID">文章ID</param>
                /// <returns>返回文章概要，报错则返回null</returns>
                public string showPostSummary(List<postData> LstPd, int post_ID)
                {
                    foreach (postData pd in LstPd)
                    {
                        if (pd.post_id == post_ID)
                        {
                            Response.Write(pd.post_summary);
                            return pd.post_summary;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// 根据ID输出文章内容
                /// </summary>
                /// <param name="LstPd">带有文章信息的非泛型集合类</param>
                /// <param name="post_ID">文章ID</param>
                /// <returns>返回文章内容，报错则返回null</returns>
                public string showPostContent(List<postData> LstPd, int post_ID)
                {
                    foreach (postData pd in LstPd)
                    {
                        if (pd.post_id == post_ID)
                        {
                            Response.Write(pd.post_summary);
                            return pd.post_summary;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// 根据ID输出文章封面链接
                /// </summary>
                /// <param name="LstPd">带有文章信息的非泛型集合类</param>
                /// <param name="post_ID">文章ID</param>
                /// <returns>返回文章封面链接，报错则返回null</returns>
                public string showCoverPath(List<postData> LstPd, int post_ID)
                {
                    foreach (postData pd in LstPd)
                    {
                        if (pd.post_id == post_ID)
                        {
                            Response.Write(pd.path_cover);
                            return pd.path_cover;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// 以字符串形式输出文件（第二重载）
                /// </summary>
                /// <param name="url">文件所在的本地物理路径</param>
                /// <returns>返回字符串，错误则返回null</returns>
                public string outputFileString(string url)
                {
                    try
                    {
                        //序列化/反序列化对象JavaScriptSerializer
                        //转换json为JlinfObject对象并返回
                        return FrameHandler.LibInformation.getStreamReader(url).ReadToEnd();
                    }
                    catch
                    {
                        return null;
                    }
                }

                /// <summary>
                /// 以字符串形式输出文件（第二重载）
                /// </summary>
                /// <param name="filePath">文件所在的本地物理路径</param>
                /// <param name="bufferSize">文件流缓冲区大小，默认值可填4096</param>
                /// <param name="useAsync">使用异步初始化文件流，缺乏设计的异步调用会慢于串行调用</param>
                /// <param name="encodingType">解析文件所用的编码模式</param>
                /// <returns>返回字符串，错误则返回null</returns>
                public string outputFileString(string filePath, int bufferSize, bool useAsync, string encodingType)
                {
                    try
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
                    catch
                    {
                        return null;
                    }
                }
            }
        }
    }
}