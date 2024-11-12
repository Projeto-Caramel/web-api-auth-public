using Caramel.Pattern.Services.Application.Services.Security;
using Caramel.Pattern.Services.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Caramel.Services.Pattern.Tests.Application.Security
{
    [ExcludeFromCodeCoverage]
    public class TokenServiceTest
    {
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _configurationMock;

        public TokenServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("89okger+rey=gb++ertgbhd4(GFhfg=fgh)ht08+");

            _tokenService = new TokenService(_configurationMock.Object);
        }

        [Fact]
        public void GenerateJwtTokenAsync_Success()
        {
            var userId = "123";
            var userName = "testuser";
            var userRole = Roles.Admin;
            var profileImage = "teste";

            var tokenModel = _tokenService.GenerateJwtTokenAsync(userId, userName, userRole, profileImage);

            Assert.NotNull(tokenModel);
            Assert.Equal(userId, tokenModel.Id);
            Assert.Equal(userName, tokenModel.Name);
            Assert.Equal(userRole, tokenModel.Role);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenModel.Token) as JwtSecurityToken;

            Assert.NotNull(jsonToken);
            Assert.Contains(jsonToken.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
            Assert.Contains(jsonToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userName);
            Assert.Contains(jsonToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == userRole.ToString());
        }

        [Fact]
        public void GenerateIssuerJwtTokenAsync_Success()
        {
            var token = _tokenService.GenerateIssuerJwtTokenAsync();

            Assert.NotNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.NotNull(jsonToken);
        }
    }

}
