using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsWebApp.Helpers.Pagination;

namespace NewsWebApp.Helpers.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}