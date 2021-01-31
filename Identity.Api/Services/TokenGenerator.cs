using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Identity.Api.Core;
using IdentityModel;
using Microservices.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(ICollection<Claim> claims, DateTime time);
        string GenerateToken(ICollection<Claim> claims, IEnumerable<string> roles, DateTime time);
    }
    public class TokenGenerator:ITokenGenerator
    {
        private readonly AppSettings _appSettings;
        public TokenGenerator(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateToken(ICollection<Claim> claims, DateTime time)
        {
            return Generate(claims.ToArray(), time);
        }
        public string GenerateToken(ICollection<Claim> claims, IEnumerable<string> roles, DateTime time)
        {
            claims.AddRange(roles.Select(role=> new Claim(JwtClaimTypes.Role,role)));
            return Generate(claims.ToArray(), time);
        }

        private string Generate(Claim[] claims, DateTime time)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Authentication.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = time,
                //Audience = "identity",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
