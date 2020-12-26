using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;


namespace WaterLibrary.MySQL
{
    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class MySqlManager
    {
        private MySqlConnMsg MySqlConnMsg { get; set; }
        private List<MySqlConnection> ConnectionPool = new();
        /// <summary>
        /// 数据库连接访问器
        /// </summary>
        public MySqlConnection Connection
        {
            get
            {
                var el = new MySqlConnection(
                    "DataSource=" + MySqlConnMsg.DataSource +
                    ";DataBase=" + MySqlConnMsg.DataBase +
                    ";Port=" + MySqlConnMsg.Port +
                    ";UserID=" + MySqlConnMsg.User +
                    ";Password=" + MySqlConnMsg.PWD +
                    /* UPDATE语句返回受影响的行数而不是符合查询条件的行数|兼容旧版语法 */
                    ";UseAffectedRows=TRUE;"
                    );
                el.Open();
                

                if (ConnectionPool.Count >= 8)/* 在连接数达到8时检查无用连接并进行清理 */
                {
                    for (int i = ConnectionPool.Count - 1; i >= 0; i--)
                    { /* 如果连接打开、中断或是关闭（这都是不工作的状态） */
                        if (ConnectionPool[i].State == ConnectionState.Open
                         || ConnectionPool[i].State == ConnectionState.Broken
                         || ConnectionPool[i].State == ConnectionState.Closed)
                        {
                            ConnectionPool[i].Dispose();
                            ConnectionPool.Remove(ConnectionPool[i]);
                        }
                    };
                }

                ConnectionPool.Add(el);
                return ConnectionPool[0];
            }
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        /// <param name="MySqlConnMsg"></param>
        public MySqlManager(MySqlConnMsg MySqlConnMsg)
        {
            this.MySqlConnMsg = MySqlConnMsg;
        }

        /// <summary>
        /// 生成MySqlConnection（重载一）
        /// </summary>
        /// <param name="MySqlConnMsg">连接签名</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public static MySqlConnection GetSqlConnection(MySqlConnMsg MySqlConnMsg)
        {
            //返回创建的连接
            return new MySqlConnection
                (//组建连接信息
                "Data source=" + MySqlConnMsg.DataSource + ";port=" +
                MySqlConnMsg.Port + ";User Id=" + MySqlConnMsg.User + ";password=" + MySqlConnMsg.PWD + ";"
                );
        }
        /// <summary>
        /// 生成MySqlConnection（重载二）
        /// </summary>
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public static MySqlConnection GetSqlConnection(string DataSource, string Port, string User, string PWD)
        {
            //返回创建的连接
            return new MySqlConnection
                (//组建连接信息
                "Data source=" + DataSource + ";port="
                + Port + ";User Id=" + User + ";password=" + PWD + ";"
                );
        }


        /// <summary>
        /// 获取单张数据表
        /// </summary>
        /// <param name="SQL">SQL语句，用于查询数据表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL)
        {
            using DataTable table = new DataTable();

            new MySqlDataAdapter(SQL, Connection).Fill(table);

            return table;
        }
        /// <summary>
        /// 获取单张数据表（适用于参数化查询）
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL, MySqlParameter[] parameters)
        {
            using MySqlCommand MySqlCommand = new MySqlCommand(SQL, Connection);
            MySqlCommand.Parameters.AddRange(parameters);//添加参数

            using DataTable table = new DataTable();

            new MySqlDataAdapter(MySqlCommand).Fill(table);

            return table;
        }

        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(string SQL)
        {
            /* 如果结果集为空，该方法返回null */
            return new MySqlCommand(SQL, Connection).ExecuteScalar();
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(string SQL, MySqlParameter[] parameters)
        {
            using MySqlCommand MySqlCommand = new MySqlCommand(SQL, Connection);
            MySqlCommand.Parameters.AddRange(parameters);

            /* 如果结果集为空，该方法返回null */
            return MySqlCommand.ExecuteScalar();
        }
        /// <summary>
        /// 取得指定键值（键匹配查询）
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(MySqlKey MySqlKey, string KeyName)
        {
            string SQL = $"SELECT {KeyName} FROM {MySqlKey.Table} WHERE {MySqlKey.Name}='{MySqlKey.Val}';";

            /* 如果结果集为空，该方法返回null */
            return new MySqlCommand(SQL, Connection).ExecuteScalar();
        }


        /// <summary>
        /// 获得数据行
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(string SQL)
        {
            /* 数组越界防止 */
            DataRowCollection collection = GetTable(SQL).Rows;
            return collection.Count == 0 ? null : collection[0];
        }
        /// <summary>
        /// 获得数据行（适用于参数化查询）
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(string SQL, MySqlParameter[] parameters)
        {
            DataRowCollection collection = GetTable(SQL, parameters).Rows;
            return collection.Count == 0 ? null : collection[0];
        }
        /// <summary>
        /// 从DataTable中提取指定行
        /// </summary>
        /// <param name="DataTable">数据表实例</param>
        /// <param name="KeyName">键名</param>
        /// <param name="KeyValue">键值</param>
        /// <returns>返回获得的DataRow数据行实例，表为空或未检索到返回null</returns>
        public static DataRow GetRow(DataTable DataTable, string KeyName, object KeyValue)
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
        /// 取得查询结果中的第一列
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">用于查询的SQL语句</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(string SQL)
        {
            return GetColumn<T>(GetTable(SQL));
        }
        /// <summary>
        /// 取得查询结果中的指定列
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
        /// 取得查询结果中的第一列
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(string SQL, MySqlParameter[] parameters)
        {
            return GetColumn<T>(GetTable(SQL, parameters));
        }
        /// <summary>
        /// 取得查询结果中的指定列
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <param name="Key">目标列键名</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(string SQL, MySqlParameter[] parameters, string Key)
        {
            return GetColumn<T>(GetTable(SQL, parameters), Key);
        }
        /// <summary>
        /// 从DataTable中提取第一列（此方法无空值判断）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="DataTable">数据表实例</param>
        /// <returns></returns>
        public static List<T> GetColumn<T>(DataTable DataTable)
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
        public static List<T> GetColumn<T>(DataTable DataTable, string Key)
        {
            List<T> List = new List<T>();

            foreach (DataRow DataRow in DataTable.Rows)
            {
                List.Add((T)Convert.ChangeType(DataRow[Key], typeof(T)));
            }
            return List;
        }

        /// <summary>
        /// 更新单个键值
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="Key">要更改的键</param>
        /// <param name="NewValue">新键值</param>
        /// <returns></returns>
        public bool UpdateKey(MySqlKey MySqlKey, string Key, string NewValue)
        {
            using MySqlCommand MySqlCommand = new()
            {
                CommandText = $"UPDATE {MySqlKey.Table} SET {Key}=?NewValue WHERE {MySqlKey.Name}=?Val",
                Connection = Connection,
                Transaction = Connection.BeginTransaction()
            };
            MySqlCommand.Parameters.AddWithValue("NewValue", NewValue);
            MySqlCommand.Parameters.AddWithValue("Val", MySqlKey.Val);

            if (MySqlCommand.ExecuteNonQuery() == 1)
            {
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                MySqlCommand.Transaction.Rollback();
                return false;
            }
        }
        /// <summary>
        /// 更新单个键值
        /// </summary>
        /// <param name="Table">目标表</param>
        /// <param name="Key">键名</param>
        /// <param name="OldValue">旧值</param>
        /// <param name="NewValue">新值</param>
        /// <returns></returns>
        public bool UpdateKey(string Table, string Key, string OldValue, string NewValue)
        {
            using MySqlCommand MySqlCommand = new()
            {
                CommandText = $"UPDATE {Table} SET {Key}=?NewValue WHERE {Key}=?OldValue",
                Connection = Connection,
                Transaction = Connection.BeginTransaction()
            };
            MySqlCommand.Parameters.AddWithValue("NewValue", NewValue);
            MySqlCommand.Parameters.AddWithValue("OldValue", OldValue);

            if (MySqlCommand.ExecuteNonQuery() == 1)
            {
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                MySqlCommand.Transaction.Rollback();
                return false;
            }
        }


        /* 实验性内容，不建议使用 
        /// <summary>
        /// 单纯执行SQL查询
        /// </summary>
        /// <param name="SQL">用于查询的SQL语句</param>
        /// <returns>返回受影响的行数</returns>
        public int QueryOnly(string SQL)
        {
            return new MySqlCommand(SQL, Connection).ExecuteNonQuery();
        }
        */
    }

    /// <summary>
    /// 键（用于键匹配查询）
    /// </summary>
    public struct MySqlKey
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 主键名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主键值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// MySql连接信息
    /// </summary>
    public struct MySqlConnMsg
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 用户名对应的密码
        /// </summary>
        public string PWD { get; set; }
    }
}
