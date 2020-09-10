using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.stru.pilipala.core;

namespace WaterLibrary.stru.pilipala
{
    //116维护完成
    namespace core
    {
        /// <summary>
        /// 啪啦数据库接口
        /// </summary>
        interface IPLDB
        {
            /// <summary>
            /// Pala数据表所在数据库
            /// </summary>
            string DataBase { get; }
            /// <summary>
            /// 文本表
            /// </summary>
            PLTables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PLViews Views { get; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            MySqlConnH MySqlConnH { get; set; }
        }

        /// <summary>
        /// 索引表数据接口
        /// </summary>
        public interface ITableIndex
        {

            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            DateTime CT { get; set; }
            /// <summary>
            /// 模式
            /// </summary>
            string Mode { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            string Type { get; set; }
            /// <summary>
            /// 隶属用户
            /// </summary>
            string User { get; set; }
            /// <summary>
            /// 浏览计数
            /// </summary>
            int UVCount { get; set; }
            /// <summary>
            /// 星星计数
            /// </summary>
            int StarCount { get; set; }

        }
        /// <summary>
        /// 迭代表数据接口
        /// </summary>
        public interface ITablePrimary
        {
            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            DateTime LCT { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            string Title { get; set; }
            /// <summary>
            /// 摘要
            /// </summary>
            string Summary { get; set; }
            /// <summary>
            /// 正文
            /// </summary>
            string Content { get; set; }
            /// <summary>
            /// 归档
            /// </summary>
            string Archiv { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            string Label { get; set; }
            /// <summary>
            /// 封面
            /// </summary>
            string Cover { get; set; }
        }
        /// <summary>
        /// 用户表数据接口
        /// </summary>
        public interface ITableUser
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            string Name { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            string Note { get; set; }
        }

        /// <summary>
        /// 数据库表集合
        /// </summary>
        public struct PLTables
        {
            /// <summary>
            /// 初始化表名结构
            /// </summary>
            /// <parmm name="root"></parmm>
            /// <parmm name="IndexTable"></parmm>
            /// <parmm name="PrimaryTable"></parmm>
            /// <parmm name="text_sub"></parmm>
            public PLTables(string UserTable, string IndexTable, string PrimaryTable) : this()
            {
                this.UserTable = UserTable;
                this.IndexTable = IndexTable;
                this.PrimaryTable = PrimaryTable;
            }

            /// <summary>
            /// 
            /// </summary>
            public string UserTable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IndexTable { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PrimaryTable { get; set; }
        }
        /// <summary>
        /// 数据库视图视图集合
        /// </summary>
        public struct PLViews
        {
            /// <summary>
            /// 初始化视图名结构
            /// </summary>
            /// <param name="IndexView"></param>
            /// <param name="PrimaryView"></param>
            public PLViews(string IndexView, string PrimaryView) : this()
            {
                this.IndexView = IndexView;
                this.PrimaryView = PrimaryView;
            }
            /// <summary>
            /// 
            /// </summary>
            public string IndexView { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PrimaryView { get; set; }
        }
    }

    /// <summary>
    /// 啪啦数据读取器接口
    /// </summary>
    interface IPLDataReader : IPLDB
    {
        /// <summary>
        /// 获得全部文本ID列表
        /// </summary>
        /// <returns></returns>
        List<int> GetIDList();


        /// <summary>
        /// 取得文本ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <returns></returns>
        List<int> GetIDList(int Start, int Length);
        /// <summary>
        /// 取得指定类型的文本ID列表（步进式）
        /// </summary>
        /// <parmm name="Start">步进起始行（包含该行）</parmm>
        /// <parmm name="Length">加载行数</parmm>
        /// <parmm name="Type">自定义文本类型</parmm>
        /// <returns></returns>
        List<int> GetIDList(int Start, int Length, string Type);

        /// <summary>
        /// 通用文章匹配器
        /// </summary>
        /// <typeparmm name="T">继承自IKey的类型</typeparmm>
        /// <parmm name="OBJ">继承自IKey的对象实例</parmm>
        /// <returns></returns>
        List<int> MatchPost<T>(T obj) where T : IKey;

        /// <summary>
        /// 获得符合ID的文本索引数据
        /// </summary>
        /// <parmm name="ID">文本序列号</parmm>
        /// <returns></returns>
        Post GetIndex(int ID);
        /// <summary>
        /// 获得符合ID的文本主数据
        /// </summary>
        /// <parmm name="ID">文本序列号</parmm>
        /// <returns></returns>
        Post GetPrimary(int ID);

        /// <summary>
        /// 取得指定文本 ID 的下一个文本 ID（按照ID升序查找）
        /// </summary>
        /// <parmm name="current_ID">当前文本序列号</parmm>
        /// <returns>错误返回 -1</returns>
        int NextID(int ID);
        /// <summary>
        /// 取得指定文本 ID 的上一个文本 ID（按照ID升序查找）
        /// </summary>
        /// <parmm name="current_ID">当前文本序列号</parmm>
        /// <returns>错误返回 -1</returns>
        int PrevID(int ID);
    }
    /// <summary>
    /// 啪啦数据修改器接口
    /// </summary>
    interface IPLDataUpdater : IPLDB
    {
        /// <summary>
        /// 将目标文章状态标记为展示
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool ShowReg(int ID);
        /// <summary>
        /// 将目标文章状态标记为隐藏
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool CloseReg(int ID);
        /// <summary>
        /// 将目标文章状态标记为计划中
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool ScheduleReg(int ID);
        /// <summary>
        /// 将目标文章状态标记为已归档
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool ArchivReg(int ID);

        /// <summary>
        /// 通用文章键更改器
        /// </summary>
        /// <typeparam name="T">继承自IKey的类型</typeparam>
        /// <param name="OBJ">继承自IKey的对象实例</param>
        /// <returns></returns>
        bool UpdateKey<T>(T OBJ) where T : IKey;
    }

    /// <summary>
    /// 表键值接口
    /// </summary>
    public interface IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        string Val { get; set; }/* 为减少拆装箱为SQL的性能损耗，Val值规定为string */
    }

    /// <summary>
    /// 文章
    /// </summary>
    public struct Post : ITableIndex, ITablePrimary
    {
        /*文章索引*/
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 文章模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string Type { get; set; }

        /*文章体*/
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        /*分类数据*/
        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Archiv { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /*时间数据*/
        /// <summary>
        /// 
        /// </summary>
        public DateTime CT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LCT { get; set; }

        /*计数数据*/
        /// <summary>
        /// 
        /// </summary>
        public int UVCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StarCount { get; set; }

        /*其他*/
        /// <summary>
        /// 
        /// </summary>
        public string Cover { get; set; }
    }
    /// <summary>
    /// 用户
    /// </summary>
    public struct User : ITableUser
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }
    }
    /// <summary>
    /// 啪啦数据库信息
    /// </summary>
    public struct PLDB : IPLDB
    {
        /// <summary>
        /// Pala数据表所在数据库
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 文本表
        /// </summary>
        public PLTables Tables { get; set; }
        /// <summary>
        /// 文本视图
        /// </summary>
        public PLViews Views { get; set; }
        /// <summary>
        /// 数据库管理器实例
        /// </summary>
        public MySqlConnH MySqlConnH { get; set; }
    }

    #region 文章键
    /// <summary>
    /// 文章模式
    /// </summary>
    public struct Mode : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 文章类型
    /// </summary>
    public struct Type : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }

    /// <summary>
    /// 文章标题
    /// </summary>
    public struct Title : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 文章摘要
    /// </summary>
    public struct Summary : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 文章内容
    /// </summary>
    public struct Content : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }

    /// <summary>
    /// 文章归档
    /// </summary>
    public struct Archiv : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 文章标签
    /// </summary>
    public struct Label : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public struct CT : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public struct LCT : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }

    /// <summary>
    /// 浏览计数
    /// </summary>
    public struct UVCount : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    /// <summary>
    /// 星星计数
    /// </summary>
    public struct StarCount : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }

    /// <summary>
    /// 文章标题
    /// </summary>
    public struct Cover : IKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
    }
    #endregion
}
