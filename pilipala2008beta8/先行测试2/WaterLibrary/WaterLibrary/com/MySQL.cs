using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

using WaterLibrary.stru.MySQL;

namespace WaterLibrary.com.MySQL
{
    /// <summary>
    /// MySql数据库管理器
    /// </summary>
    public class MySqlManager
    {
        /* 注意！所有的查询均不会使用参数化 */
        private MySqlConnection HandlerConnection;

        /// <summary>
        /// MySql控制器的数据库连接
        /// </summary>
        public MySqlConnection Connection
        {
            get { return HandlerConnection; }
        }

        /// <summary>
        /// Close主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool CloseHandlerConnection()
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
        /// <summary>
        /// Dispose主连接
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool DisposeHandlerConnection()
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
        /// <summary>
        /// 将主连接设置为null值
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool NullHandlerConnection()
        {
            HandlerConnection = null;
            return true;
        }
        /// <summary>
        /// 重启主连接（须以主连接定义完成为前提）
        /// </summary>
        /// <returns>成功返回ture，反之或报错返回false</returns>
        public bool ReStartHandlerConnection()
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

        /// <summary>
        /// 生成MySqlConnection（重载一）
        /// </summary>
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回一个MySqlConnection对象，错误则返回null</returns>
        public MySqlConnection GetSqlConnection(MySqlConn MySqlConn)
        {
            //返回创建的连接
            return new MySqlConnection
                (//组建连接信息
                "Data source=" + MySqlConn.DataSource + ";port=" +
                MySqlConn.Port + ";User Id=" + MySqlConn.User + ";password=" + MySqlConn.PWD + ";"
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
        public MySqlConnection GetSqlConnection(string DataSource, string Port, string User, string PWD)
        {
            //返回创建的连接
            return new MySqlConnection
                (//组建连接信息
                "Data source=" + DataSource + ";port="
                + Port + ";User Id=" + User + ";password=" + PWD + ";"
                );
        }

        /// <summary>
        /// 启动连接
        /// </summary>
        /// <param name="DataSource">数据源</param>
        /// <param name="DataBase">数据库</param>
        /// <param name="Port">端口</param>
        /// <param name="User">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回true，错误则返回null</returns>
        public void Start(string DataSource, string DataBase, string Port, string User, string PWD)
        {
            /* 拼接连接字符串 */
            HandlerConnection = new MySqlConnection
                (
                "DataSource=" + DataSource +
                ";DataBase=" + DataBase +
                ";Port=" + Port +
                ";UserID=" + User +
                ";Password=" + PWD +
                /* 设置UPDATE语句返回受影响的行数而不是符合查询条件的行数 */
                ";UseAffectedRows=TRUE;"
                );
            /* 打开连接 */
            HandlerConnection.Open();
        }
        /// <summary>
        /// 启动连接
        /// </summary>
        /// <param name="MySqlConn">连接签名</param>
        /// <returns>返回true，错误则返回false</returns>
        public void Start(MySqlConn MySqlConn)
        {
            Start(
                MySqlConn.DataSource,
                MySqlConn.DataBase,
                MySqlConn.Port,
                MySqlConn.User,
                MySqlConn.PWD
                );
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
        /// 建立参数化查询CMD对象
        /// </summary>
        /// <param name="SQL">携带查询参数的SQL语句</param>
        /// <param name="Parm">查询参数</param>
        /// <returns>返回建立的参数化查询CMD对象</returns>
        public MySqlCommand ParmQueryCMD(string SQL, params MySqlParm[] Parm)
        {
            //建立CMD对象，用于执行参数化查询
            MySqlCommand MySqlCommand = new MySqlCommand(SQL);

            foreach (MySqlParm p in Parm)
            {
                MySqlCommand.Parameters.AddWithValue(p.Name, p.Val);//添加参数
            }
            return MySqlCommand;
        }


        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(string SQL)
        {
            /* 如果结果集为空，该方法返回null */
            return new MySqlCommand(SQL, HandlerConnection).ExecuteScalar();
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(MySqlCommand MySqlCommand)
        {
            //将外来CMD设置为基于HandlerConnection执行
            MySqlCommand.Connection = HandlerConnection;

            /* 如果结果集为空，该方法返回null */
            return MySqlCommand.ExecuteScalar();
        }
        /// <summary>
        /// 取得首个键值（键匹配查询）
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">键名</param>
        /// <returns>返回结果集中的第一行第一列，若查询无果或异常则返回null</returns>
        public object GetKey(MySqlKey MySqlKey, string KeyName)
        {
            string SQL = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}';",
                KeyName, MySqlKey.Table, MySqlKey.Name, MySqlKey.Val);

            /* 如果结果集为空，该方法返回null */
            return new MySqlCommand(SQL, HandlerConnection).ExecuteScalar();
        }


        /// <summary>
        /// 获得数据行
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(string SQL)
        {
            return GetTable(SQL).Rows[0];
        }
        /// <summary>
        /// 获得数据行（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>操作异常或目标行不存在时，返回null</returns>
        public DataRow GetRow(MySqlCommand MySqlCommand)
        {
            //将外来CMD设置为基于HandlerConnection执行
            MySqlCommand.Connection = HandlerConnection;

            return GetTable(MySqlCommand).Rows[0];
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
        /// 获取一张数据表
        /// </summary>
        /// <param name="SQL">SQL语句，用于查询数据表</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(string SQL)
        {
            DataTable table = new DataTable();

            /* 新建MySqlDataAdapter后填表 */
            new MySqlDataAdapter(SQL, HandlerConnection).Fill(table);

            return table;
        }
        /// <summary>
        /// 获取一张数据表（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">MySqlCommand对象，用于进行查询</param>
        /// <returns>返回一个DataTable对象，无结果或错误则返回null</returns>
        public DataTable GetTable(MySqlCommand MySqlCommand)
        {
            //将外来CMD设置为基于HandlerConnection执行
            MySqlCommand.Connection = HandlerConnection;

            DataTable table = new DataTable();

            /* 新建MySqlDataAdapter后填表 */
            new MySqlDataAdapter(MySqlCommand).Fill(table);

            return table;
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
        /// 取得查询结果中的第一列（适用于参数化查询）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="MySqlCommand">CMD实例</param>
        /// <returns></returns>
        public List<T> GetColumn<T>(MySqlCommand MySqlCommand)
        {
            return GetColumn<T>(GetTable(MySqlCommand));
        }
        /// <summary>
        /// 取得查询结果中的指定列（适用于参数化查询）
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
        /// 设置(更新)一个键值
        /// </summary>
        /// <param name="MySqlKey">操作定位器</param>
        /// <param name="KeyName">操作于键名</param>
        /// <param name="KeyValue">更改为该键值</param>
        /// <returns></returns>
        public bool UpdateKey(MySqlKey MySqlKey, string KeyName, string KeyValue)
        {
            string SQL = string.Format("UPDATE {0} SET {1} = ?KeyValue WHERE {2} = ?Val"
                , MySqlKey.Table, KeyName, MySqlKey.Name);

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
        /// 单纯执行SQL查询
        /// </summary>
        /// <param name="SQL">用于查询的SQL语句</param>
        /// <returns>返回受影响的行数</returns>
        public int QueryOnly(string SQL)
        {
            return new MySqlCommand(SQL, HandlerConnection).ExecuteNonQuery();
        }
        /// <summary>
        /// 单纯执行SQL查询（适用于参数化查询）
        /// </summary>
        /// <param name="MySqlCommand">CMD实例</param>
        /// <returns>返回受影响的行数</returns>
        public int QueryOnly(ref MySqlCommand MySqlCommand)
        {
            MySqlCommand.Connection = HandlerConnection;
            return MySqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="MySqlCommand">执行事务的MySqlCommand</param>
        public void BeginTransaction(ref MySqlCommand MySqlCommand)
        {
            MySqlCommand.Transaction = HandlerConnection.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="MySqlCommand">执行事务的MySqlCommand</param>
        public void CommitTransaction(ref MySqlCommand MySqlCommand)
        {
            MySqlCommand.Transaction.Commit();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="MySqlCommand">执行事务的MySqlCommand</param>
        public void RollbackTransaction(ref MySqlCommand MySqlCommand)
        {
            MySqlCommand.Transaction.Rollback();
        }
    }
}