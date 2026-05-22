using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context, IOptions<MongoDbSettings> settings)
        {
            _users = context.GetCollection<User>(settings.Value.UsersCollectionName);
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _users.Find(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _users.Find(u => u.Email == email)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _users.Find(_ => true)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _users.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user, new ReplaceOptions(), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _users.DeleteOneAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetFilteredAsync(string? roleId, string? status, IEnumerable<string>? userIds, CancellationToken cancellationToken = default)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(roleId))
                filter &= builder.Eq(u => u.RoleId, roleId);

            if (!string.IsNullOrEmpty(status))
                filter &= builder.Eq(u => u.Status, status);

            if (userIds != null)
                filter &= builder.In(u => u.Id, userIds);

            return await _users.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Or(
                builder.Eq(u => u.Id, searchTerm),
                builder.Regex(u => u.Email, new BsonRegularExpression(searchTerm, "i"))
            );

            return await _users.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await _users.Find(u => ids.Contains(u.Id)).ToListAsync(cancellationToken);
        }
    }
}
