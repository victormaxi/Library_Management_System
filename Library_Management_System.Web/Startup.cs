using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Services.Intrerface;
using Library_Management_System.Web.Services.Manager;
using Library_Management_System_Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Library_Management_System.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(c =>
              {
                  c.LoginPath = "/account/login";
                  c.LogoutPath = "/account/logout";
                  c.ExpireTimeSpan = TimeSpan.FromSeconds(120);
                  c.SlidingExpiration = true;
              });
            services.AddControllersWithViews();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var appsettings = Configuration.GetSection("ApiRequestUri");
            services.Configure<ApiRequestUri>(appsettings);
            services.AddScoped<IResources, Resources>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            services.AddMemoryCache();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                   name: "areas",
                   //pattern: "{controller=Home}/{action=Index}/{id?}",
                   pattern: "{area:exists}/{controller=AdminDashBoard}/{action=Index}/{id?}");
                   //defaults: new { controller = "Home", action = "Index" });
                   //defaults: new { controller = "AdminDashBoard", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}",
                    pattern: "{controller=Account}/{action=Login}/{id?}",
                    //defaults: new { controller = "Home", action = "Index" });
                    defaults: new { controller = "Account", action = "Login" });
               
           
            });
             

            //    app.UseMvc(routes =>
            //    {
            //        routes.MapRoute(
            //            name: "areas",
            //            template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            //          );

            //        routes.MapRoute(
            //            name: "default",
            //            template: "{controller=Home}/{action=Index}/{id?}");

            //    });
        }
    }
}