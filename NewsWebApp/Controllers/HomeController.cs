using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsWebApp.Data;
using NewsWebApp.Extensions;
using NewsWebApp.Helpers;
using NewsWebApp.Models;
using NewsWebApp.ViewModels;
namespace NewsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Search(string Search, int page = 1)


        {
            var posts = from m in _context.Posts.Include(x => x.AppUser) select m;
            PagedResult<Post> lst = new PagedResult<Post>();
            if (!String.IsNullOrEmpty(Search))
            {
                posts = posts.Where(x => x.Name.ToLower().Contains(Search.ToLower()) | x.AppUser.UserName.Contains(Search.ToLower()));
                lst = posts.GetPaged(page, 10);
                ViewData["wtitle"] = Search;
            }
            return View("Search", lst);

        }
        [ResponseCache(Duration = 50, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var viewModel = new PostViewModel
            {
                Posts = _context.Posts.Include(p => p.Categories).Include(c => c.Tags).ToList()
            };


            var homeViewModel = new HomeViewModel
            {
                PoliticsNews = GetPostsByCategory(11, 4),
                EntertainmentNews = GetPostsByCategory(2, 4),
                FeatureNews = GetPostsByCategory(12, 3),
                InternationalNews = GetPostsByCategory(9),
                BusinessNews = GetPostsByCategory(10, 4),
                SportsNews = GetPostsByCategory(1),
                TechnologyNews = GetPostsByCategory(3, 6),
                LatestUpdate =
                _context.Posts.OrderByDescending(p => p.CreatedDate)
                .Include(x => x.AppUser)
                .Take(5)
                .Where(p => p.PostStatus == PostStatus.Publish && p.CreatedDate <= DateTime.Now),
                Categories = _context.Categories.ToList(),
                //PostsByAuthor = _context.Posts.Include(post => post.AppUser).ToList()
            };




            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IEnumerable<Post> GetPostsByCategory(int? id, int limit = 3)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            return _context.Posts.Include(x => x.AppUser)
            .OrderByDescending(p => p.CreatedDate)
            .Where(p => p.PostCategories.Any(pc => pc.CategoryId == id))
            .Take(limit).Where(p => p.PostStatus == PostStatus.Publish && p.CreatedDate <= DateTime.Now)
            .ToList();
        }





    }
}
