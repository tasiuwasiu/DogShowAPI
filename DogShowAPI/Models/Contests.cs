using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Contests
    {
        public int ContestId { get; set; }
        public int? ContestTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PlaceId { get; set; }

        public ContestTypes ContestType { get; set; }
        public Places Place { get; set; }
    }
}
