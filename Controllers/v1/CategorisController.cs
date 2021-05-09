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
    [ApiVersion("1")]
    public class CategorisController : CrudController<CategoryDto, Category>
    {
        public CategorisController(IRepository<Category> repository) : base(repository)
        {
        }
    }
}