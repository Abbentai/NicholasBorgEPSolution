using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<PollDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PollDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllers(options =>
            {
                //options.Filters.Add<LoginVoteActionFilter> (); // Add the filter to all controllers
            });


            //Comment the other out to switch which repository to use
            //builder.Services.AddSingleton<IPollRepository, PollFileRepository>();
            builder.Services.AddScoped<IPollRepository, PollRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
