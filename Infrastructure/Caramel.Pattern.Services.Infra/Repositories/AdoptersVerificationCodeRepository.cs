using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public class AdoptersVerificationCodeRepository(MongoDbContext context) : BaseRepository<AdopterVerificationCode, string>(context, "users-verification-codes"), IUserVerificationCodeRepository
    {
    }
}
