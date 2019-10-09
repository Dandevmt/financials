using Financials.Application.Configuration;
using Financials.Application.Security;
using Financials.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Api.Authentication
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly AppSettings appSettings;

        public TokenBuilder(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public string Build(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.TokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(appSettings.TokenIssuer,
              appSettings.TokenAudience,
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
