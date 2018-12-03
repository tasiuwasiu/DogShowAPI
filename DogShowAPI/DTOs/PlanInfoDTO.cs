using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class PlanInfoDTO
    {
        public string date { get; set; }
        public List<ContestInfoDTO> contests { get; set; }
    }
}
