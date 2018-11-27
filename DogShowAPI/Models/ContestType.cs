using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class ContestType
    {
        public ContestType()
        {
            AllowedBreedsContest = new HashSet<AllowedBreedsContest>();
            Contest = new HashSet<Contest>();
            Participation = new HashSet<Participation>();
        }

        public int ContestTypeId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public bool Enterable { get; set; }

        public ICollection<AllowedBreedsContest> AllowedBreedsContest { get; set; }
        public ICollection<Contest> Contest { get; set; }
        public ICollection<Participation> Participation { get; set; }
    }
}
