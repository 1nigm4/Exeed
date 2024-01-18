using Exeed.DAL.Repositories;
using Exeed.Domain;
using Exeed.Managers;
using Exeed.Extensions;
using Exeed.Models;
using Exeed.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Exeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new Configuration();
            builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                       .UseLazyLoadingProxies());
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<IEmailSender, EmailService>();
            builder.Services.AddTransient<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<AccountManager>();
            builder.Services.AddTransient<IDesireRepository, DesireRepository>();
            builder.Services.AddTransient<DesireManager>();
            builder.Services.AddTransient<ILikeRepository, LikeRepository>();
            builder.Services.AddTransient<LikeManager>();
            builder.Services.AddTransient<IWinnerRepository, WinnerRepository>();
            builder.Services.AddTransient<WinnerManager>();
            builder.Services.AddScoped<EventManager>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            
            app.RunApplicationAsync().GetAwaiter().GetResult();
        }
    }
}