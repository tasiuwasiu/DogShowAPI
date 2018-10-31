using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class UsersPermission
    {
        public UsersPermission()
        {
            UsersSecurity = new HashSet<UsersSecurity>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }

        public ICollection<UsersSecurity> UsersSecurity { get; set; }
    }
}
