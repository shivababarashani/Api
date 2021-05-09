using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filters;

namespace Api.Controllers.v1
{
    //[ApiController]
    //[AllowAnonymous]
    //[ApiResultFilter]
    //[Route("api/[controller]")]
    [ApiVersion("1")]
    public class OldPostsController ://ControllerBase
        BaseController
    {
        private readonly IRepository<Post> _repository;

        public OldPostsController(IRepository<Post> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostSelectDto>>> Get(CancellationToken cancellationToken)
        {

            #region old code
            //var list = await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).ToListAsync(cancellationToken);
            //return Ok(list);
            #endregion
            // var posts=await _repository.TableNoTracking
            //     .Include(p=>p.Category)
            //     .Include(p=>p.Author)
            //     .ToListAsync(cancellationToken);

            //var list= posts.Select(p =>
            // {
            //     var dto = Mapper.Map<PostDto>(p);
            //     return dto;
            // }).ToList();


            //این کد دوتاکار بالا رو باهم انجام میده یعنی خود پست رو میاره  include ها رو هم 
            //نکته پرفورمنسی اینه که همه category ها رو برامون اینکلود نمیکنه
            //بلکه فقط name یا هرچیزی که توی پست نوشتیم رو برامون میاره
            var list = await _repository.TableNoTracking.ProjectTo<PostSelectDto>()
                //.Where(postdto=>postdto.Title.Contains("test") //میشه شرط گذاشت
                //|| postdto.CategoryName.Contains("test"))
                .ToListAsync(cancellationToken);
            return list;

        }

        [HttpGet("{id:guid}")]
        public async Task<ApiResult<PostSelectDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            #region old code
            //var dto = new PostDto
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    Description = model.Description,
            //    CategoryId = model.CategoryId,
            //    AuthorId = model.AuthorId,
            //    AuthorFullName = model.Author.FullName,
            //    CategoryName = model.Category.Name
            //};
            #endregion
            var dto = await _repository.TableNoTracking.ProjectTo<PostSelectDto>()
             .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            //autor , categury رو نمیاره
            //var model = await _repository.GetByIdAsync(cancellationToken, id);
            //var dto= Mapper.Map<PostDto>(model);

            if (dto == null)
                return NotFound();
          
            return dto;
        }

        [HttpPost]
        public async Task<ApiResult<PostSelectDto>> Create(PostDto dto, CancellationToken cancellationToken)
        {
            //var model = Mapper.Map<Post>(dto);
            var model =dto.ToEntity();

            #region old code
            //var model = new Post
            //{
            //    Title = dto.Title,
            //    Description = dto.Description,
            //    CategoryId = dto.CategoryId,
            //    AuthorId = dto.AuthorId
            //};
            #endregion

            await _repository.AddAsync(model, cancellationToken);

            #region old code
            //await _repository.LoadReferenceAsync(model, p => p.Category, cancellationToken);
            //await _repository.LoadReferenceAsync(model, p => p.Author, cancellationToken);
            //روش اول برای گرفتن دوتا نویگیشن بعد از ذخیره
            //model = await _repository.TableNoTracking
            //    .Include(p => p.Category)
            //    .Include(p => p.Author)
            //    .SingleOrDefaultAsync(p => p.Id == model.Id,cancellationToken);

            //روش دوم
            //var resultDto = new PostDto
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    Description = model.Description,
            //    CategoryId = model.CategoryId,
            //    AuthorId = model.AuthorId,
            //    AuthorFullName = model.Author.FullName,
            //    CategoryName = model.Category.Name
            //};

            //روش سوم و بهترین روش برای پرفورمنس
            //var resultDto =  await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            #endregion
            var list = await _repository.TableNoTracking.ProjectTo<PostSelectDto>()
             .SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            return list;
        }

        [HttpPut]
        public async Task<ApiResult<PostSelectDto>> Update(Guid id, PostDto dto, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(cancellationToken, id);

            var postDto= new PostDto();
            #region Entity=>Dto
            //create
            var post = postDto.ToEntity();
            //update
            var updatePost = postDto.ToEntity(model);//وقتی که یه سلکت از قبل موجود داشته باشیم
            #endregion

            #region Dto=>Entity
            //get by id
            var postdto1 = PostDto.FromEntity(model);
            #endregion




            //وقتی بخوایم مپ دقیقا روی model 
            //که از دیتابیس خوندیم اعمال بشه مثل زیر مینویسیم
            //Mapper.Map(dto, model);
            model= dto.ToEntity(model);

            #region old code
            //model.Title = dto.Title;
            //model.Description = dto.Description;
            //model.CategoryId = dto.CategoryId;
            //model.AuthorId = dto.AuthorId;
            #endregion

            await _repository.UpdateAsync(model, cancellationToken);

            #region old code
            //var resultDto = await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            #endregion

            var resultDto = await _repository.TableNoTracking.ProjectTo<PostSelectDto>().SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            return resultDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(cancellationToken, id);
            await _repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }
    }
}