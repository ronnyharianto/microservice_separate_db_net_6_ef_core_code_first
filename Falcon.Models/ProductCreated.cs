namespace Falcon.Models
{
    public class ProductCreated
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public List<string> VariantNames { get; set; } = new();
    }
}