using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class SavedGradeDTO
    {
        public int participationId { get; set; }
        public int gradeId { get; set; }
        public bool isFinalist { get; set; }
        public int place { get; set; }
        public string description { get; set; }
    }
}
