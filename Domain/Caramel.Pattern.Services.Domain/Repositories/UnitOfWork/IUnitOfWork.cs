namespace Caramel.Pattern.Services.Domain.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Adopters { get; }
        IUserVerificationCodeRepository AdoptersVerificationCodes { get; }
        IPartnerRepository Partners { get; }
        IPartnerVerificationCodeRepository PartnerVerificationCodes { get; }
    }
}
