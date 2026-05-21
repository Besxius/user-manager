using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Domain.Entities
{
    public class UserProfile
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public string Address { get; private set; }

        public UserProfile(string userId, string fullName, DateTime dateOfBirth, string gender, string address)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
        }

        public void UpdateInfo(string fullName, DateTime dateOfBirth, string gender, string address)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
        }
    }
}
