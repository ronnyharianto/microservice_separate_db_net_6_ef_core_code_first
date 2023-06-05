using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Showcases.Domain.Models.Builders
{
    public class ProductVariantViewModelBuilder : ViewModelBaseBuilder<ProductVariantViewModel>
    {
        public override void Configure(EntityTypeBuilder<ProductVariantViewModel> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.ProductId);

            builder
                .Property(e => e.VariantName)
                .HasMaxLength(50);
        }
    }
}
