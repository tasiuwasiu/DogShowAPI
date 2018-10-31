using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class UsersSecurity
    {
        public int UserId { get; set; }
        public string UserSalt { get; set; }
        public string UserHash { get; set; }
        public int PermissionLevel { get; set; }

        public UsersPermission PermissionLevelNavigation { get; set; }
        public User User { get; set; }
    }
}
