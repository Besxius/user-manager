using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;

namespace UserManager.Application.Features.Admin.Commands.UpdateAccountRole
{
    public sealed record UpdateAccountRoleCommand(string UserId, string RoleId) : ICommand<bool>;
}
