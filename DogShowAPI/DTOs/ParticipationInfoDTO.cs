﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.DTOs
{
    public class ParticipationInfoDTO
    {
        public int dogId { get; set; }
        public string name { get; set; }
        public string breedName { get; set; }
    }
}