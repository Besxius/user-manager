using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Application.Features.Admin.Queries.GetAllUsers;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Queries.SearchUsers
{
    public sealed class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, IReadOnlyList<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public SearchUsersQueryHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IReadOnlyList<UserResponse>> Handle(SearchUsersQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.SearchTerm))
                return Array.Empty<UserResponse>();

            var searchTerm = query.SearchTerm.Trim();

            // Tìm kiếm song song trên cả 2 bảng để tăng tối đa tốc độ phản hồi
            var usersTask = _userRepository.SearchAsync(searchTerm, cancellationToken);
            var profilesTask = _userProfileRepository.SearchByFullNameAsync(searchTerm, cancellationToken);

            await Task.WhenAll(usersTask, profilesTask);

            // Gộp toàn bộ UserId tìm thấy từ cả 2 nguồn và loại bỏ trùng lặp
            var allUserIds = usersTask.Result.Select(u => u.Id)
                .Union(profilesTask.Result.Select(p => p.UserId))
                .Where(id => id != query.CurrentUserId)
                .Distinct()
                .ToList();

            if (!allUserIds.Any())
                return Array.Empty<UserResponse>();

            // Truy vấn dữ liệu đầy đủ của danh sách ID thu được
            var finalUsersTask = _userRepository.GetByIdsAsync(allUserIds, cancellationToken);
            var finalProfilesTask = _userProfileRepository.GetByUserIdsAsync(allUserIds, cancellationToken);

            await Task.WhenAll(finalUsersTask, finalProfilesTask);

            var finalUsers = finalUsersTask.Result;
            var profileMap = finalProfilesTask.Result.ToDictionary(p => p.UserId);

            return finalUsers.Select(user =>
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
