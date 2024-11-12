using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterAuthenticateRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
