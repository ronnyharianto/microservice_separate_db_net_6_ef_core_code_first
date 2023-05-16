namespace Falcon.Models.Topics
{
    public class ProductUpdated
    {
		public Guid ProductId { get; set; }
		//public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? Remark { get; set; }
	}
}