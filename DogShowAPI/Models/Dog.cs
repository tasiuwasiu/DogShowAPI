using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Dog
    {
        public Dog()
        {
            Participation = new HashSet<Participation>();
        }

        public int DogId { get; set; }
        public string Name { get; set; }
        public string LineageNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string Titles { get; set; }
        public string ChipNumber { get; set; }
        public int BreedId { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string BreederName { get; set; }
        public string BreederAddress { get; set; }
        public int? ClassId { get; set; }
        public int OwnerId { get; set; }

        public DogBreed Breed { get; set; }
        public DogClass Class { get; set; }
        public User Owner { get; set; }
        public ICollection<Participation> Participation { get; set; }
    }
}
