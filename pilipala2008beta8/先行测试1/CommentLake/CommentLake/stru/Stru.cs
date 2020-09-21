using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentLake.Stru
{
    public struct Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public string CommentID { get; set; }
        /// <summary>
        /// 被评论的文章ID
        /// </summary>
        public string PostID { get; set; }
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
        public string WebSIte { get; set; }
        /// <summary>
        /// 回复到的ID（为空时表示不回复）
        /// </summary>
        public string HEAD { get; set; }
    }
}
