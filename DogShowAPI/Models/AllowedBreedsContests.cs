using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class AllowedBreedsContests
    {
        public int ContestTypeId { get; set; }
        public int BreedTypeId { get; set; }

        public DogBreeds BreedType { get; set; }
    }
}
