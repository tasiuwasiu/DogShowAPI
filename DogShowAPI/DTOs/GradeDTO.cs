using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class GradeDTO
    {
        public int gradeId { get; set; }
        public int gradeLevel { get; set; }
        public string namePolish { get; set; }
        public string nameEnglish { get; set; }
        public bool forPuppies { get; set; }
    }
}
