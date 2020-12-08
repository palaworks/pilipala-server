using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterLibrary.stru.CommentLake
{
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
