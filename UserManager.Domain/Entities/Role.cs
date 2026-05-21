using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Domain.Entities
{
    public class Role
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Role(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name.ToUpperInvariant();
            Description = description;
        }
    }
}
