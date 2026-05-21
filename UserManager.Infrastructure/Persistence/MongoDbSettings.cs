using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Infrastructure.Persistence
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string UserProfilesCollectionName { get; set; } = null!;
        public string RolesCollectionName { get; set; } = null!;
    }
}
