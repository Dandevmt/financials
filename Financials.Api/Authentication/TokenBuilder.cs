using Financials.Application.Security.UseCases;
using Financials.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Api.Authentication
{
    public class TokenBuilder : ITokenBuilder
    {
        public string Build(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT:KEY"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("JWT:Issuer",
              "Jwt:Issuer",
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
