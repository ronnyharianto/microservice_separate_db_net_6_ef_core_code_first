namespace Falcon.BackEnd.Products.Controllers.Products.Inputs
{
    public class NotifInput
    {
        public List<string> Topic { get; set; } = new List<string>();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
