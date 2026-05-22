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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPasswordHasher _passwordHasher;

        public MongoDataSeeder(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUserProfileRepository userProfileRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userProfileRepository = userProfileRepository;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            // 1. SEED ROLES
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

            // 2. SEED ADMIN
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

                var adminProfile = new UserProfile(
                    adminUser.Id, "System Administrator", new DateTime(1990, 1, 1), "Other", "System");
                await _userProfileRepository.AddAsync(adminProfile, cancellationToken);
            }   

            // 3. SEED TEST USERS (Dữ liệu mẫu để test Filter & Search)
            var testUsers = new List<(string Email, string FullName, DateTime Dob, string Gender, string Address, bool IsLocked)>
            {
                ("trong.dev@yourdomain.com", "Lê Đức Trọng", new DateTime(2003, 6, 16), "Male", "Hồ Chí Minh", false),
                ("hang.design@yourdomain.com", "Thu Hằng", new DateTime(2003, 2, 16), "Female", "Hồ Chí Minh", false),
                ("locked.user@yourdomain.com", "Spam Account", new DateTime(1995, 10, 10), "Other", "Unknown", true)
            };

            foreach (var testData in testUsers)
            {
                var existingTestUser = await _userRepository.GetByEmailAsync(testData.Email, cancellationToken);
                if (existingTestUser == null)
                {
                    var testUser = new User(
                        email: testData.Email,
                        passwordHash: _passwordHasher.Hash("User@123"),
                        roleId: userRole.Id.ToString()
                    );

                    if (testData.IsLocked)
                    {
                        testUser.LockAccount();
                    }

                    await _userRepository.AddAsync(testUser, cancellationToken);

                    var testProfile = new UserProfile(
                        testUser.Id,
                        testData.FullName,
                        testData.Dob,
                        testData.Gender,
                        testData.Address
                    );

                    await _userProfileRepository.AddAsync(testProfile, cancellationToken);
                }
            }
        }
    }
}
