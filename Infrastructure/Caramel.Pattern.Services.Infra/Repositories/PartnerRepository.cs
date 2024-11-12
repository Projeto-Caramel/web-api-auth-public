using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public class PartnerRepository(MongoDbContext context) : BaseRepository<Partner, string>(context, "partners-data"), IPartnerRepository
    {
    }
}
