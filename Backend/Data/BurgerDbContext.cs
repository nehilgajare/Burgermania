using Burgermania.Models;
using Microsoft.EntityFrameworkCore;

namespace Burgermania.Data
{
    public class BurgerDbContext:DbContext
    {
        public BurgerDbContext(DbContextOptions<BurgerDbContext> options) : base(options)
        {
            
        }
        public DbSet<Burger> Burgers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.Burger)
        //        .WithMany(b => b.Orders)
        //        .HasForeignKey(o => o.BurgerId);

        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.User)
        //        .WithMany(u => u.Orders)
        //        .HasForeignKey(o => o.UserId);
        //}
    }
}
