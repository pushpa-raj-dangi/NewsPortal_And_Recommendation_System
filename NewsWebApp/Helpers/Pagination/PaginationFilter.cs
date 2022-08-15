using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebApp.Helpers.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 5;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 5 ? 5 : pageSize;
        }
        public PaginationFilter(int pageNumber, int pageSize, string search)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 5 ? 5 : pageSize;
            this.Search = search;
        }
    }
}