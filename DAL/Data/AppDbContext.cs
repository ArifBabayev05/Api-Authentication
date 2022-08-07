using System;
using DAL.Entities;
using DAL.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Product> Products{ get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductImage> ProductsImages { get; set; }
    }
}

