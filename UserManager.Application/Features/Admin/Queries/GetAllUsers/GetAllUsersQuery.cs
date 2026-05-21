using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Entities;

namespace UserManager.Application.Features.Admin.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery : IQuery<IReadOnlyList<UserResponse>>;
}
