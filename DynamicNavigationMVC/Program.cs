using DynamicNavigationMVC.data;
using DynamicNavigationMVC.Services.LayoutService;
using DynamicNavigationMVC.DataFilters;
using Microsoft.EntityFrameworkCore;
using DynamicNavigationMVC.Repository;
using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.Services.CategoryService;
using DynamicNavigationMVC.Services.ProductService;

namespace DynamicNavigationMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<NavDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("devConnection")).EnableSensitiveDataLogging());

            builder.Services.AddScoped<IDataAccess<Category>, CategoryService>();
            builder.Services.AddScoped<IDataAccess<Product>, ProductService>();
            builder.Services.AddScoped<ILayoutDataService, LayoutDataService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<LayoutDataFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDataProtection();

            builder.Services.AddCors(options =>
                    options.AddPolicy("CORSPolicy",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition");
                    }));


            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
