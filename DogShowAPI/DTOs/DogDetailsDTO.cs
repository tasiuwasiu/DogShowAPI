using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class DogDetailsDTO
    {
        public int dogId { get; set; }
        public string name { get; set; }
        public string lineageNumber { get; set; }
        public string registrationNumber { get; set; }
        public string titles { get; set; }
        public string chipNumber { get; set; }
        public string breedName { get; set; }
        public string sex { get; set; }
        public DateTime? birthday { get; set; }
        public string fatherName { get; set; }
        public string motherName { get; set; }
        public string breederName { get; set; }
        public string breederAddress { get; set; }
        public string className { get; set; }
    }
}
