using NewsWebApp.Models;
using System;
using System.Collections.Generic;

namespace NewsWebApp.Dtos
{
    public class PostDetailDto
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public PostStatus PostStatus { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int TotalComments { get; set; }

    }
}
