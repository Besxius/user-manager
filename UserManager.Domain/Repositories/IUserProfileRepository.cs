using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;

namespace UserManager.Domain.Repositories
{
    public interface IUserProfileRepository
    {
            Task<UserProfile> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
            Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default);
            Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default);
    }
}
