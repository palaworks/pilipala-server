using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Data;

using WaterLibrary.stru.MySQL;

namespace WaterLibrary.com.MySQL
{
    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class MySqlConnH
    {

        private MySqlConnection HConnection;
        private MySqlCommand HCommand;

        /// <summary>
        /// Close主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool CloseHConnection()
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
        /// Dispose主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool DisposeHConnection()
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
        public bool NullHConnection()
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
        public bool ReStartHConnection()
        {
            try
            {
                //考虑到重启连接需花费一定时间的假设，该方法只提供了重启操作，而不去判断是否执行成功。
                switch (HConnection.State)
                {
                    case ConnectionState.Closed:
                        HConnection.Open();
                        return true;

                    case ConnectionState.Open:
                        HConnection.Close();
                        HConnection.Open();

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
        /// dispose主命令行
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool DisposeHCommand()
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
        public bool NullHCommand()
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
        /// <param name="SQL">被设置的sql语句</param>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool SetHCommand(MySqlConnection MySqlConnection, string SQL)
        {
            try
            {
                HCommand = new MySqlCommand(SQL, MySqlConnection);
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
        public bool SetHCommand(string sql)
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
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回true，错误则返回null</returns>
        public bool Start(string DataSource, string Port, string User, string PWD)
        {
            //组建连接信息并创建连接
            HConnection = new MySqlConnection
                (
                "Data source=" + DataSource + ";port="
                + Port + ";User Id=" + User + ";password=" + PWD + ";"
                );
            return Start(HConnection);
        }
        /// <summary>
        /// 启动连接（重载二：HConnection介入）
        /// </summary>
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回true，错误则返回false</returns>
        public bool Start(MySqlConn MySqlConn)
        {
            //组建连接信息并创建连接
            HConnection = new MySqlConnection
                (
                "Data source=" + MySqlConn.DataSource + ";port=" +
                MySqlConn.Port + ";User Id=" + MySqlConn.User + ";password=" + MySqlConn.PWD + ";"
                );
            return Start(HConnection);
        }
        /// <summary>
        /// 启动连接（重载三）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <returns>返回true，错误则返回false</returns>
        public bool Start(MySqlConnection MySqlConnection)
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
        /// <param name="SQL">SQL语句</param>
        /// <returns>返回查询结果</returns>
        public DataRow GetRow(MySqlConnection MySqlConnection, string SQL)
        {
            try
            {
                if (MySqlConnection.State == ConnectionState.Closed)//判断连接是否被关闭
                {
                    MySqlConnection.Open();//连接关闭则打开
                }
                using (MySqlDataAdapter myDA = new MySqlDataAdapter(SQL, MySqlConnection))
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
        public DataRow GetRow(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
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
        public DataRow GetRow(string sql)
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
        public DataRow GetRow(MySqlCommand MySqlCommand)
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
        /// <param name="KeyName">键名</param>
        /// <param name="KeyValue">键值</param>
        /// <returns>返回获得的DataRow数据行实例，未检索到返回null</returns>
        public DataRow GetRow(DataTable DataTable, string KeyName, object KeyValue)
        {
            foreach (DataRow DataRow in DataTable.Rows)
            {
                if (//全部转为string来判断是否相等，因为object箱结构不一样
                    DataRow[KeyName].ToString() == KeyValue.ToString()
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
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection GetConnection(string DataSource, string Port, string User, string PWD)
        {
            try
            {
                //返回创建的连接
                return new MySqlConnection
                    (//组建连接信息
                    "Data source=" + DataSource + ";port="
                    + Port + ";User Id=" + User + ";password=" + PWD + ";"
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
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection GetConnection(MySqlConn MySqlConn)
        {
            try
            {
                //返回创建的连接
                return new MySqlConnection
                    (//组建连接信息
                    "Data source=" + MySqlConn.DataSource + ";port=" +
                    MySqlConn.Port + ";User Id=" + MySqlConn.User + ";password=" + MySqlConn.PWD + ";"
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
        /// <param name="SQL">用于查询数据表的SQL语句</param>
        /// <returns>返回一个DataTable对象，错误则返回null</returns>
        public DataTable GetTable(MySqlConnection MySqlConnection, string SQL)
        {
            //新建数据适配器
            MySqlDataAdapter myDA = new MySqlDataAdapter(SQL, MySqlConnection);
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
        public DataTable GetTable(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
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
        public DataTable GetTable(string sql)
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
        public DataTable GetTable(MySqlCommand MySqlCommand)
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
        /// <param name="ColumnName">列名</param>
        /// <returns>返回非泛型List{object}实例，错误则返回null</returns>
        public List<object> GetColumn(DataTable DataTable, string ColumnName)
        {
            try
            {
                List<object> list = new List<object>();
                foreach (DataRow DataRow in DataTable.Rows)
                {
                    list.Add(DataRow[ColumnName]);//将数据表中ColumnName列的所有行数据依次添加到list中
                }
                return list;//返回列
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 设置(替换)一个键值（键匹配查询）
        /// </summary>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">操作于键名</param>
        /// <param name="KeyValue">更改为该键值</param>
        /// <returns>操作成功返回true</returns>
        public bool SetColumnValue(MySqlConnection MySqlConnection, MySqlKey MySqlKey, string KeyName, string KeyValue)
        {
            #region SQL字符串处理
            string sql = "UPDATE " +
                         MySqlKey.DataBase + "." + MySqlKey.Table +
                         " SET " +
                         KeyName + "='" + KeyValue +
                         "' WHERE " +
                         MySqlKey.Name + "='" + MySqlKey.Val +
                         "';";
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
        /// 设置(替换)一个键值（键匹配查询）
        /// （重载二：HConnection介入）
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">操作于键名</param>
        /// <param name="KeyValue">更改为该键值</param>
        /// <returns>操作成功返回true</returns>
        public bool SetColumnValue(MySqlKey MySqlKey, string KeyName, string KeyValue)
        {
            #region SQL字符串处理
            string sql = "UPDATE " +
                         MySqlKey.DataBase + "." + MySqlKey.Table +
                         " SET " +
                         KeyName + "='" + KeyValue +
                         "' WHERE " +
                         MySqlKey.Name + "='" + MySqlKey.Val +
                         "';";
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
        /// 获得一个键值（键匹配查询）
        ///（重载一）
        /// 如果查询到多个行，则只返回第一行的数据
        /// </summary>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>操作成功返回true</returns>
        public object GetColumnValue(MySqlConnection MySqlConnection, MySqlKey MySqlKey, string KeyName)
        {
            #region SQL字符串处理
            string sql = "SELECT " +
                         KeyName +
                         " FROM " +
                         MySqlKey.DataBase + "." + MySqlKey.Table +
                         " WHERE " +
                         MySqlKey.Name + "='" + MySqlKey.Val +
                         "';";
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
        /// 获得一个键值（键匹配查询）
        /// （重载二：HConnection介入）
        /// 如果查询到多个行，则只返回第一行的数据
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>操作成功返回true</returns>
        public object GetColumnValue(MySqlKey MySqlKey, string KeyName)
        {
            #region SQL字符串处理
            string sql = "SELECT " +
                         KeyName +
                         " FROM " +
                         MySqlKey.DataBase + "." + MySqlKey.Table +
                         " WHERE " +
                         MySqlKey.Name + "='" + MySqlKey.Val +
                         "';";
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
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="ParmList">查询参数列表</param>
        /// <returns>返回建立的参数化查询cmd对象</returns>
        public MySqlCommand ParmQueryCMD(string SQL, List<MySqlParm> ParmList)
        {
            //建立cmd对象，用于执行参数化查询
            using (MySqlCommand MySqlCommand = new MySqlCommand(SQL))
            {
                foreach (MySqlParm Parm in ParmList)
                {
                    MySqlCommand.Parameters.AddWithValue(Parm.Name, Parm.Val);//添加参数
                }
                return MySqlCommand;
            }
        }
    }
}
