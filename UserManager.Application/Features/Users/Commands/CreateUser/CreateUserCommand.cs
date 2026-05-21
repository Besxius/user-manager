using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
