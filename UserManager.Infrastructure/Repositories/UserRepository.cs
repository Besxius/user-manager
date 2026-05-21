using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(MongoDbContext context, IOptions<MongoDbSettings> settings)
        {
            _usersCollection = context.GetCollection<User>(settings.Value.UsersCollectionName);
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _usersCollection.Find(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _usersCollection.Find(_ => true)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _usersCollection.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user, new ReplaceOptions(), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id, cancellationToken);
        }
    }
}
