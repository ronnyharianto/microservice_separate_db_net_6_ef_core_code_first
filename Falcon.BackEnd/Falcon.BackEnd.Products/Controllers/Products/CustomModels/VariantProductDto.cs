namespace Falcon.BackEnd.Products.Controllers.Products.CustomModels
{
    public class VariantProductDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
    }
}
