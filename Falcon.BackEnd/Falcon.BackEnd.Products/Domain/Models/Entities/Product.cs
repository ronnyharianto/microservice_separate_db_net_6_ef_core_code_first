using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Products.Domain.Models.Entities
{
    public class Product : EntityBase
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public decimal Price { get; set; } = 0;
        public DateTime ProductValidTo { get; set; }
        public virtual List<ProductVariant> ProductVariants { get; set; } = new();
    }
}
