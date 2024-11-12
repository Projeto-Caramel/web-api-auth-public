using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Caramel.Pattern.Services.Infra.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDBSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDBSettings:DatabaseName"]);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
        {
            return _database.GetCollection<TEntity>(name);
        }
    }
}
