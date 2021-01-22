using System;
using System.Collections.Generic;

using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;


namespace WaterLibrary.MySQL
{
    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class MySqlManager
    {
        private string ConnectionString { get; init; }
        private List<MySqlConnection> ConnectionPool { get; init; }
        /// <summary>
        /// 数据库连接访问器
        /// </summary>
        public MySqlConnection Connection
        {
            get
            {
                if (ConnectionPool.Count > 32)/* 在连接数超出时检查无用连接并进行清理 */
                {
                    for (int i = ConnectionPool.Count - 1; i >= 0; i--)
                    { /* 如果连接中断或是关闭（这都是不工作的状态） */
                        if (ConnectionPool[i].State is ConnectionState.Broken or ConnectionState.Closed)
                        {
                            ConnectionPool[i].Dispose();/* 注销并移除连接池 */
                            ConnectionPool.RemoveAt(i);
                        }
                    };
                }

                ConnectionPool.Add(new(ConnectionString));
                ConnectionPool.Last().Open();
                return ConnectionPool.Last();
            }
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        private MySqlManager() { }
        /// <summary>
        /// 连接信息构造
        /// </summary>
        /// <param name="MySqlConnMsg">MySQL数据库连接信息</param>
        public MySqlManager(MySqlConnMsg MySqlConnMsg)
        {
            ConnectionPool = new();
            ConnectionString =
            $@";DataSource={MySqlConnMsg.DataSource}
               ;Port={MySqlConnMsg.Port }
               ;UserID={MySqlConnMsg.User}
               ;Password={MySqlConnMsg.PWD}
               ;UseAffectedRows=TRUE;";/* UPDATE语句返回受影响的行数而不是符合查询条件的行数 */
        }
        /// <summary>
        /// 带有目标数据库的连接信息构造
        /// </summary>
        /// <param name="MySqlConnMsg">MySQL数据库连接信息</param>
        /// <param name="Database">目标数据库</param>
        public MySqlManager(MySqlConnMsg MySqlConnMsg, string Database)
        {
            ConnectionPool = new();
            ConnectionString = /* USING目标数据库 */
            $@";DataSource={MySqlConnMsg.DataSource}
               ;DataBase={Database}
               ;Port={MySqlConnMsg.Port }
               ;UserID={MySqlConnMsg.User}
               ;Password={MySqlConnMsg.PWD}
               ;UseAffectedRows=TRUE;";
        }

        /// <summary>
        /// 一次性连接使用器
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="todo">委托</param>
        /// <returns></returns>
        public T DoInConnection<T>(Func<MySqlConnection, T> todo)
        {
            MySqlConnection conn = Connection;
            T result = todo(conn);
            conn.Close();
            return result;
        }

        /// <summary>
        /// 获取单张数据表
        /// </summary>
        /// <param name="SQL">SQL语句，用于查询数据表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL)
        {
            return DoInConnection(conn =>
            {
                using DataTable table = new DataTable();
                new MySqlDataAdapter(SQL, conn).Fill(table);

                return table;
            });
        }
        /// <summary>
        /// 获取单张数据表（适用于参数化查询）
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL, params MySqlParameter[] parameters)
        {
            return DoInConnection(conn =>
            {
                using DataTable table = new DataTable();
                using (MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn))
                {
                    MySqlCommand.Parameters.AddRange(parameters);//添加参数
                    new MySqlDataAdapter(MySqlCommand).Fill(table);
                }

                return table;
            });
        }

        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(string SQL)
        {
            return DoInConnection(conn =>
            {
                /* 如果结果集为空，该方法返回null */
                return new MySqlCommand(SQL, conn).ExecuteScalar(); ;
            });
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(string SQL, params MySqlParameter[] parameters)
        {
            return DoInConnection(conn =>
            {
                using MySqlCommand MySqlCommand = new MySqlCommand(SQL, conn);
                MySqlCommand.Parameters.AddRange(parameters);

                /* 如果结果集为空，该方法返回null */
                return MySqlCommand.ExecuteScalar(); ;
            });
        }
        /// <summary>
        /// 取得指定键值（键匹配查询）
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey((string Table, string Name, object Val) MySqlKey, string KeyName)
        {
            return DoInConnection(conn =>
            {
                string SQL = $"SELECT {KeyName} FROM {MySqlKey.Table} WHERE {MySqlKey.Name}='{MySqlKey.Val}';";
                /* 如果结果集为空，该方法返回null */
                return new MySqlCommand(SQL, conn).ExecuteScalar(); ;
            });
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
        public DataRow GetRow(string SQL, params MySqlParameter[] parameters)
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
        public List<T> GetColumn<T>(string SQL, params MySqlParameter[] parameters)
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
        public List<T> GetColumn<T>(string SQL, string Key, params MySqlParameter[] parameters)
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
        public bool UpdateKey((string Table, string Name, object Val) MySqlKey, string Key, object NewValue)
        {
            return DoInConnection(conn =>
            {
                using MySqlCommand MySqlCommand = new()
                {
                    CommandText = $"UPDATE {MySqlKey.Table} SET {Key}=?NewValue WHERE {MySqlKey.Name}=?Val",
                    Connection = conn,
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
            });
        }
        /// <summary>
        /// 更新单个键值
        /// </summary>
        /// <param name="Table">目标表</param>
        /// <param name="Key">键名</param>
        /// <param name="OldValue">旧值</param>
        /// <param name="NewValue">新值</param>
        /// <returns></returns>
        public bool UpdateKey(string Table, string Key, object OldValue, object NewValue)
        {
            return DoInConnection(conn =>
            {
                using MySqlCommand MySqlCommand = new()
                {
                    CommandText = $"UPDATE {Table} SET {Key}=?NewValue WHERE {Key}=?OldValue",
                    Connection = conn,
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
            });
        }
    }

    /// <summary>
    /// MySql数据库连接信息
    /// 数据源
    /// 数据库
    /// 端口
    /// 用户名
    /// 密码
    /// </summary>
    public record MySqlConnMsg(string DataSource, int Port, string User, string PWD);
}
