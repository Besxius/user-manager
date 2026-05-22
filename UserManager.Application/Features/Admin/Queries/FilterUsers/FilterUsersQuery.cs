using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Application.Features.Admin.Queries.GetAllUsers;

namespace UserManager.Application.Features.Admin.Queries.FilterUsers
{
    public sealed record FilterUsersQuery(
        string CurrentUserId,
        string? RoleId,
        string? Status,
        string? Gender) : IQuery<IReadOnlyList<UserResponse>>;
}
