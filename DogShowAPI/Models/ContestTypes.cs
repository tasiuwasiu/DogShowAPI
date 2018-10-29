using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class ContestTypes
    {
        public ContestTypes()
        {
            Contests = new HashSet<Contests>();
        }

        public int ContestTypeId { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public sbyte Enterable { get; set; }

        public ICollection<Contests> Contests { get; set; }
    }
}
