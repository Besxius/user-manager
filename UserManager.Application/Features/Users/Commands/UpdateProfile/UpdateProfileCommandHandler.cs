using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public UpdateProfileCommandHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<bool> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null) throw new Exception("User does not exist.");

            var profile = await _userProfileRepository.GetByUserIdAsync(command.UserId, cancellationToken);
            if (profile == null) throw new Exception("Profile does not exist. Please create profile first.");

            profile.UpdateInfo(command.FullName, command.DateOfBirth, command.Gender, command.Address);

            await _userProfileRepository.UpdateAsync(profile, cancellationToken);

            return true;
        }
    }
}
