namespace Falcon.BackEnd.Products.Controllers.Products.Update
{
	public class ProductUpdate
	{
		public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? Remark { get; set; }
		public decimal Price { get; set; } = 0;

	}

	
}
