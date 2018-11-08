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
        Place getPlace(int placeId);
        List<Place> getAllPlaces();
        Place editPlace(int placeId, Place newPlace);
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

        public Place getPlace(int placeId)
        {
            return context.Place.Where(p => p.PlaceId == placeId).FirstOrDefault();
        }

        public List<Place> getAllPlaces()
        {
            return context.Place.ToList();
        }

        public Place editPlace(int placeId, Place newPlace)
        {
            Place place = context.Place.Where(p => p.PlaceId == placeId).FirstOrDefault();
            if (place == null)
            {
                return null;
            }
            place.Name = newPlace.Name;
            context.SaveChanges();
            return place;
        }
    }
}
