using System;

namespace NewsWebApp.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CContent { get; set; }
        public string UserName{ get; set; }
        public string ProfileImg { get; set; }
        public DateTime DateComment { get; set; }
        public bool IsApproved { get; set; }
    }
}
