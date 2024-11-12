using Caramel.Pattern.Services.Domain.Entities.Auth;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Caramel.Pattern.Services.Application.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenModel GenerateJwtTokenAsync(string id, string name, Roles role, string profileImage)
        {
            var claims = CreateClaims(id, name, role);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddDays(10),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                claims: claims
            );

            return new TokenModel()
            {
                Id = id,
                Name = name,
                ProfileImageUrl = profileImage,
                Role = role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }

        public string GenerateIssuerJwtTokenAsync()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                claims: CreateClaims(Roles.Admin)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] CreateClaims(string id, string name, Roles role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            return claims;
        }

        private Claim[] CreateClaims(Roles role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Key"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            return claims;
        }
    }
}
