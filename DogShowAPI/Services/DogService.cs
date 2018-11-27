using DogShowAPI.Helpers;
using DogShowAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.Services
{
    public interface IDogService
    {
        List<BreedGroup> getGroups();
        List<BreedSection> getSectionsInGroup(int groupID);
        List<DogBreed> getBreedsInSection(int sectionID);
        Dog addDog(Dog newDog);
        List<DogClass> getClasses();
        void deleteDog(int dogId);
        Dog getDogById(int dogId);
    }


    public class DogService : IDogService
    {
        private DogShowContext context;

        public DogService(DogShowContext context)
        {
            this.context = context;
        }


        public List<BreedGroup> getGroups()
        {
            return context.BreedGroup.ToList();
        }

        public List<BreedSection> getSectionsInGroup(int groupID)
        {
            return context.BreedSection.Where(bs => bs.GroupNumber == groupID).ToList();
        }

        public List<DogBreed> getBreedsInSection(int sectionID)
        {
            return context.DogBreed.Where(db => db.SectionId == sectionID).ToList();
        }

        public List<DogClass> getClasses()
        {
            return context.DogClass.ToList();
        }

        public Dog addDog(Dog newDog)
        {
            if (context.Dog.Where(d => d.ChipNumber == newDog.ChipNumber).Any())
            {
                throw new AppException("Pies z podanym numerem tatuażu/chipa już istnieje!");
            }
            context.Dog.Add(newDog);
            context.SaveChanges();
            return context.Dog.Where(d => d.ChipNumber == newDog.ChipNumber).FirstOrDefault();
        }

        public void deleteDog(int dogId)
        {
            Dog dog = context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
            if (dog == null)
            {
                throw new AppException("Nie odnaleziono podanego miejsca");
            }
            context.Dog.Remove(dog);
        }

        public Dog getDogById (int dogId)
        {
            return context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
        }
    }
}
