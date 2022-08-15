using NewsWebApp.Models;
using System.Collections.Generic;

namespace NewsWebApp.Dtos
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        //public virtual List<PostCategoryDto> PostCategories { get; set; }


    }
}
