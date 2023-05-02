using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Showcases.Domain.Models.Builders
{
    public class ProductViewModelBuilder : ViewModelBaseBuilder<ProductViewModel>
    {
        public override void Configure(EntityTypeBuilder<ProductViewModel> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Code)
                .HasMaxLength(10);

            builder
                .Property(e => e.Name)
                .HasMaxLength(50);

            builder
                .Property(e => e.Remark)
                .HasMaxLength(500);

            DataSeeding(builder);
        }

        private static void DataSeeding(EntityTypeBuilder<ProductViewModel> builder)
        {
            builder
                .HasData(new ProductViewModel()
                {
                    Id = new Guid("420b5665-c03e-41e1-b591-c30620ac7b94"),
                    Code = "PRD-001",
                    Name = "Milna Biskuit Bayi",
                    Remark = "Milna Biskuit Bayi adalah biskuit kaya gizi sebagai pengenalan makanan padat pertama yang teksturnya lembut sehingga tidak mudah tersedak. Milna Biskuit Bayi juga dilengkapi dengan AA DHA, Tinggi Kalsium, dan Prebiotik Inulin untuk kecerdasan dan kesehatan pencernaan Si Kecil.",
                });

            builder
                .HasData(new ProductViewModel()
                {
                    Id = new Guid("05ed93bf-c16c-4572-980d-8eb86338560f"),
                    Code = "PRD-002",
                    Name = "Milna Bubur Bayi 6 - 12 Bulan",
                    Remark = "Milna Bubur Bayi adalah bubur bayi lengkap gizi dengan komposisi 4 sehat 5 sempurna untuk mendukung tumbuh kembang optimal Si Kecil. Sangat cocok sebagai makanan pendamping ASI pertama saat Si Kecil memasuki usia 6 bulan.",
                });

            builder
                .HasData(new ProductViewModel()
                {
                    Id = new Guid("0a5c9473-7628-43ed-ae94-c8b3a4b8eaad"),
                    Code = "PRD-003",
                    Name = "Milna Bubur Organic",
                    Remark = "Milna Bubur Organic adalah bubur bayi lengkap gizi dengan bahan organik untuk pengenalan makanan padat pertama bayi.",
                });
        }
    }
}
