using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Cryptography;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Constants;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        public RegisterCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordHasher.Hash(request.Password);

            var defaultRole = await _roleRepository.GetByNameAsync(UserRoles.User, cancellationToken);
            if (defaultRole == null)
            {
                defaultRole = new Role(UserRoles.User, "Default role for new users");

                await _roleRepository.AddAsync(defaultRole, cancellationToken);
            }

            var user = new User(request.Email, hashedPassword, defaultRole.Id);
            await _userRepository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}
