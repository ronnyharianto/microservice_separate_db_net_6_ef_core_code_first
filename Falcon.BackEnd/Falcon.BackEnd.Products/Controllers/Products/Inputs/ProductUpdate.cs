namespace Falcon.BackEnd.Products.Controllers.Products.Inputs
{
    public class ProductUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public decimal Price { get; set; } = 0;

    }


}
