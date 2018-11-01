using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class UsersSecurity
    {
        public int UserId { get; set; }
        public byte[] UserSalt { get; set; }
        public byte[] UserHash { get; set; }
        public int PermissionLevel { get; set; }

        public UsersPermission PermissionLevelNavigation { get; set; }
        public User User { get; set; }
    }
}
