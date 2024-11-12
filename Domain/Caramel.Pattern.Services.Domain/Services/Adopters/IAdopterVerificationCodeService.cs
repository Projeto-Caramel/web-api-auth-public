namespace Caramel.Pattern.Services.Domain.Services.Adopters
{
    public interface IAdopterVerificationCodeService
    {
        Task SendConfirmation(string email);
        Task<bool> VerificationCodeValidate(string email, string code);
    }
}
