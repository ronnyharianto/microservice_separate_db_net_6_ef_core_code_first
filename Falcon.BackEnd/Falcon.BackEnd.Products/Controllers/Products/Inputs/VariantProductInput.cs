namespace Falcon.BackEnd.Products.Controllers.Products.Inputs
{
    public class VariantProductInput
    {
        public Guid ProductId { get; set; }
        public List<ProductVariantInput> ProductVariants { get; set; } = new();
    }
}
