using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterLibrary.stru.MySQL
{
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
    public struct MySqlConn
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
    /// <summary>
    /// 参数（用于参数化查询添加参数）
    /// </summary>
    public struct MySqlParm
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Val { get; set; }
    }
}
