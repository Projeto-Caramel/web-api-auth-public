using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl
{
    public interface IPartnersApiService
    {
        Task<Partner> GetSingleOrDefaultByIdAsync(string id);
        Task<Partner> GetPartnerByEmailAsync(string email);
        Task<Partner> RegisterPartnerAsync(PartnerRegistrationApiRequest request);
        Task<Partner> UpdatePartnerPassword(string id, PartnerUpdatePasswordApiRequest request);
    }
}
