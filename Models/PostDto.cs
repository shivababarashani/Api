using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Api.Models
{
    //حالت نمایش با گرفتن به عنوان پارامتر ورودی رو تفکیک کردیم

    //حالت گرفتن به عنوان پارامتر
    public class PostDto : BaseDto<PostDto, Post, Guid>
    {
        //public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

    }
    //حالت نمایش به کلاینت
    public class PostSelectDto : BaseDto<PostSelectDto, Post, Guid>
    {
        //public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; } //Category.Name
        public string AuthorFullName { get; set; } //Author.FullName
        public string FullTitle { get; set; } // => mapped from "Title (Category.Name)"

        //[IgnoreMap]//باعث میشه مپینگی بینشون انجام نشه بصورت دستی
        //public string Category { get; set; }

        //مپ های سفارشی با اون مدل رو میشه اینجا اضافه کرد
        public override void CustomMappings(IMappingExpression<Post, PostSelectDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.FullTitle,
                    config => config.MapFrom(src => $"{src.Title} ({src.Category.Name})"));
        }
    }
}
