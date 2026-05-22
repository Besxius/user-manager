using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Admin.Queries.GetAllRoles
{
    public sealed record GetAllRolesQuery : IQuery<IReadOnlyList<RoleResponse>>;
}
