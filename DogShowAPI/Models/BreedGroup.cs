using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class BreedGroup
    {
        public BreedGroup()
        {
            BreedSection = new HashSet<BreedSection>();
        }

        public int GroupId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }

        public ICollection<BreedSection> BreedSection { get; set; }
    }
}
