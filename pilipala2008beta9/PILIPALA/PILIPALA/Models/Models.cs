using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace PILIPALA.Models
{
    public class UserModel
    {
        public string Account { get; set; }
        public string PWD { get; set; }
    }
    public class ThemeConfigModel
    {
        public string Path { get; set; }
    }
    namespace Form
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
        public class PostModel
        {
            public int PostID { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Mode { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Type { get; set; }
            public string User { get; set; }
            public int UVCount { get; set; }
            public int StarCount { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Title { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Summary { get; set; }
            public string Content { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string ArchiveName { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Label { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Cover { get; set; }
        }
    }
}
