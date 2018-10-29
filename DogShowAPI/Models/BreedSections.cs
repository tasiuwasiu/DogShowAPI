using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class BreedSections
    {
        public int SectionId { get; set; }
        public int? SectionNumber { get; set; }
        public int? GroupNumber { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }

        public BreedGroups GroupNumberNavigation { get; set; }
    }
}
