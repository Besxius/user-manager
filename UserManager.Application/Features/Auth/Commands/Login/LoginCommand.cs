using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand (
        string Email,
        string Password) : ICommand<string>;
}
