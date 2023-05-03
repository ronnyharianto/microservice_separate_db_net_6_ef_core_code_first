using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.Libraries.DataAccess.Domain
{
    public class EntityBaseBuilder<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(e => e.Created)
                .HasColumnType("timestamp without time zone");

            builder
                .Property(e => e.Modified)
                .HasColumnType("timestamp without time zone");

            builder
                .HasQueryFilter(e => e.RowStatus == 0);
        }
    }
}
