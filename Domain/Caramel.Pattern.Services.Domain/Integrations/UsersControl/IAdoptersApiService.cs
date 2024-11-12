using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl
{
    public interface IAdoptersApiService
    {
        Task<Adopter> GetSingleOrDefaultByIdAsync(string id);
        Task<Adopter> GetAdopterByEmailAsync(string email);
        Task<Adopter> RegisterAdopterAsync(AdopterRegistrationApiRequest request);
        Task<Adopter> UpdateAdopterPassword(string id, AdopterUpdatePasswordApiRequest request);
    }
}
