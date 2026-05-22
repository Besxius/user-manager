using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Commands.UpdateAccountRole
{
    public class UpdateAccountRoleCommandHandler : ICommandHandler<UpdateAccountRoleCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateAccountRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(UpdateAccountRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role == null)
            {
                throw new Exception("Role does not exist.");
            }

            user.ChangeRole(command.RoleId);

            await _userRepository.UpdateAsync(user, cancellationToken);

            return true;
        }
    }
}
