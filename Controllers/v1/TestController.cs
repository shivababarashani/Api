using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using WebFramework.Api;

namespace Api.Controllers.v1
{
    #region Swagger Annotations
    public class AddressDto
    {
        /// <summary>
        /// 3-letter ISO country code
        /// </summary>
        /// <example>Iran</example>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Name of city
        /// </summary>
        /// <example>Seattle</example>
        [DefaultValue("Seattle")]
        public string City { get; set; }

        [JsonProperty("promo-code")]
        public string Code { get; set; }

        [JsonIgnore]
        public int Discount { get; set; }
    }

    //فرمت کامل ایجاد کامنت هماهنگ با swagger
    ///// <summary>
    ///// Retrieves a specific product by unique id
    ///// </summary>
    ///// <param name="param1">Parameter 1 description</param>
    ///// <param name="param2">Parameter 2 description</param>
    ///// <param name="param3">Parameter 2 description</param>
    ///// <remarks>Awesomeness!</remarks>
    ///// <response code="200">Product created</response>
    ///// <response code="400">Product has missing/invalid values</response>
    ///// <response code="500">Oops! Can't create your product right now</response>
    //[HttpGet("Test")]
    //public ActionResult Test(/*
    //        IFormFile file, ...
    //        [FromQuery] Address address, ...
    //        [FromForm] parameter, ...
    //        [FromBody] parameter, ...
    //        [Required] parameter, ... */)
    //{
    //    throw new NotImplementedException();
    //}
    #endregion

    [ApiVersion("1")]
    public class TestController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult UploadFile1(IFormFile file)
        {
            //Request.Form.Files
            return Ok();
        }
        [AllowAnonymous]
        [AddSwaggerFileUploadButton]
        [HttpPost("[action]")]
        public ActionResult UploadFile2()
        {
           var file= Request.Form.Files[0];
            return Ok();
        }


        #region Action Annotations
        //Specific request content type
        //این باعث میشه ورودی ها با این حالت فرستاده شده در غیر اینصورت کار نمیکنه
        //[Consumes("application/json")]
        //Specific response content type
        //مقدار خروجی اکشن رو تعریف میکنیم در غیراینصورت کار نمیکنه
        //[Produces("application/json")]

        //حالت های خروجی ممکنه اکشن
        //Specific response http status codes
        //[ProducesResponseType(200)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[SwaggerResponse(200)]
        //[SwaggerResponse(StatusCodes.Status200OK)]

            //مشابه حالت های بالا داره با این تفاوت که میتوان یک سری جزئیات اضافه کنیم
        //Specific response type & description
        //[ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status200OK, type: typeof(IEnumerable<UserDto>))]
        //[SwaggerResponse(StatusCodes.Status200OK, "my custom descriptions", type: typeof(IEnumerable<UserDto>))]

        //[SwaggerOperation(OperationId = "CreateCart")]
        //[SwaggerOperation(OperationId = "DeleteCart", Summary = "Deletes a specific cart", Description = "Requires admin privileges")]
        //[SwaggerOperationFilter(typeof(MyCustomIOperationFilter))]
        //[SwaggerTag("Manipulate Carts to your heart's content", "http://www.tempuri.org")]
        #endregion

        [AllowAnonymous]
        //ایجاد مقدار نمونه ورودی
        [SwaggerRequestExample(typeof(UserDto),typeof(CreateUserRequestExample))]
        //ایجاد مقدار نمونه خروجی اگه اکشن با موفقیت انجام شد
        [SwaggerResponseExample(200,typeof(CreateUserResponceExample))]
        [HttpPost("[action]")]
        public ActionResult CreateUser(UserDto userDto)
        {
            return Ok();
        }
        /// <summary>
        /// Assign an address to user 
        /// </summary>
        /// <param name="address">Address of User</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Address created</response>
        /// <response code="400">Address has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your Address right now</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult Address(AddressDto address)
        {
            return Ok();
        }
    }
    //ایجاد یک داده نمونه در زمان افزودن
    public class CreateUserRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new UserDto
            {
                FullName = "محمدجواد ابراهیمی",
                Age = 25,
                UserName = "mjebrahimi",
                Email = "admin@site.com",
                Gender = Entities.GenderType.Male,
                Password = "1234567"
            };
            }
    }

    //ایجاد یک داده نمونه برای خروجی
    public class CreateUserResponceExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new UserDto
            {
                FullName = "محمدجواد ابراهیمی",
                Age = 25,
                UserName = "mjebrahimi",
                Email = "admin@site.com",
                Gender = Entities.GenderType.Male,
                Password = "1234567"
            };
        }
    }
}