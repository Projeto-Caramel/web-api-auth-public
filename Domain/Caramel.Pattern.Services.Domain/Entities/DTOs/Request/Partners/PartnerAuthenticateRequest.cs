using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerAuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
