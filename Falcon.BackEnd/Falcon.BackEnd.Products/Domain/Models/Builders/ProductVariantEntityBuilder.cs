using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Products.Domain.Models.Builders
{
    public class ProductVariantEntityBuilder : EntityBaseBuilder<ProductVariant>
    {
        public override void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.VariantName)
                .HasMaxLength(50);

            builder
                .HasOne(e => e.Product)
                .WithMany(e => e.ProductVariants)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            DataSeeding(builder);
        }

        private static void DataSeeding(EntityTypeBuilder<ProductVariant> builder)
        {
            var variantc30620ac7b94 = new List<string>() { "Original", "Berah Merah", "Kacang Hijau", "Jeruk", "Pisang", "Apel Jeruk" };
            var variant8eb86338560f = new List<string>() { "Berah Merah", "Pisang", "Sup Ayam Wortel", "Sup Daging Brokoli", "Tim Hati Ayam" };
            var variantc8b3a4b8eaad = new List<string>() { "Pisang", "Kacang Hijau", "Beras Merah", "Multigrain" };

            foreach (var item in variantc30620ac7b94)
            {
                builder
                    .HasData(new ProductVariant()
                    {
                        ProductId = new Guid("420b5665-c03e-41e1-b591-c30620ac7b94"),
                        VariantName = item
                    });
            }

            foreach (var item in variant8eb86338560f)
            {
                builder
                    .HasData(new ProductVariant()
                    {
                        ProductId = new Guid("05ed93bf-c16c-4572-980d-8eb86338560f"),
                        VariantName = item
                    });
            }

            foreach (var item in variantc8b3a4b8eaad)
            {
                builder
                    .HasData(new ProductVariant()
                    {
                        ProductId = new Guid("0a5c9473-7628-43ed-ae94-c8b3a4b8eaad"),
                        VariantName = item
                    });
            }
        }
    }
}
