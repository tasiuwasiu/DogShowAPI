using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            Dogs = new HashSet<Dogs>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public UsersSecurity UsersSecurity { get; set; }
        public ICollection<Dogs> Dogs { get; set; }
    }
}
