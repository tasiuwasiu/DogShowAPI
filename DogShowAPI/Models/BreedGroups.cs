using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class BreedGroups
    {
        public BreedGroups()
        {
            BreedSections = new HashSet<BreedSections>();
        }

        public int GroupId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }

        public ICollection<BreedSections> BreedSections { get; set; }
    }
}
