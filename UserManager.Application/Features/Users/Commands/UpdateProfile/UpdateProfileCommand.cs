using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Users.Commands.UpdateProfile
{
    public sealed record UpdateProfileCommand(
        string UserId,
        string FullName,
        DateTime DateOfBirth,
        string Gender,
        string Address) : ICommand<bool>;
}
