using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application.Features.Admin.Queries.GetAllUsers
{
    public sealed record UserResponse(
        string Id,
        string Email,
        string RoleId,
        string Status,
        DateTime CreatedAt);
}
