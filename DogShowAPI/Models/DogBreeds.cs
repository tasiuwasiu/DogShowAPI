using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class DogBreeds
    {
        public DogBreeds()
        {
            AllowedBreedsContests = new HashSet<AllowedBreedsContests>();
        }

        public int BreedId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public int? SectionId { get; set; }

        public ICollection<AllowedBreedsContests> AllowedBreedsContests { get; set; }
    }
}
