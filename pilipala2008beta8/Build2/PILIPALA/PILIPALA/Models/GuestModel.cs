using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace PILIPALA.Models.Guest
{
    public class CommentModel
    {
        public int PostID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string User { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Content { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WebSite { get; set; }
        public int HEAD { get; set; }
    }
}
