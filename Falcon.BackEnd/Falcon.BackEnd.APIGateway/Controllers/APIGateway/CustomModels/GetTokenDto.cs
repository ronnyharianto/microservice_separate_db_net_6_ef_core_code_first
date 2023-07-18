namespace Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels
{
    public class GetTokenDto
    {
        public string access_token { get; set; } = string.Empty; 
        public int expires_in { get; set; }
    }
}
