using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebApp.Data;
using NewsWebApp.Dtos;
using NewsWebApp.Helpers;
using NewsWebApp.Helpers.Pagination;
using NewsWebApp.Helpers.Services;
using NewsWebApp.Models;
using NewsWebApp.Repositories;

namespace NewsWebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase

    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Post> _repository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;


        public PostsController(ApplicationDbContext context, IRepository<Post> repository, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
            _uriService = uriService;

        }

        public IActionResult All()
        {
            // var posts = _context.Posts.OrderByDescending(x => x.CreatedDate).ProjectTo<PostForRecommendDto>(_mapper.ConfigurationProvider).Where(x => x.PostStatus == PostStatus.Publish).ToList();
            var posts = _context.Posts.Where(x => x.PostStatus == PostStatus.Publish).OrderByDescending(x => x.CreatedDate).Include(x => x.PostCategories)
                .ThenInclude(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    content = x.Content,
                    category = x.PostCategories.Select(k => k.Category.Name).FirstOrDefault(),
                    Tag = x.PostTags.Select(k => k.Tag.Name).FirstOrDefault()
                });
            return Ok(posts);
        }


        [HttpGet("draft/count")]
        public IActionResult DraftCount()
        {

            var post = _context.Posts.OrderByDescending(pid => pid.CreatedDate).Where(x => x.PostStatus == PostStatus.Draft).Count();

            return Ok(post);
        }

        [HttpGet("news/search")]

        public List<string> Search([FromQuery] string name)
        {
            var route = Request.Path.Value;

            System.Console.WriteLine(route);
            var posts = _context.Posts.Where(x => x.PostStatus == PostStatus.Publish).OrderByDescending(x => x.CreatedDate).Include(x => x.PostCategories)
                 .ThenInclude(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).Select(x => x.Name).Take(10)
                 .ToList();

            return posts;
        }

        [HttpGet("alnews")]
        public async Task<IActionResult> GetAllP([FromQuery] PaginationFilter filter)
        {

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.Search);
            var pagedData =
            await _context.Posts.Where(x => x.PostStatus == PostStatus.Publish).OrderByDescending(x => x.CreatedDate).Include(x => x.PostCategories)
                .ThenInclude(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    content = x.Content,
                    Categories = x.PostCategories.Select(k => k.Category.Name),
                    Tags = x.PostTags.Select(k => k.Tag.Name),
                    UserName = x.AppUser.UserName,
                    profileImg = x.AppUser.ProfileImg,
                    createdDate = x.CreatedDate,
                    canAccess = (x.AppUser.UserName == User.Identity.Name || User.IsInRole("Admin"))
                }).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            // await _context.Posts.OrderByDescending(x => x.CreatedDate).ProjectTo<PostDto>(_mapper.ConfigurationProvider).Where(x => x.PostStatus == PostStatus.Publish)
            //     .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            //     .Take(validFilter.PageSize)
            //     .ToListAsync();
            var totalRecords = _context.Posts.Where(x => x.PostStatus == PostStatus.Publish).Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

            // var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = _context.Posts.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            //    .Take(validFilter.PageSize)
            //    .ToList();
            // var totalRecords = _context.Posts.Count();
            // return Ok(new PagedResponse<IEnumerable<Post>>(response, validFilter.PageNumber, validFilter.PageSize));
        }


        [HttpGet("trash/count")]
        public IActionResult TrashCount()
        {
            var post = _context.Posts.OrderByDescending(pid => pid.CreatedDate).Where(x => x.PostStatus == PostStatus.Trash).Count();

            return Ok(post);
        }


        [HttpGet("{id}")]
        public IActionResult GetSingle(int id)
        {
            var posts = _context.Posts.ProjectTo<PostDto>(_mapper.ConfigurationProvider).SingleOrDefault(p => p.Id == id);
            return Ok(posts);
        }

        [HttpGet("recommendations")]
        public IActionResult GetRecomendations([FromQuery] List<int> id)
        {
            var posts =
            _context.Posts.Where(x => x.PostStatus == PostStatus.Publish && id.Contains(x.Id)).OrderByDescending(x => x.CreatedDate).Include(x => x.PostCategories)
                .ThenInclude(x => x.Category).Include(x => x.PostTags).ThenInclude(x => x.Tag).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    slug = x.Slug,
                    content = x.Content,
                    category = x.PostCategories.Select(k => k.Category.Name).FirstOrDefault(),
                    picture = x.Picture,
                    Tag = x.PostTags.Select(k => k.Tag.Name).FirstOrDefault(),
                    CreatedDate = x.CreatedDate
                }).Take(3).ToList();
            //  _repository.All().Where(x => id.Contains(x.Id)).Take(3).ToList();

            return Ok(posts);
        }

        [Authorize]

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        [HttpPost("{id}")]

        public async Task<ActionResult<Post>> EditPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.PostStatus = PostStatus.Trash;
            _context.SaveChanges();

            return Ok(post);
        }

        //private void ScheduledPost()
        //{
        //    if(DateTime.Now)
        //}

    }




}