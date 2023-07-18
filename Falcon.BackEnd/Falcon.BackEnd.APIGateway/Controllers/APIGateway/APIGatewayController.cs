using Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels;
using Falcon.BackEnd.APIGateway.Controllers.APIGateway.Inputs;
using Falcon.BackEnd.APIGateway.Service.APIGateway;
using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.BackEnd.APIGateway.Controllers.APIGateway
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class APIGatewayController : Controller
    {
        private readonly APIGatewayService _aPIGatewayService;

        public APIGatewayController(APIGatewayService aPIGatewayService)
        {
            _aPIGatewayService = aPIGatewayService;
        }

        [HttpPost("login")]
        public async Task<ObjectResult<object>> Login(LoginInput input)
        {
            var retVal = await _aPIGatewayService.Login(input);

            return retVal;
        }
    }
}
