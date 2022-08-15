using NewsWebApp.Models;
using System.Collections.Generic;

namespace NewsWebApp.Dtos
{
    public class PostForMl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
