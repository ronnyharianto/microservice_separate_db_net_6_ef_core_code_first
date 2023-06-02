using Falcon.BackEnd.Showcases.Domain.Models.Builders;
using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Showcase.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<ProductViewModel> ProductViewModels { get; set; }
        public virtual DbSet<ProductVariantViewModel> ProductVariantViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("showcase");

            new ProductViewModelBuilder().Configure(modelBuilder.Entity<ProductViewModel>());
            new ProductVariantViewModelBuilder().Configure(modelBuilder.Entity<ProductVariantViewModel>());
        }
    }
}
