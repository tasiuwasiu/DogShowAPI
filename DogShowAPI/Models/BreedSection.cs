using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class BreedSection
    {
        public int SectionId { get; set; }
        public int? SectionNumber { get; set; }
        public int? GroupNumber { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }

        public BreedGroup GroupNumberNavigation { get; set; }
    }
}
