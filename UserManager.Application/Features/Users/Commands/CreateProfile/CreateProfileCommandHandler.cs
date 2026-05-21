using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Messaging;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Application.Features.Users.Commands.CreateProfile
{
    public class CreateProfileCommandHandler : ICommandHandler<CreateProfileCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public CreateProfileCommandHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }


        public async Task<string> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null) throw new Exception("User does not exist.");

            var existingProfile = await _userProfileRepository.GetByUserIdAsync(command.UserId, cancellationToken);
            if (existingProfile != null) return existingProfile.Id;

            var profile = new UserProfile(
                command.UserId,
                command.FullName,
                command.DateOfBirth,
                command.Gender,
                command.Address);

            await _userProfileRepository.AddAsync(profile, cancellationToken);

            return profile.Id;
        }
    }
}
