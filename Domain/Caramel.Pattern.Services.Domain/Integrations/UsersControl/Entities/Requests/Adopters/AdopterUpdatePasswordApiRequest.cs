using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterUpdatePasswordApiRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
