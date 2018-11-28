using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class DogInfoDTO
    {
        public int dogId { get; set; }
        public string name { get; set; }
        public string breedName { get; set; }
        public string className { get; set; }
        public string chipNumber { get; set; }
    }
}
