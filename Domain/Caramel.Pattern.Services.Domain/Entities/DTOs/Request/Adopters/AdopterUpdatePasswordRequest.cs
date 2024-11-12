using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterUpdatePasswordRequest
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
