using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Users.Commands.CreateProfile
{
    public sealed record CreateProfileCommand(
        string UserId,
        string FullName,
        DateTime DateOfBirth,
        string Gender,
        string Address) : ICommand<string>;
}
