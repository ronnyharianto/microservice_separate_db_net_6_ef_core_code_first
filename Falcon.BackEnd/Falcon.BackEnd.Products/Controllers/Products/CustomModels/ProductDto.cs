using Falcon.BackEnd.Products.Domain.Models.Entities;

namespace Falcon.BackEnd.Products.Controllers.Products.CustomModels
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public decimal Price { get; set; } = 0;
        public DateTime ProductValidTo { get; set; }
        public virtual List<ProductVariantDto> ProductVariants { get; set; } = new();
    }
}
