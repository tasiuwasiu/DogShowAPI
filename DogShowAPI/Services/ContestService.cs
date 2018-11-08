using DogShowAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.Services
{
    public interface IContestService
    {

    }

    public class ContestService
    {
        private DogShowContext context;

        public ContestService(DogShowContext context)
        {
            this.context = context;
        }
    }
}
