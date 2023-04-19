using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Falcon.BackEnd.Security.Controllers.Security
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SecurityController : Controller
    {
        [HttpPost("login")]
        public ObjectResult<string> Login(LoginInput input)
        {
            var retVal = new ObjectResult<string>();

            if (input.UserName == "username" && input.Password == "password")
            {
                var key = Encoding.ASCII.GetBytes(SecurityConstant.Key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, input.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, input.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(SecurityConstant.ExpiryInDays),
                    Issuer = SecurityConstant.Issuer,
                    Audience = SecurityConstant.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                
                retVal.Obj = tokenHandler.WriteToken(token);
                retVal.OK(null);
                return retVal;
            }

            retVal.UnAuthorized("Please check username / password");
            return retVal;
        }
    }
}
