using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Queries.GetAllRoles
{
    public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, IReadOnlyList<RoleResponse>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IReadOnlyList<RoleResponse>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync();
            var response = roles.Select(role => new RoleResponse(role.Id, role.Name, role.Description)).ToList();
            return response;
        }
    }
}
