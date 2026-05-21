using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;

namespace UserManager.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task AddAsync(Role role, CancellationToken cancellationToken = default);
    }
}
