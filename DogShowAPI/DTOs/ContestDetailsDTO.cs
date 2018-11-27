using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class ContestDetailsDTO
    {
        public int contestTypeId { get; set; }
        public int contestId { get; set; }
        public string name { get; set; }
        public bool isEnterable { get; set; }
        public int placeId { get; set; }
        public string placeName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<BreedInfoDTO> allowedBreeds { get; set; }
        public List<ParticipationInfoDTO> participants { get; set; }
    }
}
