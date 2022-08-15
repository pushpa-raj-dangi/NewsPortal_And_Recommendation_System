using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebApp.Data;
using NewsWebApp.Helpers;
using NewsWebApp.Models;
using NewsWebApp.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using NewsWebApp.Services;
using NewsWebApp.Repositories;
using NewsWebApp.Extensions;
using System.Threading.Tasks;

namespace NewsWebApp.Controllers
{

    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Comment> _commentRepo;

        public IWebHostEnvironment HostingEnvironment => hostingEnvironment;
        public PostsController(ApplicationDbContext context,
            IFileUploadService fileUploadService,
            IWebHostEnvironment hostingEnvironment,
            UserManager<AppUser> userManager,
            IRepository<Post> repository,
            IRepository<AppUser> userRepository,
            IRepository<Category> categoryRepository,
            IRepository<Tag> tagRepository,
            IRepository<Comment> commentRepo

            )
        {
            _context = context;
            _fileUploadService = fileUploadService;
            this.hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _postRepository = repository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _commentRepo = commentRepo;
        }
        public IActionResult Index()
        {
            var posts = _postRepository.All();

            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new PostCreateViewModel
            {
                Tags = _tagRepository.All(),
                Categories = _categoryRepository.All(),
                Posts = _postRepository.All().Where(x => x.PostStatus == PostStatus.Publish).ToList(),
            };
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _postRepository.Get(id);
            PostCreateViewModel viewModel = new PostCreateViewModel
            {
                Categories = _categoryRepository.All(),
                Tags = _tagRepository.All(),
                Id = post.Id,
                Name = post.Name,
                Slug = post.Slug,
                Content = post.Content,
                PostStatus = post.PostStatus,
                Post = _postRepository.FindWithDraft(x => x.Id == id).FirstOrDefault(),
                SelectedCategory = post.PostCategories.Select(pc => pc.CategoryId).ToList(),
                SelectedTag = post.PostTags.Select(pt => pt.TagId).ToList()
            };

            ViewBag.Categories = new SelectList(_categoryRepository.All(), "Id", "Name");
            ViewBag.Tags = new SelectList(_tagRepository.All(), "Id", "Name");



            if (viewModel.Post == null)
                return View("NotFound");
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel newspost, int[] SelectedCategoryIds, int[] SelectedTagIds)
        {

            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(newspost.Name);

            while (_postRepository.Find(t => t.Slug == slug).Any())
            {
                slug += rand.Next(1000, 9999);
            }
            newspost.Slug = slug;


            foreach (var selectedCatId in SelectedCategoryIds)
            {
                newspost.PostCategories.Add(new PostCategory { CategoryId = selectedCatId });
            }

            foreach (var selectedTagId in SelectedTagIds)
            {
                newspost.PostTags.Add(new PostTag { TagId = selectedTagId });
            }



            string unique = _fileUploadService.Upload(newspost.Picture);



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.Find(u => u.Id == userId).FirstOrDefault();

            if (!ModelState.IsValid)
                return View();


            var postModel = new Post
            {
                Content = newspost.Content,
                Name = newspost.Name,
                Categories = newspost.Categories,
                Tags = newspost.Tags,
                PostCategories = newspost.PostCategories,
                PostStatus = newspost.PostStatus,
                Picture = unique,
                PostTags = newspost.PostTags,
                Slug = newspost.Slug,
                AppUser = user,
                CreatedDate = (newspost.CreatedDateString != null) ? Convert.ToDateTime(newspost.CreatedDateString) : DateTime.Now
            };
            _postRepository.Add(postModel);
            _postRepository.Savechanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(PostCreateViewModel newspost)
        {
            if (!ModelState.IsValid)
                return View();



            string filename = _fileUploadService.Upload(newspost.Picture);
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(newspost.Name);
            while (_postRepository.Find(t => t.Slug == slug).Any())
            {
                slug += rand.Next(1000, 9999);
            }
            newspost.Slug = slug;


            //var post = new Post
            //{
            //    Name = newspost.Name,
            //    Content = newspost.Content,
            //    Slug = slug,
            //    Picture = filename,
            //};

            var postInDb = _context.Posts.Include(p => p.PostCategories).Include(p => p.PostTags)
                    .Where(p => p.Id == newspost.Id).FirstOrDefault();


            postInDb.Content = newspost.Content;
            postInDb.Name = newspost.Name;
            //postInDb.CreatedDate = (newspost.CreatedDateString != null) ? Convert.ToDateTime(newspost.CreatedDateString) : DateTime.Now;
            postInDb.PostCategories = new List<PostCategory>();
            foreach (var CategoryId in newspost.SelectedCategory)
            {
                postInDb.PostCategories.Add(new PostCategory { CategoryId = CategoryId });
            }
            postInDb.PostTags = new List<PostTag>();
            foreach (var TagId in newspost.SelectedTag)
            {
                postInDb.PostTags.Add(new PostTag { TagId = TagId });
            }

            postInDb.PostStatus = newspost.PostStatus;
            postInDb.Picture = filename;
            postInDb.Slug = newspost.Slug;

            var postDto = newspost;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));




        }
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        //[HttpGet("{id}")]
        [Route("posts/news/{slug?}")]

        public async Task<IActionResult> News(string slug)
        {

            var post =
                await _context.Posts.Include(x => x.AppUser).Include(x => x.PostTags).ThenInclude(x => x.Tag).Where(p => p.Slug == slug).SingleOrDefaultAsync();
            //_postRepository.Find(pt => pt.Slug.Equals(slug)).Single;


            if (post == null)
                return View("NotFound");
            //var pId = post.PostCategories.Select(pn => pn.CategoryId).SingleOrDefault();

            //var pId = post.PostCategories.Select(pn => pn.CategoryId).FirstOrDefault();

            //ViewData["relatedPost"] = _postRepository.Find(x=>x.PostCategories.Any(pc => pc.CategoryId == pId) && x.CreatedDate <= DateTime.Now).OrderByDescending(p => p.CreatedDate).Take(4).Skip(1).ToList();

            //var user = await _context.Users.FindAsync(post.AppUser.UserName);
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users.FindAsync(userId);

            var Comments = await
                _context.Comments.Include(p => p.AppUser).Where(d => d.PostId == post.Id).ToListAsync();


            var model = new PostDetailViewModel
            {
                Post = post,
                ListComments = Comments,
                AppUser = user,

                PostId = post.Id,
                Slug = post.Slug,
                LatestPost = await _context.Posts.Where(x => x.PostStatus == PostStatus.Publish && x.CreatedDate <= DateTime.Now)
                    .OrderByDescending(n => n.CreatedDate)
                    .Take(5)
                    .ToListAsync()

            };
            return View(model);
        }



        [AllowAnonymous]
        [Route("posts/category/{slug?}")]

        public IActionResult Category(string slug, int page = 1)
        {
            var post = _context.Posts.Where(p => p.PostCategories.Any(pc => pc.Category.Slug.Equals(slug))).OrderByDescending(p => p.CreatedDate).Where(x => x.PostStatus == PostStatus.Publish && x.CreatedDate < DateTime.Now);
            PagedResult<Post> lst = new PagedResult<Post>();
            if (post == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", slug);
            }
            ViewData["category"] = _context.Categories.SingleOrDefault(p => p.Slug.Equals(slug)).Name;
            lst = post.GetPaged(page, 10);

            return View(lst);
        }



        //[AllowAnonymous]
        //[HttpPost]
        //[Route("posts/category/{currentPage}")]

        //public IActionResult Category(int currentPage)
        //{
        //    return View(_postRepository.Find(p=>p.PostCategories.Any(pc => pc.Category.Slug.Equals(slug))).OrderByDescending(p => p.CreatedDate).ToList());
        //}


        [AllowAnonymous]
        public IActionResult PostByAuthor(string id)
        {
            var post = _postRepository.Find(x => x.AppUser.Id == id).Where(x => x.PostStatus == PostStatus.Publish).Take(5);


            if (post.Count() == 0)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var user = _context.Users.SingleOrDefault(p => p.Id == id).UserName;
            ViewData["author"] = GenerateUserName.RenderUserName(user);
            return View(post);
        }



        public IActionResult Draft()
        {

            var post = _context.Posts.Include(p => p.PostCategories).ThenInclude(c => c.Category).Include(tag => tag.PostTags).ThenInclude(pt => pt.Tag).Include(u => u.AppUser).OrderByDescending(pid => pid.CreatedDate).Where(x => x.PostStatus == PostStatus.Draft);

            return View(post);
        }

        public IActionResult Trash()
        {

            var post = _context.Posts.Include(p => p.PostCategories).ThenInclude(c => c.Category).Include(tag => tag.PostTags).ThenInclude(pt => pt.Tag).Include(u => u.AppUser).OrderByDescending(pid => pid.CreatedDate).Where(x => x.PostStatus == PostStatus.Trash);

            return View(post);
        }

        [AllowAnonymous]
        public IActionResult PostByTag(int id)
        {

            var post = _postRepository.Find(p => p.PostTags.Any(pc => pc.TagId == id)).OrderByDescending(p => p.CreatedDate);

            if (post.Count() == 0)
                return View("NotFound", id);
            ViewData["tag"] = _context.Tags.FirstOrDefault(p => p.Id == id).Name;
            return View(post);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.Include(pst => pst.PostCategories).ThenInclude(c => c.Category).Include(tag => tag.PostTags).ThenInclude(pt => pt.Tag).FirstOrDefaultAsync(pst => pst.Id == id);

            if (post == null)
                return View("NotFound");
            var p = post.PostCategories.Select(pcat => pcat.CategoryId);
            return View(post);
        }

        public IActionResult DeleteConfirm(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return RedirectToAction(nameof(Index));
            _context.Remove(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
