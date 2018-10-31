using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Place
    {
        public Place()
        {
            Contest = new HashSet<Contest>();
        }

        public int PlaceId { get; set; }
        public string Name { get; set; }

        public ICollection<Contest> Contest { get; set; }
    }
}
