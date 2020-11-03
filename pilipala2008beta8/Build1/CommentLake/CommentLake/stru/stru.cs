using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentLake.stru
{
    public struct Comment
    {
        /* 数据库中的量 */
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// 被评论的文章ID
        /// </summary>
        public int PostID { get; set; }
        /// <summary>
        /// 评论用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 评论用户的电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论人的个人站点
        /// </summary>
        public string WebSite { get; set; }
        /// <summary>
        /// 回复到的评论ID（为空时表示不回复）
        /// </summary>
        public int HEAD { get; set; }
        /// <summary>
        /// 评论的创建时间
        /// </summary>
        public DateTime Time { get; set; }

        /* 计算得到的量 */
        /// <summary>
        /// 所在楼层
        /// </summary>
        public int Floor { get; set; }
    }
}
