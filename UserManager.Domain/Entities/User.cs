using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Domain.Entities
{
    public class User
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(string username, string email)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email is required");

            Email = newEmail;
        }
    }
}
