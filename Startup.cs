using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Data;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services;
using WebFramework.Configuration;
using WebFramework.Midllewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Entities;
using Api.Models;
using AutoMapper;
using WebFramework.CustomMapping;
using Swashbuckle.AspNetCore.Swagger;
using WebFramework.Swagger;
using Services.DataInitializer;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly SiteSettings _siteSetting;

        public Startup(IConfiguration configuration)
         {
            Configuration = configuration;
            //استفاده از خودکار سازی AutoMapper
           AutoMapperConfiguration.InitializeAutoMapper();
            //استفاده از AutoMapper
            //Mapper.Initialize(config =>
            //{
            //    //ReverseMap: تبدیل پست به پست دی تی او و برعکسش
            //    config.CreateMap<Post, PostDto>().ReverseMap()
            //    .ForMember(p=>p.Author,opt=>opt.Ignore())
            //    .ForMember(p=>p.Category,opt=>opt.Ignore());

            //});


            //در appsettings.development.json
            //یک سکشن نوشتیم که مقادیر رو داخلش ریختیم اینجا میگیم برو از اون بخون
            //و چون جیسان هست اونو دیسریالایز کن به یه کلاس
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //از این طریق میتونیم توی همه سازنده های پروژه هامون  
            //SiteSettings
            //رو تزریق کنیم و فقط کافیه با کد زیر اونو درخواست کنیم
            //IOptionsSnapshot<SiteSettings>
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddDbContext(Configuration);

            
            //تنظیمات مربوط به identity
            services.AddCustomeIdentity(_siteSetting.identitySettings);

            services.AddMinimalMvc();

            //برای دسترسی به الماه
            services.AddElmah(Configuration,_siteSetting);


            //برای اعتبار سنجی jwt
            services.AddJwtAuthentication(_siteSetting.JwtSettings);

            //مدیریت ورژن
            services.AddCustomApiVersioning();

            //برای استفاده از Swagger
            services.AddSwager();
            //Autofac
            return services.BuildAutofacServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)//,IServiceProvider serviceProvider)
        {
            //ایجاد داده پیش فرض در دیتابیس
            app.InitializeDataBase();

            //برای استفاده از midlevare که ساختیم
            app.UseCustomExceptionHandler();

            //if (env.IsDevelopment())
            //{
            //    //app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //   // app.UseExceptionHandler();
            //    app.UseHsts();
            //}
            app.UseHsts(env);

            app.UseElmah();

            app.UseHttpsRedirection();

            app.AddSwaggerAndUI();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
