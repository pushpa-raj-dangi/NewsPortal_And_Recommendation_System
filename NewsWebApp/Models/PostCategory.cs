﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebApp.Models
{
    public class PostCategory
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }


    }
}
