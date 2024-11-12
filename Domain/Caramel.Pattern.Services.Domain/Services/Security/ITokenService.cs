using Caramel.Pattern.Services.Domain.Entities.Auth;
using Caramel.Pattern.Services.Domain.Enums;

namespace Caramel.Pattern.Services.Domain.Services.Security
{
    public interface ITokenService
    {
        TokenModel GenerateJwtTokenAsync(string id, string name, Roles role, string profileImage);
        string GenerateIssuerJwtTokenAsync();
    }
}
