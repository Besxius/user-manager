using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;

namespace UserManager.Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        string GenerateToken(User user, string roleName);
    }
}
