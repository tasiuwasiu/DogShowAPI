using DogShowAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.Services
{
    public interface IPlaceService
    {
        Place addPlace(Place newPlace);
    }

    public class PlaceService : IPlaceService
    {
        private DogShowContext context;

        public PlaceService(DogShowContext context)
        {
            this.context = context;
        }

        public Place addPlace(Place newPlace)
        {
            context.Place.Add(newPlace);
            context.SaveChanges();
            return context.Place.Where(p => p.Name == newPlace.Name).FirstOrDefault();
        }
    }
}
