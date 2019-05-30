using System;
using System.IO;
using AutoMapper;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Repositories;
using GlodnyStudent.Models.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace GlodnyStudent
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
            {
                _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");
            }

            services.AddDbContext<ApplicationDbContext>(
                options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), opts => opts.UseNetTopologySuite()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddTransient<ICuisineRepository, CuisineRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IMenuItemRepository, MenuItemRepository>();
            services.AddTransient<IRestaurantAddressRepository, RestaurantAddressRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            //services.AddSingleton<IRestaurantRepository, MookRestaurantRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "UploadImages")),
                RequestPath = "/UploadImages"
            });

            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
