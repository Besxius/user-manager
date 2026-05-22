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
        Task<IReadOnlyList<UserProfile>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserProfile> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default);
        Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserProfile>> GetByGenderAsync(string gender, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserProfile>> GetByUserIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserProfile>> SearchByFullNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
