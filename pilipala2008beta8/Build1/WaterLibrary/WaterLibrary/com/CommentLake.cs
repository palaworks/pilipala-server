using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

using WaterLibrary.com.MySQL;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.CommentLake;


namespace WaterLibrary.com.CommentLake
{
    /// <summary>
    /// 评论湖
    /// </summary>
    public class CommentLake
    {
        /// <summary>
        /// 评论表
        /// </summary>
        public string CommentTable { get; private set; }
        /// <summary>
        /// MySql数据库管理器
        /// </summary>
        public MySqlManager MySqlManager { get; private set; }

        /// <summary>
        /// 初始化CommentLake
        /// </summary>
        /// <param name="MySqlConn">MySql连接信息</param>
        /// <param name="CommentTable">评论表</param>
        public CommentLake(MySqlConn MySqlConn, string CommentTable)
        {
            this.CommentTable = CommentTable;
            MySqlManager = new MySqlManager();
            MySqlManager.Start(MySqlConn);
        }

        /// <summary>
        /// 得到最大评论ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        private int GetMaxCommentID()
        {
            try
            {
                string SQL = string.Format("SELECT max(CommentID) FROM {0}", CommentTable);

                return Convert.ToInt32(MySqlManager.GetRow(SQL)[0]);
            }
            catch
            {
                return -1;
            }
        }

        /* 评论删除功能列为第二优先级（待开发） */

        /// <summary>
        /// 得到目标文章的评论计数
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public int GetCommentCount(int PostID)
        {
            /* 按时间从早到晚排序 */
            string SQL = string.Format("SELECT COUNT(*) FROM {0} WHERE PostID = ?PostID", CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                return Convert.ToInt32(MySqlManager.GetRow(MySqlCommand)[0]);
            }
        }


        public string GetCommentPostID(int CommentID)
        {

        }
        public string GetCommentUser(int CommentID)
        {
            
        }
        public string GetCommentEmail(int CommentID)
        {

        }
        public string GetCommentContent(int CommentID)
        {

        }
        public string GetCommentWebSite(int CommentID)
        {

        }
        public string GetCommentHEAD(int CommentID)
        {

        }
        public string GetCommentTime(int CommentID)
        {

        }

        /// <summary>
        /// 得到目标文章下的评论列表
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public List<Comment> GetCommentList(int PostID)
        {
            List<Comment> CommentList = new List<Comment>();

            /* 按时间从早到晚排序 */
            string SQL = string.Format("SELECT * FROM {0} WHERE PostID = ?PostID ORDER BY Time", CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataTable result = MySqlManager.GetTable(MySqlCommand);

                /* 楼层暂存变量 */
                ushort f = 0;

                foreach (DataRow Row in result.Rows)
                {
                    CommentList.Add(new Comment
                    {
                        /* 数据库中的量 */
                        CommentID = Convert.ToInt32(Row["CommentID"]),
                        PostID = Convert.ToInt32(Row["PostID"]),
                        Name = Convert.ToString(Row["Name"]),
                        Email = Convert.ToString(Row["Email"]),
                        Content = Convert.ToString(Row["Content"]),
                        WebSite = Convert.ToString(Row["WebSite"]),
                        HEAD = Convert.ToInt32(Row["HEAD"]),
                        Time = Convert.ToDateTime(Row["Time"]),

                        /* 计算后得到的量 */
                        Floor = ++f
                    });
                }
                return CommentList;
            }
        }
        public List<Comment> GetCommentReplyList(int CommentID)
        {

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
                "(`CommentID`, `PostID`, `Name`, `Email`, `Content`, `WebSite`, `HEAD`, `Time`) VALUES " +
                "(?CommentID, ?PostID, ?Name, ?Email, ?Content, ?WebSite, ?HEAD, ?Time);"

                , CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "CommentID", Val = GetMaxCommentID() + 1 },

                new MySqlParm() { Name = "PostID", Val =  Comment.PostID},
                new MySqlParm() { Name = "Name", Val =  Comment.Name},
                new MySqlParm() { Name = "Email", Val =  Comment.Email},
                new MySqlParm() { Name = "Content", Val =  Comment.Content},
                new MySqlParm() { Name = "WebSite", Val =  Comment.WebSite},
                new MySqlParm() { Name = "HEAD", Val =  Comment.HEAD},

                new MySqlParm() { Name = "Time", Val =  DateTime.Now}
            };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                if (MySqlCommand.ExecuteNonQuery() == 1)
                    return true;
                else
                    throw new Exception("多行操作异常");
            }
        }

        /// <summary>
        /// 删除评论(相关回复不会被删除)
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public bool DelComment(int CommentID)
        {
            
        }
    }
}
