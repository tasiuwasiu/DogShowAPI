using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class ContestTypeDTO
    {
        public int contestTypeId { get; set; }
        public string name { get; set; }
        public bool isEnterable { get; set; }
        public List<int> breedIds { get; set; }
    }
}
