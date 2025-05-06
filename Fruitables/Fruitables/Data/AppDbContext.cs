using Fruitables.Models;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
