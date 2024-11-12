using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;

namespace Caramel.Pattern.Services.Infra.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        public IUserRepository Adopters { get; }
        public IUserVerificationCodeRepository AdoptersVerificationCodes { get; }
        public IPartnerRepository Partners { get; }
        public IPartnerVerificationCodeRepository PartnerVerificationCodes { get; }

        public UnitOfWork(
            MongoDbContext context,
            IUserRepository users,
            IUserVerificationCodeRepository userVerificationCodes,
            IPartnerRepository partners,
            IPartnerVerificationCodeRepository partnerVerificationCodes)
        {
            _context = context;
            Adopters = users;
            AdoptersVerificationCodes = userVerificationCodes;
            Partners = partners;
            PartnerVerificationCodes = partnerVerificationCodes;
        }
    }
}
