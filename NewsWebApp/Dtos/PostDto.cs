using NewsWebApp.Models;
using System;
using System.Collections.Generic;

namespace NewsWebApp.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string ProfileImg { get; set; }
        public string UserName { get; set; }
        public PostStatus PostStatus { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public ICollection<TagDto> Tags { get; set; }
        public virtual ICollection<PostTagDto> PostTags { get; set; }
        public virtual ICollection<PostCategoryDto> PostCategories { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CommentDto> Comments { get; set; }



    }
}
