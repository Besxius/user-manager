using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Admin.Commands.UpdateAccountStatus
{
    public sealed record UpdateAccountStatusCommand(string UserId, string Status) : ICommand<bool>;
}
