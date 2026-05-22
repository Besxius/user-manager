using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Application.Features.Admin.Queries.GetAllUsers;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Queries.FilterUsers
{
    public sealed class FilterUsersQueryHandler : IQueryHandler<FilterUsersQuery, IReadOnlyList<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public FilterUsersQueryHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IReadOnlyList<UserResponse>> Handle(FilterUsersQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<string>? allowedUserIds = null;

            if (!string.IsNullOrEmpty(query.Gender))
            {
                var profilesWithGender = await _userProfileRepository.GetByGenderAsync(query.Gender, cancellationToken);
                allowedUserIds = profilesWithGender.Select(p => p.UserId).ToList();

                if (!allowedUserIds.Any())
                    return Array.Empty<UserResponse>();
            }

            var usersFromDb = await _userRepository.GetFilteredAsync(query.RoleId, query.Status, allowedUserIds, cancellationToken);

            // LỌC BỎ NGƯỜI ĐANG ĐĂNG NHẬP
            var users = usersFromDb.Where(u => u.Id != query.CurrentUserId).ToList();

            if (!users.Any())
                return Array.Empty<UserResponse>();

            var finalUserIds = users.Select(u => u.Id).ToList();
            var profiles = await _userProfileRepository.GetByUserIdsAsync(finalUserIds, cancellationToken);
            var profileMap = profiles.ToDictionary(p => p.UserId);

            return users.Select(user =>
            {
                profileMap.TryGetValue(user.Id, out var profile);
                return new UserResponse(
                    user.Id,
                    user.Email,
                    user.RoleId,
                    user.Status,
                    user.CreatedAt,
                    profile?.FullName ?? string.Empty,
                    profile?.DateOfBirth ?? default,
                    profile?.Gender ?? string.Empty,
                    profile?.Address ?? string.Empty
                );
            }).ToList();
        }
    }
}
