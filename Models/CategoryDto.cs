using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Api.Models
{
    public class CategoryDto :BaseDto<CategoryDto,Category>//دوتا اورلود داره BaseDto 
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } //=> mapped from ParentCategory.Name
    }
}
