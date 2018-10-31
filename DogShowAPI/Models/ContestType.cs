using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class ContestType
    {
        public ContestType()
        {
            Contest = new HashSet<Contest>();
        }

        public int ContestTypeId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public sbyte Enterable { get; set; }

        public ICollection<Contest> Contest { get; set; }
    }
}
