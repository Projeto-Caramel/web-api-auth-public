using Caramel.Pattern.Services.Domain.Entities.Models.Partners;

namespace Caramel.Pattern.Services.Domain.Services.Partners
{
    public interface IPartnerService
    {
        Task<Partner> RegisterAsync(Partner entity);
        Task<Partner> ValidateLoginAsync(string email, string password);
        Task<Partner> UpdatePasswordAsync(string id, string newPassword);
    }
}
