using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Participation
    {
        public int ParticipationId { get; set; }
        public int DogId { get; set; }
        public int ContestId { get; set; }
        public int? GradeId { get; set; }
        public int? Place { get; set; }

        public ContestType Contest { get; set; }
        public Dog Dog { get; set; }
        public Grade Grade { get; set; }
    }
}
