using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebApp.Data;
using NewsWebApp.Models;
using NewsWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var dvm = new DashboardViewModel
            {
                NoOfComments = await _context.Comments.CountAsync(),
                NumOfCategory = await _context.Categories.CountAsync(),
                NumOfPublishedPost = await _context.Posts.Where(x => x.PostStatus == PostStatus.Publish).CountAsync(),
                NoOfUsers = await _context.Users.CountAsync()

            };
            return View(dvm);
        }
    }
}
