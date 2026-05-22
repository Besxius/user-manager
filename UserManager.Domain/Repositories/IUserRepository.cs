using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;

namespace UserManager.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetFilteredAsync(string? roleId, string? status, IEnumerable<string>? userIds, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
