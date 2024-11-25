using CaloriesCounter.BLL;
using Microsoft.EntityFrameworkCore;

namespace CaloriesCounter

{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CaloriesCounterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("your-connection-string");

        }
    }

}
