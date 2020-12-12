using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MySql.Data.MySqlClient;

using WaterLibrary.MySQL;
using WaterLibrary.pilipala;
using WaterLibrary.pilipala.Database;
using WaterLibrary.CommentLake.CommentKey;

namespace WaterLibrary.CommentLake
{
    /// <summary>
    /// 评论湖
    /// </summary>
    public class CommentLake
    {
        /// <summary>
        /// 表集
        /// </summary>
        private PLTables Tables { get; set; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        private MySqlManager MySqlManager { get; set; }

        /// <summary>
        /// 准备评论湖
        /// </summary>
        /// <param name="CORE"></param>
        public void Ready(CORE CORE)
        {
            Tables = CORE.Tables;
            MySqlManager = CORE.MySqlManager;
        }

        /// <summary>
        /// 得到最大评论ID（私有）
        /// </summary>
        /// <returns>不存在返回1</returns>
        private int GetMaxCommentID()
        {
            string SQL = $"SELECT max(CommentID) FROM {Tables.Comment}";

            object MaxCommentID = MySqlManager.GetKey(SQL);
            return MaxCommentID == DBNull.Value ? 1 : Convert.ToInt32(MaxCommentID);
        }
        /// <summary>
        /// 获得目标文章下的最大评论楼层（私有）
        /// </summary>
        /// <returns>不存在返回1</returns>
        private int GetMaxFloor(int PostID)
        {
            string SQL = $"SELECT max(Floor) FROM {Tables.Comment} WHERE PostID = {PostID}";

            object MaxFloor = MySqlManager.GetKey(SQL);
            return MaxFloor == DBNull.Value ? 1 : Convert.ToInt32(MaxFloor);
        }

        /// <summary>
        /// 评论总计数
        /// </summary>
        public int TotalCommentCount
        {
            get
            {
                object Count = MySqlManager.GetKey($"SELECT COUNT(*) FROM {Tables.Comment}");
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
        }
        /// <summary>
        /// 得到目标文章的评论计数
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public int GetCommentCount(int PostID)
        {
            object Count = MySqlManager.GetKey($"SELECT COUNT(*) FROM {Tables.Comment} WHERE PostID = {PostID}");
            return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
        }

        /// <summary>
        /// 获得评论属性
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="CommentID">目标评论ID</param>
        /// <returns></returns>
        public string GetComment<T>(int CommentID) where T : ICommentKey
        {
            /* int类型传入，SQL注入无效 */
            string SQL = $"SELECT {typeof(T).Name} FROM {Tables.Comment} WHERE CommentID = {CommentID}";
            return MySqlManager.GetKey(SQL).ToString();
        }

        /// <summary>
        /// 得到被评论文章的ID列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetCommentedPostID()
        {
            var List = new List<int>();

            string SQL = string.Format("SELECT ID FROM {0} JOIN {1} ON {0}.ID={1}.PostID GROUP BY {0}.ID", Tables.Index, Tables.Comment);

            foreach (DataRow Row in MySqlManager.GetTable(SQL).Rows)
            {
                List.Add(Convert.ToInt32(Row[0]));
            }

            return List;
        }
        /// <summary>
        /// 获得目标文章的评论列表
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public CommentSet GetComments(int PostID)
        {
            CommentSet CommentSet = new CommentSet();

            /* 按楼层排序 */
            string SQL = $"SELECT * FROM {Tables.Comment} WHERE PostID = ?PostID ORDER BY Floor";

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            DataTable result = MySqlManager.GetTable(MySqlCommand);

            foreach (DataRow Row in result.Rows)
            {
                CommentSet.Add(new Comment
                {
                    CommentID = Convert.ToInt32(Row["CommentID"]),
                    HEAD = Convert.ToInt32(Row["HEAD"]),
                    PostID = Convert.ToInt32(Row["PostID"]),
                    Floor = Convert.ToInt32(Row["Floor"]),

                    User = Convert.ToString(Row["User"]),
                    Email = Convert.ToString(Row["Email"]),
                    Content = Convert.ToString(Row["Content"]),
                    WebSite = Convert.ToString(Row["WebSite"]),
                    Time = Convert.ToDateTime(Row["Time"]),
                });
            }
            return CommentSet;
        }
        /// <summary>
        /// 获得目标评论的回复列表
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public CommentSet GetCommentReplies(int CommentID)
        {
            CommentSet CommentSet = new CommentSet();

            DataTable result =
                MySqlManager.GetTable($"SELECT * FROM {Tables.Comment} WHERE HEAD={CommentID} ORDER BY Floor");

            foreach (DataRow Row in result.Rows)
            {
                CommentSet.Add(new Comment
                {
                    /* 数据库中的量 */
                    CommentID = Convert.ToInt32(Row["CommentID"]),
                    HEAD = Convert.ToInt32(Row["HEAD"]),
                    PostID = Convert.ToInt32(Row["PostID"]),
                    Floor = Convert.ToInt32(Row["Floor"]),

                    User = Convert.ToString(Row["User"]),
                    Email = Convert.ToString(Row["Email"]),
                    Content = Convert.ToString(Row["Content"]),
                    WebSite = Convert.ToString(Row["WebSite"]),
                    Time = Convert.ToDateTime(Row["Time"]),
                });
            }
            return CommentSet;
        }

        /// <summary>
        /// 添加评论(CommentID和Time由系统生成，无需传入)
        /// </summary>
        /// <param name="Comment">评论内容</param>
        /// <returns></returns>
        public bool AddComment(Comment Comment)
        {
            string SQL = string.Format(
                "INSERT INTO {0} " +
                "(CommentID , HEAD, PostID, Floor, User, Email, Content, WebSite, Time) VALUES " +
                "(?CommentID,?HEAD,?PostID,?Floor,?User,?Email,?Content,?WebSite,?Time)"

                , Tables.Comment);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "CommentID", Val = GetMaxCommentID() + 1 },
                new MySqlParm() { Name = "HEAD", Val =  Comment.HEAD},
                new MySqlParm() { Name = "PostID", Val =  Comment.PostID},
                new MySqlParm() { Name = "Floor", Val =  GetMaxFloor(Comment.PostID) + 1 },

                new MySqlParm() { Name = "User", Val =  Comment.User},
                new MySqlParm() { Name = "Email", Val =  Comment.Email},
                new MySqlParm() { Name = "Content", Val =  Comment.Content},
                new MySqlParm() { Name = "WebSite", Val =  Comment.WebSite},
                new MySqlParm() { Name = "Time", Val =  DateTime.Now},
            };

            MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList);
            MySqlCommand.Connection = MySqlManager.Connection;

            /* 开始事务 */
            MySqlCommand.Transaction = MySqlManager.Connection.BeginTransaction();

            if (MySqlCommand.ExecuteNonQuery() == 1)
            {
                /* 指向表和拷贝表分别添加1行数据 */
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
        /// 删除评论(相关回复不会被删除)
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public bool DeleteComment(int CommentID)
        {
            MySqlCommand MySqlCommand = new MySqlCommand
            {
                CommandText = $"DELETE FROM {Tables.Comment} WHERE CommentID = {CommentID}",
                Connection = MySqlManager.Connection,

                /* 开始事务 */
                Transaction = MySqlManager.Connection.BeginTransaction()
            };

            if (MySqlManager.QueryOnly(ref MySqlCommand) == 1)
            {
                /* 删除1条评论，操作行为1 */
                MySqlCommand.Transaction.Commit();
                return true;
            }
            else
            {
                MySqlCommand.Transaction.Rollback();
                return false;
            }
        }
    }

    namespace CommentKey
    {
        /// <summary>
        /// 表键值接口
        /// </summary>
        public interface ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            string Val { get; set; }/* 为减少拆装箱为SQL的性能损耗，Val值规定为string */
        }

        /// <summary>
        /// 被评论的文章ID
        /// </summary>
        public struct PostID : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 评论用户
        /// </summary>
        public struct User : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public struct Email : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        public struct Content : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 用户站点
        /// </summary>
        public struct WebSite : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 回复到的评论ID
        /// </summary>
        public struct HEAD : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
        /// <summary>
        /// 评论创建时间
        /// </summary>
        public struct Time : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }

        /// <summary>
        /// 所在楼层
        /// </summary>
        public struct Floor : ICommentKey
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int CommentID { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Val { get; set; }
        }
    }
    /// <summary>
    /// 评论
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="Key">索引名</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get
            {
                /* 通过反射获取属性 */
                return GetType().GetProperty(Key).GetValue(this);
            }
            set
            {
                /* 通过反射设置属性 */
                System.Type ThisType = GetType();
                System.Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
                ThisType.GetProperty(Key).SetValue(this, Convert.ChangeType(value, KeyType));
            }
        }
        /// <summary>
        /// 将当前对象序列化到JSON
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            return JsonConvert.SerializeObject
                (this, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public Comment()
        {
            /* -1表示未被赋值，同时也于数据库的非负冲突 */
            CommentID = -1;
            HEAD = -1;
            PostID = -1;
            Floor = -1;

            User = "";
            Email = "";
            Content = "";
            WebSite = "";
            Time = new DateTime();
        }

        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// 回复到的评论ID
        /// </summary>
        public int HEAD { get; set; }
        /// <summary>
        /// 被评论的文章ID
        /// </summary>
        public int PostID { get; set; }
        /// <summary>
        /// 所在楼层
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// 评论用户
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 用户站点
        /// </summary>
        public string WebSite { get; set; }
        /// <summary>
        /// 评论创建时间
        /// </summary>
        public DateTime Time { get; set; }

    }
    /// <summary>
    /// 评论数据集
    /// </summary>
    public class CommentSet : IEnumerable
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return CommentList.GetEnumerator();
        }
        private readonly List<Comment> CommentList = new List<Comment>();
        /// <summary>
        /// 将当前对象序列化到JSON
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            return JsonConvert.SerializeObject
                (this, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 当前数据集的评论对象数
        /// </summary>
        public int Count
        {
            get { return CommentList.Count; }
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="Comment">评论对象</param>
        public void Add(Comment Comment)
        {
            CommentList.Add(Comment);
        }
        /// <summary>
        /// 取得数据集中的最后一个评论对象
        /// </summary>
        /// <returns></returns>
        public Comment Last()
        {
            return CommentList.Last();
        }

        /// <summary>
        /// 数据集内最近一月(30天内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinMonthCount()
        {
            int Count = 0;
            foreach (Comment Comment in CommentList)
            {
                if (Comment.Time > DateTime.Now.AddDays(-30))
                {
                    Count++;
                }
            }
            return Count;
        }
        /// <summary>
        /// 数据集内最近一周(7天内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinWeekCount()
        {
            int Count = 0;
            foreach (Comment Comment in CommentList)
            {
                if (Comment.Time > DateTime.Now.AddDays(-7))
                {
                    Count++;
                }
            }
            return Count;
        }
        /// <summary>
        /// 数据集内最近一天(24小时内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinDayCount()
        {
            int Count = 0;
            foreach (Comment Comment in CommentList)
            {
                if (Comment.Time > DateTime.Now.AddDays(-1))
                {
                    Count++;
                }
            }
            return Count;
        }
    }
}
