using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Users.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandler : IQueryHandler<GetMyProfileQuery, UserProfileResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public GetMyProfileQueryHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileResponse> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
        {
            var profile = await _userProfileRepository.GetByUserIdAsync(query.UserId, cancellationToken);

            if (profile == null)
            {
                throw new Exception("Profile not found for the requested user.");
            }

            return new UserProfileResponse(
                profile.Id,
                profile.UserId,
                profile.FullName,
                profile.DateOfBirth,
                profile.Gender,
                profile.Address);
        }
    }
}
