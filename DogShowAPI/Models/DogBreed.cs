using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class DogBreed
    {
        public DogBreed()
        {
            AllowedBreedsContest = new HashSet<AllowedBreedsContest>();
            Dog = new HashSet<Dog>();
        }

        public int BreedId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public int? SectionId { get; set; }

        public BreedSection Section { get; set; }
        public ICollection<AllowedBreedsContest> AllowedBreedsContest { get; set; }
        public ICollection<Dog> Dog { get; set; }
    }
}
