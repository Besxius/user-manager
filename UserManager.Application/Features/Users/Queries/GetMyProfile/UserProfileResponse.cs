using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application.Features.Users.Queries.GetMyProfile
{
    public sealed record UserProfileResponse(
        string Id,
        string UserId,
        string FullName,
        DateTime DateOfBirth,
        string Gender,
        string Address);

}
