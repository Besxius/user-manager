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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IMongoCollection<UserProfile> _userProfiles;
        public UserProfileRepository(MongoDbContext context, IOptions<MongoDbSettings> settings)   
        {
            _userProfiles = context.GetCollection<UserProfile>(settings.Value.UserProfilesCollectionName);
        }

        public async Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _userProfiles.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default)
        {
            await _userProfiles.InsertOneAsync(profile, new InsertOneOptions(), cancellationToken);
        }

        public async Task<UserProfile> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _userProfiles
                .Find(x => x.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default)
        {
            await _userProfiles.ReplaceOneAsync(x => x.UserId == profile.UserId, profile, new ReplaceOptions(), cancellationToken);
        }

        public async Task<IReadOnlyList<UserProfile>> GetByGenderAsync(string gender, CancellationToken cancellationToken = default)
        {
            return await _userProfiles.Find(p => p.Gender == gender).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserProfile>> GetByUserIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
        {
            return await _userProfiles.Find(p => userIds.Contains(p.UserId)).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserProfile>> SearchByFullNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserProfile>.Filter.Regex(p => p.FullName, new BsonRegularExpression(searchTerm, "i"));
            return await _userProfiles.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
