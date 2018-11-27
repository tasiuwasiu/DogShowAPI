using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class ContestInfoDTO
    {
        public int contestTypeId { get; set; }
        public int contestId { get; set; }
        public string name { get; set; }
        public string placeName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
