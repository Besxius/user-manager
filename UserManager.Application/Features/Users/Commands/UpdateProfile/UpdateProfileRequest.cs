using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application.Features.Users.Commands.UpdateProfile
{
    public sealed record UpdateProfileRequest(
        string FullName,
        DateTime DateOfBirth,
        string Gender,
        string Address);
}
