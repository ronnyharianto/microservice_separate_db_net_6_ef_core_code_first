using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Products.Domain.Models.Entities
{
    public class ProductVariant : EntityBase
    {
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public virtual Product? Product { get; set; }
    }
}
