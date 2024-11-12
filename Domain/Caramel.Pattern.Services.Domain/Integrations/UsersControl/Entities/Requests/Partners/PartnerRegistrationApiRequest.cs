using Caramel.Pattern.Services.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerRegistrationApiRequest
    {
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public OrganizationType Type { get; set; }
        public Roles Role { get; set; }
        public int MaxCapacity { get; set; }
    }
}
