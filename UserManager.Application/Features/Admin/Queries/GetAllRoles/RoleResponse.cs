using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application.Features.Admin.Queries.GetAllRoles
{
    public sealed record RoleResponse (string Id, string Name, string Description);
}
