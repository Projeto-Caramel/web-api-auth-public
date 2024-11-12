using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;

namespace Caramel.Pattern.Services.Domain.Services.Adopters
{
    public interface IAdoptersService
    {
        Task<Adopter> RegisterAsync(Adopter entity);
        Task<Adopter> ValidateLoginAsync(string email, string password);
        Task<Adopter> UpdatePasswordAsync(string id, string newPassword);
        Task SendInterestEmailAsync(AdopterInfos adopterInfos, PetInfos petInfos, string partnerId);
    }
}
