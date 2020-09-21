using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.com.MySQL;
using WaterLibrary.stru.MySQL;
using CommentLake.Stru;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql;
using System.Data;

namespace CommentLake
{
    public class CommentLake
    {
        public string DataBase { get; private set; }
        public string CommentTable { get; private set; }

        public MySqlConnH MySqlConnH { get; private set; }

        public CommentLake(string DataBase, string CommentTable)
        {
            this.DataBase = DataBase;
            this.CommentTable = CommentTable;
        }

        public void DBCINIT(MySqlConn MySqlConn)
        {
            MySqlConnH.Start(MySqlConn);
        }

        /// <summary>
        /// 得到最大评论ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        private int GetMaxCommentID()
        {
            try
            {
                string SQL = string.Format("SELECT max(CommentID) FROM {0}.`{1}`", DataBase, CommentTable);

                return Convert.ToInt32(MySqlConnH.GetRow(SQL)[0]);
            }
            catch
            {
                return -1;
            }
        }

        /* 评论删除功能列为第二优先级 */

        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="PostID">评论归属的文章ID</param>
        /// <param name="Comment">评论内容</param>
        /// <param name="HEAD">回复到评论的CommentID，为空则表示为非回复性评论</param>
        /// <returns></returns>
        public bool AddComment(Comment Comment)
        {
            string SQL = string.Format(
                "INSERT INTO {0}.`{1}` " +
                "(`CommentID`, `PostID`, `Name`, `Email`, `Content`, `WebSite`, `HEAD`) VALUES " +
                "(`?CommentID`,`?PostID`,`?Name`,`?Email`,`?Content`,`?WebSite`,`?HEAD`);"

                , DataBase, CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?CommentID", Val = GetMaxCommentID() + 1 },
                new MySqlParm() { Name = "?PostID", Val =  Comment.PostID},
                new MySqlParm() { Name = "?Name", Val =  Comment.Name},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?HEAD", Val =  Comment.HEAD},
            };

            if (MySqlConnH.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }

        /// <summary>
        /// 得到目标文章ID下的评论列表
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public List<Comment> GetCommentList(int PostID)
        {
            List<Comment> CommentList = new List<Comment>();

            string SQL = string.Format("SELECT * FROM {0}.'{1}' WHERE PostID = ?PostID", DataBase,CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>/* 为参数化查询添加元素 */
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using (MySqlCommand MySqlCommand = MySqlConnH.ParmQueryCMD(SQL, ParmList))/* 参数化查询 */
            {
                DataTable result = MySqlConnH.GetTable(MySqlCommand);

                foreach(DataRow Row in result.Rows)
                {
                    CommentList.Add(new Comment {
                        CommentID=Convert.ToString(Row["CommentID"]),
                        PostID = Convert.ToString(Row["PostID"]),
                        Name = Convert.ToString(Row["Name"]),
                        Email = Convert.ToString(Row["Email"]),
                        Content = Convert.ToString(Row["Content"]),
                        WebSIte = Convert.ToString(Row["WebSite"]),
                        HEAD = Convert.ToString(Row["HEAD"])
                    });
                }
            }
            return CommentList;
        }
    }
}
