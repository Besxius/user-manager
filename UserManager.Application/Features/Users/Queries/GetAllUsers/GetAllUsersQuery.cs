using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;

namespace UserManager.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>>
    {
    }
}
