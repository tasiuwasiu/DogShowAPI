using System;
using System.Collections.Generic;

namespace DogShowAPI.Models
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public int GradeLevel { get; set; }
        public string NamePolish { get; set; }
        public string NameEnglish { get; set; }
        public sbyte ForPuppies { get; set; }
    }
}
