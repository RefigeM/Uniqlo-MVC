using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniqloTasks.DataAccess;
using UniqloTasks.Models;
using UniqloTasks.Extentions;
using UniqloTasks.Services.Abstractions;
using UniqloTasks.Services.Concretes;
using Microsoft.AspNetCore.Hosting;

namespace UniqloTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UniqloDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
            });
            builder.Services.AddScoped<ISliderService,SliderService>();
			
			builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Lockout.MaxFailedAccessAttempts = 2;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureApplicationCookie(y =>
            {
                y.LoginPath = "/login";
                y.AccessDeniedPath = "/Home/AccessDenied";

            });
            var app = builder.Build();
			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();


			app.UseAuthorization();
            //app.UseUsersSeed();
            app.UseUserSeed();

			app.MapControllerRoute(
			  name: "login",
			  pattern: "login", new
			  {
				  Controller = "Account",
				  Action = "Login"

			  }); app.MapControllerRoute(
			  name: "register",
			  pattern: "register", new
			  {
				  Controller = "Account",
				  Action = "Register"

			  });

			app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
