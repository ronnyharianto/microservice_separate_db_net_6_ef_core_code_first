namespace Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels
{
    public class LoginDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; } = DateTime.Now;
    }
}
