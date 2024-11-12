using Caramel.Pattern.Services.Domain.Entities.Models;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public abstract class BaseRepository<TEntity, T> : IBaseRespository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
    {
        private readonly MongoDbContext _context;
        protected readonly IMongoCollection<TEntity> _collection;

        protected MongoDbContext Context { get { return _context; } }

        public BaseRepository(MongoDbContext context, string collectionName)
        {
            _context = context;
            _collection = _context.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> GetSingleAsync(T id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _collection.Find(condition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> FetchAsync(Expression<Func<TEntity, bool>> condition = null)
        {
            return condition != null
                ? await _collection.Find(condition).ToListAsync()
                : await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public void Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            _collection.ReplaceOne(filter, entity);
        }

        public void Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            _collection.DeleteOne(filter);
        }
    }
}
