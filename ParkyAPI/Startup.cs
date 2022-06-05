using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();


            //These are the services related to Automapper & Swagger.
            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            //services.AddSwaggerGen(
            //    options => {
            //        options.SwaggerDoc("ParkyOpenAPISpec", new OpenApiInfo()
            //        {
            //            Title = "Parky API",
            //            Version = "1",
            //            Description = "Udemy Parky API",
            //            Contact = new OpenApiContact()
            //            {
            //                Email = "rajat121237@gmail.com",
            //                Name = "Rajat Pandey",
            //                Url = new Uri("https://www.google.co.in/")
            //            },
            //            License = new OpenApiLicense()
            //            {
            //                Name = "MIT License",
            //                Url = new Uri("https://www.google.co.in/")
            //            }
            //        });

            //        //Second Swagger Doc
            //        //options.SwaggerDoc("ParkyOpenAPISpecTrails", new OpenApiInfo()
            //        //{
            //        //    Title = "Parky API (Trails)",
            //        //    Version = "1",
            //        //    Description = "Udemy Parky API (Trails)",
            //        //    Contact = new OpenApiContact()
            //        //    {
            //        //        Email = "rajat121237@gmail.com",
            //        //        Name = "Rajat Pandey",
            //        //        Url = new Uri("https://www.google.co.in/")
            //        //    },
            //        //    License = new OpenApiLicense()
            //        //    {
            //        //        Name = "MIT License",
            //        //        Url = new Uri("https://www.google.co.in/")
            //        //    }
            //        //});



            //        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //        options.IncludeXmlComments(xmlCommentsFullPath);
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(options => {
                foreach (var desc in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                options.RoutePrefix = "";
            });


            //app.UseSwaggerUI(options => {
            //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
            //    options.RoutePrefix = "";
            //});

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
