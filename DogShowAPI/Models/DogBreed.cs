using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class DogBreed
    {
        public int BreedId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public int? SectionId { get; set; }
    }
}
