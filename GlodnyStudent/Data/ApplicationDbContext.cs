using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AddressCoordinates> AddressCoordinates { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantAddress> RestaurantAddresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
