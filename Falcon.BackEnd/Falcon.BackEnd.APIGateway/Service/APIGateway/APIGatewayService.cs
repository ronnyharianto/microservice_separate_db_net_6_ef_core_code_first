using AutoMapper;
using Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels;
using Falcon.BackEnd.APIGateway.Controllers.APIGateway.Inputs;
using Falcon.BackEnd.APIGateway.Domain;
using Falcon.BackEnd.APIGateway.loggingmiddleware;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Falcon.BackEnd.APIGateway.Service.APIGateway
{
    public class APIGatewayService : BaseService<ApplicationDbContext>
    {
        private readonly APIGatewaySetting _aPIGatewaySetting;
        private readonly JsonHelper _jsonHelper;
        public APIGatewayService(ApplicationDbContext dbContext, IMapper mapper, JsonHelper jsonHelper, IOptions<APIGatewaySetting> settings) : base(dbContext, mapper)
        {
            _aPIGatewaySetting = settings.Value;
            _jsonHelper = jsonHelper;
        }

        public async Task<ObjectResult<object>> Login(LoginInput input)
        {
            var retVal = new ObjectResult<object>(ServiceResultCode.BadRequest);

            HttpClient client = new HttpClient();

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_aPIGatewaySetting.ClientId}:{_aPIGatewaySetting.ClientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var formData = new Dictionary<string, string>
            {
                { "grant_type", _aPIGatewaySetting.GrantType}
            };

            // Encode the form data as x-www-form-urlencoded
            var content = new FormUrlEncodedContent(formData);

            // Send the POST request with the form data
            var responseToken = await client.PostAsync("https://apigw.kalbenutritionals.com/token", content);

            // Read the response
            var responseContent = await responseToken.Content.ReadAsStringAsync();

            GetTokenDto? responseObject = JsonConvert.DeserializeObject<GetTokenDto>(responseContent);

            HttpClient clientLogin = new HttpClient();

            if (responseObject != null)
            {
                clientLogin.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseObject.access_token);
            }

            var data = new
            {
                objRequestData = new 
                {
                    TxtPassword = input.UserName,
                    TxtPegawaiID = input.Password
                }
            };

            var json = _jsonHelper.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var responselogin = await clientLogin.PostAsync("https://apigw.kalbenutritionals.com/t/kalbenutritionals.com/prm/v2/VisitPlanAPI/LogIn_J", httpContent);

            var responseLoginContent = await responselogin.Content.ReadAsStringAsync();

            var responseLoginObject = JsonConvert.DeserializeObject(responseLoginContent);

            if (responseLoginContent != null)
            {
                retVal.Obj = responseLoginObject;
                retVal.OK("succeced");
            }

            return retVal;
        }
    }
}
