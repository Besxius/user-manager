using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IReadOnlyList<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IReadOnlyList<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users =  await _userRepository.GetAllAsync(cancellationToken);

            return users.Select(user => new UserResponse(
                user.Id,
                user.Email,
                user.RoleId,
                user.Status,
                user.CreatedAt)).ToList();
        }
    }
}
