using Microsoft.EntityFrameworkCore;

namespace WebBookShop.Models
{
   public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<FavoritesPurchases> favoritesPurchases { get; set; }
        public DbSet<Discounts> discounts { get; set; }
        public DbSet<QuantityAndSales> quantityAndSales { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
