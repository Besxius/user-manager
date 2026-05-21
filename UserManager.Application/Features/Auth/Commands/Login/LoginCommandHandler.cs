using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Authentication;
using UserManager.Application.Abstractions.Cryptography;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Repositories;
using UserManager.Domain.Constants;

namespace UserManager.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _roleRepository = roleRepository;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), cancellationToken);
            if (user == null)
            {
                throw new Exception("Email or password is incorrected.");
            }

            bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Email or password is incorrected");
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

            string token = _jwtProvider.GenerateToken(user, role.Name);

            return token;
        }
    }
}
