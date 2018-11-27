using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class DogClass
    {
        public DogClass()
        {
            Dog = new HashSet<Dog>();
        }

        public int ClassId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }

        public ICollection<Dog> Dog { get; set; }
    }
}
