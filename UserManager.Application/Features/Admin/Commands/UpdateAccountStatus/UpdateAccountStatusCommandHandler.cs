using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Constants;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Admin.Commands.UpdateAccountStatus
{
    public class UpdateAccountStatusCommandHandler : ICommandHandler<UpdateAccountStatusCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateAccountStatusCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateAccountStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (request.Status.Equals(UserStatuses.Locked, StringComparison.OrdinalIgnoreCase))
            {
                user.LockAccount();
            }
            else if (request.Status.Equals(UserStatuses.Active, StringComparison.OrdinalIgnoreCase))
            {
                user.UnlockAccount();
            }
            else
            {
                throw new Exception("Invalid status. Allowed values: Active, Locked.");
            }

            await _userRepository.UpdateAsync(user, cancellationToken);

            return true;
        }
    }
}
