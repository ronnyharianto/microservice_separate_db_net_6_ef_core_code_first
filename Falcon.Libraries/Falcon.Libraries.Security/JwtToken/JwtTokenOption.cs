using Falcon.Libraries.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Falcon.Libraries.Security.JwtToken
{
    public static class JwtTokenOption
    {
        public static Action<JwtBearerOptions> OptionValidation = x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = SecurityConstant.Issuer,
                ValidAudience = SecurityConstant.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityConstant.Key)),
                ValidateIssuerSigningKey = true,
            };
        };
    }
}
