using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Constants;

namespace UserManager.Domain.Entities
{
    public class User
    {
        public string Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string RoleId { get; private set; }
        public string Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(string email, string passwordHash, string roleId)
        {
            Id = Guid.NewGuid().ToString();
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            Status = UserStatuses.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(string newRoleId) => RoleId = newRoleId;
        public void LockAccount() => Status = UserStatuses.Locked;
        public void UnlockAccount() => Status = UserStatuses.Active;
    }
}
