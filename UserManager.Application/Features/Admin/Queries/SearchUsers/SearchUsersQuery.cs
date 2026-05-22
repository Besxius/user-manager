using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Application.Features.Admin.Queries.GetAllUsers;

namespace UserManager.Application.Features.Admin.Queries.SearchUsers
{
    public sealed record SearchUsersQuery(
        string CurrentUserId, 
        string SearchTerm) : IQuery<IReadOnlyList<UserResponse>>;
}
