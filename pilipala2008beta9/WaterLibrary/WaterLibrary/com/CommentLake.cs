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
using WaterLibrary.pilipala.Entity;
using WaterLibrary.pilipala.Entity.CommentProp;
using WaterLibrary.pilipala.Database;

namespace WaterLibrary.pilipala.Components
{
    /// <summary>
    /// 评论湖组件
    /// </summary>
    public class CommentLake : IPLComponent<CommentLake>
    {
        private string IndexTable { get; init; }
        private string CommentTable { get; init; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        private MySqlManager MySqlManager { get; init; }

        /// <summary>
        /// 默认构造
        /// </summary>
        private CommentLake() { }
        /// <summary>
        /// 工厂构造
        /// </summary>
        /// <param name="Tables">数据库表</param>
        /// <param name="MySqlManager">数据库管理器</param>
        internal CommentLake(string IndexTable, string CommentTable, MySqlManager MySqlManager)
        {
            (this.IndexTable, this.CommentTable, this.MySqlManager) = (IndexTable, CommentTable, MySqlManager);
        }

        /// <summary>
        /// 得到最大评论CommentID（私有）
        /// </summary>
        /// <returns>不存在返回1</returns>
        private int GetMaxCommentID()
        {
            string SQL = $"SELECT max(CommentID) FROM {CommentTable}";

            object MaxCommentID = MySqlManager.GetKey(SQL);
            return MaxCommentID == DBNull.Value ? 1 : Convert.ToInt32(MaxCommentID);
        }
        /// <summary>
        /// 获得目标文章下的最大评论楼层（私有）
        /// </summary>
        /// <returns>不存在返回1</returns>
        private int GetMaxFloor(int PostID)
        {
            string SQL = $"SELECT max(Floor) FROM {CommentTable} WHERE PostID = {PostID}";

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
                object Count = MySqlManager.GetKey($"SELECT COUNT(*) FROM {CommentTable}");
                return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
            }
        }
        /// <summary>
        /// 得到目标文章的评论计数
        /// </summary>
        /// <param name="PostID">目标文章PostID</param>
        /// <returns></returns>
        public int GetCommentCount(int PostID)
        {
            object Count = MySqlManager.GetKey($"SELECT COUNT(*) FROM {CommentTable} WHERE PostID = {PostID}");
            return Count == DBNull.Value ? 0 : Convert.ToInt32(Count);
        }

        /// <summary>
        /// 获得评论属性
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="CommentID">目标评论CommentID</param>
        /// <returns></returns>
        public string GetComment<T>(int CommentID) where T : ICommentProp
        {
            /* int类型传入，SQL注入无效 */
            string SQL = $"SELECT {typeof(T).Name} FROM {CommentTable} WHERE CommentID = {CommentID}";
            return MySqlManager.GetKey(SQL).ToString();
        }

        /// <summary>
        /// 得到被评论文章的PostID列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetCommentedPostID()
        {
            List<int> List = new();

            string SQL = string.Format("SELECT PostID FROM {0} JOIN {1} ON {0}.PostID={1}.PostID GROUP BY {0}.PostID", IndexTable, CommentTable);

            foreach (DataRow Row in MySqlManager.GetTable(SQL).Rows)
            {
                List.Add(Convert.ToInt32(Row[0]));
            }

            return List;
        }
        /// <summary>
        /// 获得目标文章的评论列表
        /// </summary>
        /// <param name="PostID">目标文章PostID</param>
        /// <returns></returns>
        public CommentSet GetComments(int PostID)
        {
            CommentSet CommentSet = new();

            /* 按楼层排序 */
            string SQL = $"SELECT * FROM {CommentTable} WHERE PostID = ?PostID ORDER BY Floor";

            DataTable result = MySqlManager.GetTable(SQL, new MySqlParameter[]
            {
                new("PostID", PostID)
            });

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
            CommentSet CommentSet = new();

            DataTable result =
                MySqlManager.GetTable($"SELECT * FROM {CommentTable} WHERE HEAD={CommentID} ORDER BY Floor");

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
            string SQL = $"INSERT INTO {CommentTable} " +
                        "( CommentID, HEAD, PostID, Floor, User, Email, Content, WebSite, Time) VALUES " +
                        "(?CommentID,?HEAD,?PostID,?Floor,?User,?Email,?Content,?WebSite,?Time)";

            MySqlParameter[] parameters =
            {
                new("CommentID", GetMaxCommentID() + 1 ),
                new("HEAD", Comment.HEAD),
                new("PostID", Comment.PostID),
                new("Floor", GetMaxFloor(Comment.PostID) + 1 ),

                new("User", Comment.User),
                new("Email", Comment.Email),
                new("Content", Comment.Content),
                new("WebSite", Comment.WebSite),
                new("Time", DateTime.Now),
            };

            using MySqlCommand MySqlCommand = new(SQL, MySqlManager.Connection);
            MySqlCommand.Parameters.AddRange(parameters);

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
            using MySqlCommand MySqlCommand = new MySqlCommand
            {
                CommandText = $"DELETE FROM {CommentTable} WHERE CommentID = {CommentID}",
                Connection = MySqlManager.Connection,

                /* 开始事务 */
                Transaction = MySqlManager.Connection.BeginTransaction()
            };

            if (MySqlCommand.ExecuteNonQuery() == 1)
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
}
namespace WaterLibrary.pilipala.Entity
{
    namespace CommentProp
    {
        /// <summary>
        /// 表键值接口
        /// </summary>
        public interface ICommentProp
        {

        }

        /// <summary>
        /// 被评论的文章PostID
        /// </summary>
        public struct PostID : ICommentProp
        {

        }
        /// <summary>
        /// 评论用户
        /// </summary>
        public struct User : ICommentProp
        {

        }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public struct Email : ICommentProp
        {

        }
        /// <summary>
        /// 评论内容
        /// </summary>
        public struct Content : ICommentProp
        {

        }
        /// <summary>
        /// 用户站点
        /// </summary>
        public struct WebSite : ICommentProp
        {

        }
        /// <summary>
        /// 回复到的评论CommentID
        /// </summary>
        public struct HEAD : ICommentProp
        {

        }
        /// <summary>
        /// 评论创建时间
        /// </summary>
        public struct Time : ICommentProp
        {

        }

        /// <summary>
        /// 所在楼层
        /// </summary>
        public struct Floor : ICommentProp
        {

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
            /* 通过反射获取属性 */
            get => GetType().GetProperty(Key).GetValue(this);
            set
            {
                /* 通过反射设置属性 */
                Type ThisType = GetType();
                Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
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
            Time = new();
        }

        /// <summary>
        /// 评论CommentID
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// 回复到的评论CommentID
        /// </summary>
        public int HEAD { get; set; }
        /// <summary>
        /// 被评论的文章PostID
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
            get => CommentList.Count;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="Comment">评论对象</param>
        public void Add(Comment Comment) => CommentList.Add(Comment);
        /// <summary>
        /// 取得数据集中的最后一个评论对象
        /// </summary>
        /// <returns></returns>
        public Comment Last() => CommentList.Last();
        /// <summary>
        /// 对数据集内的每一个对象应用Action
        /// </summary>
        /// <param name="todo">Action委托</param>
        /// <returns>返回操作后的数据集</returns>
        public CommentSet ForEach(Action<Comment> todo)
        {
            CommentList.ForEach(todo);
            return this;
        }

        /// <summary>
        /// 数据集内最近一月(30天内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinMonthCount()
        {
            int Count = 0;
            CommentList.ForEach(el =>
            {
                if (el.Time > DateTime.Now.AddDays(-30))
                {
                    Count++;
                }
            });
            return Count;
        }
        /// <summary>
        /// 数据集内最近一周(7天内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinWeekCount()
        {
            int Count = 0;
            CommentList.ForEach(el =>
            {
                if (el.Time > DateTime.Now.AddDays(-7))
                {
                    Count++;
                }
            });
            return Count;
        }
        /// <summary>
        /// 数据集内最近一天(24小时内)的评论数量
        /// </summary>
        /// <returns></returns>
        public int WithinDayCount()
        {
            int Count = 0;
            CommentList.ForEach(el =>
            {
                if (el.Time > DateTime.Now.AddDays(-1))
                {
                    Count++;
                }
            });
            return Count;
        }
    }
}
