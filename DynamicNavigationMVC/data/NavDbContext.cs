using DynamicNavigationMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicNavigationMVC.data
{
    public class NavDbContext:DbContext
    {
        public NavDbContext(DbContextOptions options):base(options) 
        { 
        }

        public DbSet<Category> category { get; set; }
        public DbSet<Product> products { get; set; }

    }
}
