﻿using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Enums;
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
        private ILogger _logger;

        public SecurityController(ILogger<SecurityController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public ObjectResult<string> Login(LoginInput input)
        {
            _logger.LogInformation("Start Login Process");

            var retVal = new ObjectResult<string>(ServiceResultCode.BadRequest);

            if (input.UserName == "username" && input.Password == "password")
            {
                var key = Encoding.ASCII.GetBytes(SecurityConstants.Key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, input.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, input.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(SecurityConstants.ExpiryInDays),
                    Issuer = SecurityConstants.Issuer,
                    Audience = SecurityConstants.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                retVal.Obj = tokenHandler.WriteToken(token);
                retVal.OK(null);

                _logger.LogInformation("End Login Process - Success");
                return retVal;
            }

            retVal.UnAuthorized("Please check username / password");

            _logger.LogInformation("End Login Process - UnAuthorized");
            return retVal;
        }
    }
}
