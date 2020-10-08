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
    public class MySqlManager
    {
        private MySqlConnection HandlerConnection;

        /// <summary>
        /// Close主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool CloseHandlerConnection()
        {
            try
            {
                switch (HandlerConnection.State)
                {
                    case ConnectionState.Open:
                        HandlerConnection.Close();
                        if (HandlerConnection.State == ConnectionState.Closed)
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
        public bool DisposeHandlerConnection()
        {
            try
            {
                switch (HandlerConnection.State)
                {
                    case ConnectionState.Open:
                        HandlerConnection.Dispose();
                        if (HandlerConnection.State == ConnectionState.Closed)
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
        public bool NullHandlerConnection()
        {
            try
            {
                HandlerConnection = null;
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
        public bool ReStartHandlerConnection()
        {
            try
            {
                //考虑到重启连接需花费一定时间的假设，该方法只提供了重启操作，而不去判断是否执行成功。
                switch (HandlerConnection.State)
                {
                    case ConnectionState.Closed:
                        HandlerConnection.Open();
                        return true;

                    case ConnectionState.Open:
                        HandlerConnection.Close();
                        HandlerConnection.Open();

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
        /// 生成MySqlConnection（重载一）
        /// </summary>
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection GetSqlConnection(MySqlConn MySqlConn)
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
        /// 生成MySqlConnection（重载二）
        /// </summary>
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection GetSqlConnection(string DataSource, string Port, string User, string PWD)
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
        /// 启动连接（重载一：启动HandlerConnection）
        /// </summary>
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回true，错误则返回null</returns>
        public bool Start(string DataSource, string Port, string User, string PWD)
        {
            //组建连接信息并创建连接
            HandlerConnection = new MySqlConnection
                (
                "Data source=" + DataSource + ";port="
                + Port + ";User Id=" + User + ";password=" + PWD + ";"
                );
            return Start(HandlerConnection);
        }
        /// <summary>
        /// 启动连接（重载二：启动HandlerConnection）
        /// </summary>
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回true，错误则返回false</returns>
        public bool Start(MySqlConn MySqlConn)
        {
            //组建连接信息并创建连接
            HandlerConnection = new MySqlConnection
                (
                "Data source=" + MySqlConn.DataSource + ";port=" +
                MySqlConn.Port + ";User Id=" + MySqlConn.User + ";password=" + MySqlConn.PWD + ";"
                );
            return Start(HandlerConnection);
        }
        /// <summary>
        /// 启动连接（重载三：启动外来MySqlConnection）
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
        /// 建立参数化查询CMD对象
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="ParmList">查询参数列表</param>
        /// <returns>返回建立的参数化查询CMD对象</returns>
        public MySqlCommand ParmQueryCMD(string SQL, List<MySqlParm> ParmList)
        {
            //建立CMD对象，用于执行参数化查询
            using (MySqlCommand MySqlCommand = new MySqlCommand(SQL))
            {
                foreach (MySqlParm Parm in ParmList)
                {
                    MySqlCommand.Parameters.AddWithValue(Parm.Name, Parm.Val);//添加参数
                }
                return MySqlCommand;
            }
        }


        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// （重载一：使用HandlerConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">SQL语句</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public T GetKey<T>(string SQL)
        {
            try
            {
                /* 如果结果集为空，该方法返回null */
                return (T)new MySqlCommand(SQL, HandlerConnection).ExecuteScalar();
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// （重载二：使用HandlerConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public T GetKey<T>(MySqlCommand MySqlCommand)
        {
            try
            {
                //将外来CMD设置为基于HandlerConnection执行
                MySqlCommand.Connection = HandlerConnection;

                /* 如果结果集为空，该方法返回null */
                return (T)MySqlCommand.ExecuteScalar();
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// （重载三：使用HandlerConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public T GetKey<T>(MySqlKey MySqlKey, string KeyName)
        {
            try
            {
                string SQL = string.Format("SELECT {0} FROM {1}.{2} WHERE {3}='{4}';",
                    KeyName, MySqlKey.DataBase, MySqlKey.Table, MySqlKey.Name, MySqlKey.Val);

                /* 如果结果集为空，该方法返回null */
                return (T)new MySqlCommand(SQL, HandlerConnection).ExecuteScalar();
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        ///（重载四：使用外部MySqlConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public T GetKey<T>(MySqlConnection MySqlConnection, MySqlKey MySqlKey, string KeyName)
        {
            try
            {
                string SQL = string.Format("SELECT {0} FROM {1}.{2} WHERE {3}='{4}';",
                    KeyName, MySqlKey.DataBase, MySqlKey.Table, MySqlKey.Name, MySqlKey.Val);

                /* 如果结果集为空，该方法返回null */
                return (T)new MySqlCommand(SQL, MySqlConnection).ExecuteScalar();
            }
            catch
            {
                return default;
            }
        }


        /// <summary>
        /// 获得数据行（重载一：使用HandlerConnection查询）
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(string SQL)
        {
            try
            {
                return GetTable(SQL).Rows[0];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得数据行（重载二：使用HandlerConnection查询）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(MySqlCommand MySqlCommand)
        {
            try
            {
                //将外来CMD设置为基于HandlerConnection执行
                MySqlCommand.Connection = HandlerConnection;

                return GetTable(MySqlCommand).Rows[0];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得数据行（重载三：使用外部MySqlConnection查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="SQL">SQL语句</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(MySqlConnection MySqlConnection, string SQL)
        {
            try
            {
                return GetTable(MySqlConnection, SQL).Rows[0];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得数据行（重载四：使用外部MySqlConnection查询）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
        {
            try
            {
                return GetTable(MySqlConnection, MySqlCommand).Rows[0];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 从DataTable中提取指定行
        /// </summary>
        /// <param name="DataTable">数据表实例</param>
        /// <param name="KeyName">键名</param>
        /// <param name="KeyValue">键值</param>
        /// <returns>返回获得的DataRow数据行实例，表为空或未检索到返回null</returns>
        public DataRow GetRow(DataTable DataTable, string KeyName, object KeyValue)
        {
            foreach (DataRow DataRow in DataTable.Rows)
            {
                /* 全部转为string来判断是否相等，因为object箱结构不一样 */
                if (DataRow[KeyName].ToString() == KeyValue.ToString())
                {
                    return DataRow;/* 返回符合被检索主键的行 */
                }
            }
            return null;/* 未检索到 */
        }


        /// <summary>
        /// 获取一张数据表（重载一：使用HandlerConnection查询）
        /// </summary>
        /// <param name="SQL">SQL语句，用于查询数据表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL)
        {
            try
            {
                DataTable table = new DataTable();

                /* 新建MySqlDataAdapter后填表 */
                new MySqlDataAdapter(SQL, HandlerConnection).Fill(table);

                return table;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取一张数据表（重载二：使用HandlerConnection查询）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(MySqlCommand MySqlCommand)
        {
            try
            {
                //将外来CMD设置为基于HandlerConnection执行
                MySqlCommand.Connection = HandlerConnection;

                DataTable table = new DataTable();

                /* 新建MySqlDataAdapter后填表 */
                new MySqlDataAdapter(MySqlCommand).Fill(table);

                return table;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取一张数据表（重载三：使用外部MySqlConnection查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="SQL">用于查询数据表的SQL语句</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(MySqlConnection MySqlConnection, string SQL)
        {
            try
            {
                DataTable table = new DataTable();

                /* 新建MySqlDataAdapter后填表 */
                new MySqlDataAdapter(SQL, MySqlConnection).Fill(table);

                return table;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取一张数据表（重载四：使用外部MySqlConnection查询）（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlConnection">MySqlConnection连接实例</param>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(MySqlConnection MySqlConnection, MySqlCommand MySqlCommand)
        {
            try
            {
                //将外来CMD设置为基于MySqlConnection执行
                MySqlCommand.Connection = MySqlConnection;

                DataTable table = new DataTable();

                /* 新建MySqlDataAdapter后填表 */
                new MySqlDataAdapter(MySqlCommand).Fill(table);

                return table;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 取得查询结果中的第一列（重载一：使用HandlerConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">用于查询的SQL语句</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(string SQL)
        {
            return GetColumn<T>(GetTable(SQL));
        }
        /// <summary>
        /// 取得查询结果中的指定列（重载二：使用HandlerConnection查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">用于查询的SQL语句</param>
        /// <param name="Key">目标列键名</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(string SQL, string Key)
        {
            return GetColumn<T>(GetTable(SQL), Key);
        }
        /// <summary>
        /// 取得查询结果中的第一列（重载三：使用HandlerConnection查询）（适用于参数化查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlCommand">CMD实例</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(MySqlCommand MySqlCommand)
        {
            return GetColumn<T>(GetTable(MySqlCommand));
        }
        /// <summary>
        /// 取得查询结果中的指定列（重载四：使用HandlerConnection查询）（适用于参数化查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlCommand">CMD实例</param>
        /// <param name="Key">目标列键名</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(MySqlCommand MySqlCommand, string Key)
        {
            return GetColumn<T>(GetTable(MySqlCommand), Key);
        }
        /// <summary>
        /// 从DataTable中提取第一列（此方法无空值判断）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="DataTable">数据表实例</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(DataTable DataTable)
        {
            List<T> List = new List<T>();

            foreach (DataRow DataRow in DataTable.Rows)
            {
                List.Add((T)Convert.ChangeType(DataRow[0], typeof(T)));
            }
            return List;
        }
        /// <summary>
        /// 从DataTable中提取指定列（此方法无空值判断）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="DataTable">数据表实例</param>
        /// <param name="Key">目标列键名</param>
        /// <returns>返回非泛型List{object}实例</returns>
        public List<T> GetColumn<T>(DataTable DataTable, string Key)
        {
            List<T> List = new List<T>();

            foreach (DataRow DataRow in DataTable.Rows)
            {
                List.Add((T)Convert.ChangeType(DataRow[Key], typeof(T)));
            }
            return List;
        }

        /// <summary>
        /// 设置(更新)一个键值（键匹配查询）
        /// （重载一：使用HandlerConnection查询）
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">操作于键名</param>
        /// <param name="KeyValue">更改为该键值</param>
        /// <returns></returns>
        public bool UpdateKey(MySqlKey MySqlKey, string KeyName, string KeyValue)
        {
            string SQL = string.Format("UPDATE {0}.{1} SET {2} = ?KeyValue WHERE {3} = ?Val"
                , MySqlKey.DataBase, MySqlKey.Table, KeyName, MySqlKey.Name);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "KeyValue", Val = KeyValue },
                    new MySqlParm() { Name = "Val", Val = MySqlKey.Val }
                };

            using (MySqlCommand MySqlCommand = ParmQueryCMD(SQL, ParmList))
            {
                MySqlCommand.Connection = HandlerConnection;/* 使用控制器内部连接执行查询 */
                ushort result = (ushort)MySqlCommand.ExecuteNonQuery();/* 性能优化尝试 */

                if (result == 1)
                {
                    return true;/* 只有出现只有一行被更改时，才能返回true */
                }
                else if (result > 1)
                {
                    throw new Exception("操作了多个数据行");
                }
                else if (result == 0)
                {
                    throw new Exception("语句未作用于任何数据行");
                }
                return false;
            }
        }
        /// <summary>
        /// 设置(更新)一个键值（键匹配查询）
        /// （重载二：使用外部MySqlConnection查询）
        /// </summary>
        /// <param name="MySqlConnection">数据库连接实例，用于承担该操作</param>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">操作于键名</param>
        /// <param name="KeyValue">更改为该键值</param>
        /// <returns></returns>
        public bool UpdateKey(MySqlConnection MySqlConnection, MySqlKey MySqlKey, string KeyName, string KeyValue)
        {
            string SQL = string.Format("UPDATE {0}.{1} SET {2} = ?KeyValue WHERE {3} = ?Val"
                , MySqlKey.DataBase, MySqlKey.Table, KeyName, MySqlKey.Name);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "KeyValue", Val = KeyValue },
                    new MySqlParm() { Name = "Val", Val = MySqlKey.Val }
                };

            using (MySqlCommand MySqlCommand = ParmQueryCMD(SQL, ParmList))
            {
                MySqlCommand.Connection = MySqlConnection;/* 使用外来连接执行查询 */
                ushort result = (ushort)MySqlCommand.ExecuteNonQuery();/* 性能优化尝试 */

                if (result == 1)
                {
                    return true;/* 只有出现只有一行被更改时，才能返回true */
                }
                else if (result > 1)
                {
                    throw new Exception("操作了多个数据行");
                }
                else if (result == 0)
                {
                    throw new Exception("语句未作用于任何数据行");
                }
                return false;
            }
        }
    }
}