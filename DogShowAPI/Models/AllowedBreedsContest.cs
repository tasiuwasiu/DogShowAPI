using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class AllowedBreedsContest
    {
        public int ContestTypeId { get; set; }
        public int BreedTypeId { get; set; }

        public DogBreed BreedType { get; set; }
        public ContestType ContestType { get; set; }
    }
}
