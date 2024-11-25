using Microsoft.EntityFrameworkCore;
using System;

namespace CaloriesCounter.DAL
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<FoodEntry> FoodEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<FoodEntry>()
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }


    public class FoodEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public DateTime Date { get; set; }
    }
}
