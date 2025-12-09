using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Repo.Data.Config;

namespace Talabat.Repo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Catogary> Catogaries  { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethod { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
    .OwnsOne(o => o.ShippingAddress, a =>
    {
        a.Property(p => p.FirstName).HasMaxLength(100);
        a.Property(p => p.LastName).HasMaxLength(100);
        a.Property(p => p.City).HasMaxLength(100);
        a.Property(p => p.Country).HasMaxLength(90);
        a.Property(p => p.Street).HasMaxLength(180);
    });
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
