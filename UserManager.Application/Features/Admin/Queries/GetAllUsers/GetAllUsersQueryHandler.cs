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
        private readonly IUserProfileRepository _userProfileRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IReadOnlyList<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var usersTask = _userRepository.GetAllAsync(cancellationToken);
            var profilesTask = _userProfileRepository.GetAllAsync(cancellationToken);

            await Task.WhenAll(usersTask, profilesTask);

            var users = usersTask.Result.Where(u => u.Id != query.CurrentUserId).ToList();
            var profiles = profilesTask.Result;

            var profileDictionary = profiles.ToDictionary(p => p.UserId);

            return users.Select(user =>
            {
                profileDictionary.TryGetValue(user.Id, out var profile);

                return new UserResponse(
                    user.Id,
                    user.Email,
                    user.RoleId,
                    user.Status,
                    user.CreatedAt,
                    profile?.FullName ?? string.Empty,            
                    profile?.DateOfBirth ?? default(DateTime), 
                    profile?.Gender ?? string.Empty,
                    profile?.Address ?? string.Empty
                );
            }).ToList();
        }
    }
}
