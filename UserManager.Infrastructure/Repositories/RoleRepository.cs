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
    public class RoleRepository : IRoleRepository
    {
        private readonly IMongoCollection<Role> _roles;
        public RoleRepository(MongoDbContext context, IOptions<MongoDbSettings> settings)
        {
            _roles = context.GetCollection<Role>(settings.Value.RolesCollectionName);
        }

        public async Task<Role> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _roles.Find(r => r.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _roles.Find(r => r.Name == name).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
        {
            await _roles.InsertOneAsync(role, new InsertOneOptions(), cancellationToken);
        }
    }
}
