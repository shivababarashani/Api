using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace Api.Controllers.v1
{
    //api/v2/posts
    [ApiVersion("1")]//کل اکشن های این کنترلر توسط ورژن 2 قابل استفاده است
    public class PostsController : CrudController<PostDto,PostSelectDto, Post,Guid>
    {
        public PostsController(IRepository<Post> repository)
            :base(repository)
        {
                
        }
    }
}