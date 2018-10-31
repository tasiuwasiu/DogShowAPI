using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class User
    {
        public User()
        {
            Dog = new HashSet<Dog>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public UsersSecurity UsersSecurity { get; set; }
        public ICollection<Dog> Dog { get; set; }
    }
}
