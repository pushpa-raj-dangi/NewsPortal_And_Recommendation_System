using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsWebApp.Data;
using NewsWebApp.Helpers.Services;
using NewsWebApp.Hubs;
using NewsWebApp.Models;
using NewsWebApp.Repositories;
using NewsWebApp.Services;

namespace NewsWebApp
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


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)).EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine));

            services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IRepository<Post>, PostRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Tag>, TagRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<AppUser>, UserRepository>();
            services.AddScoped<IRepository<Stats>, StatsRepository>();


            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IUriService>(o =>
                {
                    var accessor = o.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(uri);
                });

            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddRazorPages();
            services.AddSignalR();




        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseResponseCaching();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<VisitorsHub>("/chatHub");
                endpoints.MapHub<HitCounter>("/online");
                endpoints.MapHub<UrlHub>("/url");


            });




        }
    }
}
