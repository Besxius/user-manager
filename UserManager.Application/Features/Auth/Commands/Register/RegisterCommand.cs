using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Auth.Commands.RegisterUser
{
    public sealed record RegisterCommand(
        string Email,
        string Password,
        string ConfirmPassword) : ICommand<string>;
}
