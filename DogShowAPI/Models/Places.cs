using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Places
    {
        public Places()
        {
            Contests = new HashSet<Contests>();
        }

        public int PlaceId { get; set; }
        public string Name { get; set; }

        public ICollection<Contests> Contests { get; set; }
    }
}
