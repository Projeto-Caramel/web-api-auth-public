using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerUpdatePasswordRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
