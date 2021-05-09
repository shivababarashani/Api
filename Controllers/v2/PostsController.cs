using Api.Models;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Api.Controllers.v2
{
    [ApiVersion("2")]
    public class PostsController : v1.PostsController
    {
        public PostsController(IRepository<Post> repository) : base(repository)
        {
        }

        public override Task<ApiResult<PostSelectDto>> Create(PostDto dto, CancellationToken cancellationToken)
        {
            return base.Create(dto, cancellationToken);
        }

        //[NonAction]//منسوخش کردیم
        public override Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        public async override Task<ActionResult<List<PostSelectDto>>> Get(CancellationToken cancellationToken)
        {

            return await Task.FromResult(new List<PostSelectDto>
            {
                new PostSelectDto
                {
                    FullTitle="FullTitle",
                    AuthorFullName="AuthorFullName",
                    Description="Description",
                    Title="Title"
                }
            });
        }

        public override async Task<ApiResult<PostSelectDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            return await base.Get(id, cancellationToken);
        }

        [HttpGet("Test")]
        public ActionResult test()
        {
            return Content("this is test");
        }

        public override Task<ApiResult<PostSelectDto>> Update(Guid id, PostDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }
    }
}