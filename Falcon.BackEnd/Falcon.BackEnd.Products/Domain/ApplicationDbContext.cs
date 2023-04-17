using Falcon.BackEnd.Products.Domain.Models.Builders;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Products.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("product");

            new ProductEntityBuilder().Configure(modelBuilder.Entity<Product>());
            new ProductVariantEntityBuilder().Configure(modelBuilder.Entity<ProductVariant>());
        }
    }
}
