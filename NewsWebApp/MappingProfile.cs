using NewsWebApp.Models;
using AutoMapper;
using NewsWebApp.Dtos;

namespace NewsWebApp
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<PostCategory, PostCategoryDto>()
                .ForMember(x => x.Category, ent => ent.MapFrom(p => p.Category));
               
            CreateMap<Tag, TagDto>();

            CreateMap<PostTag, PostTagDto>().ForMember(x=>x.Tag,ent=>ent.MapFrom(p=>p.Tag));

            CreateMap<Comment, CommentDto>()
                .ForMember(dto=>dto.UserName,ent=>ent.MapFrom(x=>x.AppUser.UserName))
                .ForMember(dto=>dto.ProfileImg,ent=>ent.MapFrom(x=>x.AppUser.ProfileImg));

            CreateMap<PostTag, TagDto>();

            CreateMap<Post, PostDto>()
                //.ForMember(dto=>dto.Categories,ent=>ent.MapFrom(p=>p.PostCategories))
                .ForMember(dto => dto.Tags, ent => ent.MapFrom(p => p.Tags))
                .ForMember(dto => dto.UserName, ent => ent.MapFrom(p => p.AppUser.UserName))
                .ForMember(x => x.ProfileImg, ent => ent.MapFrom(p => p.AppUser.ProfileImg));
            CreateMap<Post, PostForMl>();
          


        }
    }
}
