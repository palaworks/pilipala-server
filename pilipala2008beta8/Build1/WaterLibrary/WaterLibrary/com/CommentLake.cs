using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;

using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.CommentLake;
using WaterLibrary.stru.CommentLake.CommentKey;
using WaterLibrary.stru.pilipala.DB;


namespace WaterLibrary.com.CommentLake
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
            string SQL = string.Format("SELECT max(CommentID) FROM {0}", Tables.Comment);

            object MaxCommentID = MySqlManager.GetKey(SQL);
            return MaxCommentID == DBNull.Value ? 1 : Convert.ToInt32(MaxCommentID);
        }
        /// <summary>
        /// 获得目标文章下的最大评论楼层（私有）
        /// </summary>
        /// <returns>不存在返回1</returns>
        private int GetMaxFloor(int PostID)
        {
            string SQL = string.Format("SELECT max(Floor) FROM {0} WHERE PostID = {1}", Tables.Comment, PostID);

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
                object Count = MySqlManager.GetKey(string.Format("SELECT COUNT(*) FROM {0}", Tables.Comment));
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
            object Count = MySqlManager.GetKey(string.Format("SELECT COUNT(*) FROM {0} WHERE PostID = {1}", Tables.Comment, PostID));
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
            string SQL = string.Format
                (
                "SELECT {0} FROM {1} WHERE CommentID = {2}"
                , typeof(T).Name, Tables.Comment, CommentID
                );
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
        public CommentSet GetCommentList(int PostID)
        {
            CommentSet CommentSet = new CommentSet();

            /* 按楼层排序 */
            string SQL = string.Format("SELECT * FROM {0} WHERE PostID = ?PostID ORDER BY Floor", Tables.Comment);

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
        public CommentSet GetCommentReplyList(int CommentID)
        {
            CommentSet CommentSet = new CommentSet();

            DataTable result =
                MySqlManager.GetTable(string.Format("SELECT * FROM {0} WHERE HEAD={1} ORDER BY Floor", Tables.Comment, CommentID));

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
                CommandText = string.Format("DELETE FROM {0} WHERE CommentID = {1}", Tables.Comment, CommentID),
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
}
