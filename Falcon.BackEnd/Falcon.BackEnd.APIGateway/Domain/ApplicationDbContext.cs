
using Falcon.BackEnd.APIGateway.Domain.Model.Builders;
using Falcon.BackEnd.APIGateway.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.APIGateway.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<Login> Login { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("apigateway");

            new LoginEntityBuilder().Configure(modelBuilder.Entity<Login>());
        }
    }
}
