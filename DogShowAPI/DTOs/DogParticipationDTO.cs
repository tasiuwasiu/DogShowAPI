using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class DogParticipationDTO
    {
        public int participationId { get; set; }
        public int dogId { get; set; }
        public string contestName { get; set; }
        public string grade { get; set; }
        public string place { get; set; }
    }
}
