using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Application.Abstractions.Cryptography;
using UserManager.Domain.Constants;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Infrastructure.Persistence
{
    public class MongoDataSeeder
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public MongoDataSeeder(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var adminRole = await _roleRepository.GetByNameAsync(UserRoles.Admin, cancellationToken);
            if (adminRole == null)
            {
                adminRole = new Role(UserRoles.Admin, "Role for administrator");
                await _roleRepository.AddAsync(adminRole, cancellationToken);
            }

            var userRole = await _roleRepository.GetByNameAsync(UserRoles.User, cancellationToken);
            if (userRole == null)
            {
                userRole = new Role(UserRoles.User, "Default role for new user");
                await _roleRepository.AddAsync(userRole, cancellationToken);
            }

            var adminEmail = "admin@yourdomain.com";
            var existingAdmin = await _userRepository.GetByEmailAsync(adminEmail, cancellationToken);
            if (existingAdmin == null)
            {
                var adminUser = new User(
                    email: adminEmail,
                    passwordHash: _passwordHasher.Hash("Admin@123"),
                    roleId: adminRole.Id.ToString()
                );

                await _userRepository.AddAsync(adminUser, cancellationToken);
            }
        }
    }
}
