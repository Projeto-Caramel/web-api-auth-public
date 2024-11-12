using Caramel.Pattern.Services.Domain.Entities.Models.Partners;

namespace Caramel.Pattern.Services.Domain.Services.Partners
{
    public interface IPartnerVerificationCodeService
    {
        Task<Partner> SendConfirmation(string email);
        Task<bool> VerificationCodeValidate(string userId, string code);
    }
}
