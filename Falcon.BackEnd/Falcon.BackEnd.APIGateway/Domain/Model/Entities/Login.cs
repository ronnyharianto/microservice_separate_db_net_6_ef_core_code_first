using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.APIGateway.Domain.Model.Entities
{
    public class Login : EntityBase
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; } = DateTime.Now;
    }
}
