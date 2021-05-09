using Api.Models;
using Common.Api;
using Common.Exceptions;
using Data.Repositories;
using ElmahCore;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;
using Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Api.Controllers.v1
{
    //[Route("api/[controller]")]
    //[ApiResultFilter]
    //[ApiController]
    [ApiVersion("1")]
    public class UserController :// ControllerBase
        BaseController
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;

        public UserController(IUserRepository userRepository,ILogger<UserController> logger,IJwtService jwtService,UserManager<User>userManager,RoleManager<Role> roleManager,SignInManager<User> signInManager)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// لیست کامل کاربران
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiResultFilter]
        [Authorize(Roles ="Admin")]
        public virtual async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity.GetUserName();
            var userId = HttpContext.User.Identity.GetUserId();
            var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;

        }
        /// <summary>
        /// this method generate jwt token
        /// </summary>
        /// <param name="tokenRequest">the information of token</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        //[Route("[action]"),HttpPost,HttpGet]//وقتایی که بخوایم هردوش باشه
        [HttpPost("[action]")]
        [AllowAnonymous]//به احراز هویت نیاز ندارد
        public virtual async Task<ActionResult> Token([FromForm] TokenRequest tokenRequest,CancellationToken cancellationToken)
        {
            if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password");


            //var user=await  userRepository.GetByUserAndPass(userName, password, cancellationToken);
            var user = await userManager.FindByNameAsync(tokenRequest.username);
            if (user==null)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

           var IsPassValid=await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!IsPassValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt=await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {

            var user2=await userManager.FindByIdAsync(id.ToString());
            userManager.GetUserAsync(HttpContext.User);
            var role = await roleManager.FindByNameAsync("Admin");
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            userRepository.UpdateSecurityStampAsync(user, cancellationToken);
            return user;
            //var result = new ApiResult<User>
            //{
            //    IsSuccess = true,
            //    StatusCode = ApiResultStatusCode.Success,
            //    Message = "عملیات با موفقیت انجام شد",
            //    Data = user
            //};
            //return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            logger.LogError("");
            HttpContext.RiseError(new Exception("متد Create"));
            //var exist=await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            // if (exist)
            // {
            //     return BadRequest("نام کاربری تکراری است");
            // }
            var user = new User
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName,
                Email=userDto.Email
            };
           var result=await userManager.CreateAsync(user, userDto.Password);

           var result2=await roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Description = "admin role",
            });

           var result3=await userManager.AddToRoleAsync(user, "Admin");
            return user;
            //await userRepository.AddAsync(user,userDto.Password, cancellationToken);
           // return user;
            //return new ApiResult(true, ApiResultStatusCode.Success);

            //var result = new ApiResult
            //{
            //    IsSuccess = true,
            //    StatusCode = ApiResultStatusCode.Success,
            //    Message = "عملیات با موفقیت انجام شد"
            //};
            //return result;
        }

        [HttpPut]
        public virtual async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);
            return new ApiResult(true, ApiResultStatusCode.Success);
            //var result = new ApiResult
            //{
            //    IsSuccess = true,
            //    StatusCode = ApiResultStatusCode.Success,
            //    Message = "عملیات با موفقیت انجام شد"
            //};
            //return result;
        }

        [HttpDelete]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);
            return new ApiResult(true, ApiResultStatusCode.Success);

        }
    }
}