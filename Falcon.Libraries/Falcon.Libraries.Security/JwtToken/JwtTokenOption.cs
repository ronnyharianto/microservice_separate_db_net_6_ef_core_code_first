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
                ValidIssuer = SecurityConstants.Issuer,
                ValidAudience = SecurityConstants.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityConstants.Key)),
                ValidateIssuerSigningKey = true,
            };
        };
    }
}
