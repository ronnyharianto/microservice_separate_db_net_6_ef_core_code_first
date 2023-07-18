using Falcon.BackEnd.APIGateway.Domain.Model.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.APIGateway.Domain.Model.Builders
{
    public class LoginEntityBuilder : EntityBaseBuilder<Login>
    {
        public override void Configure(EntityTypeBuilder<Login> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Token)
                .HasMaxLength(2000);

            builder
                .Property(e => e.ExpiredDate)
                .HasColumnType("timestamp");
        }
    }
}
