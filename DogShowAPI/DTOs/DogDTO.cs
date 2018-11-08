using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class DogDTO
    {
        public int DogID { get; set; }
        public string Name { get; set; }
        public string LineageNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string Titles { get; set; }
        public string ChipNumber { get; set; }
        public int BreedID { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string BreederName { get; set; }
        public string BreederAddress { get; set; }
        public int ClassID { get; set; }
        public int OwnerID { get; set; }
    }
}
