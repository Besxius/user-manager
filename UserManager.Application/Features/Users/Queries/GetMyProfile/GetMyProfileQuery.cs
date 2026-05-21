using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Users.Queries.GetMyProfile
{
    public sealed record GetMyProfileQuery(string UserId) : IQuery<UserProfileResponse>;
}
