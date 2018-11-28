using DogShowAPI.DTOs;
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
        DogDetailsDTO getDogDetailsById(int dogId);
        List<DogInfoDTO> getByUserId(int userId);
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
            if(dog.Participation.Count >0)
            {
                throw new AppException("Pies jest zapisany na konkursy!");
            }
            context.Dog.Remove(dog);
            context.SaveChanges();
        }

        public Dog getDogById (int dogId)
        {
            return context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
        }

        public DogDetailsDTO getDogDetailsById(int dogId)
        {
            Dog dog = context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
            if (dog == null)
                throw new AppException("Nie odnaleziono psa");
            DogBreed breed = context.DogBreed.Where(db => db.BreedId == dog.BreedId).FirstOrDefault();
            if(breed == null)
                throw new AppException("Nie odnaleziono rasy");
            DogClass classD = context.DogClass.Where(c => c.ClassId == dog.ClassId).FirstOrDefault();
            if (classD == null)
                throw new AppException("Nie odnaleziono klasy");
            string sex = dog.Sex == "M" ? "Pies" : "Suka";
            DogDetailsDTO dogDetails = new DogDetailsDTO
            {
                dogId= dog.DogId,
                name = dog.Name,
                lineageNumber = dog.LineageNumber,
                registrationNumber = dog.RegistrationNumber,
                titles = dog.Titles,
                chipNumber = dog.ChipNumber,
                breedName = breed.NamePolish,
                sex = sex,
                birthday = dog.Birthday,
                fatherName = dog.FatherName,
                motherName = dog.MotherName,
                breederName = dog.BreederName,
                breederAddress = dog.BreederAddress,
                className = classD.NamePolish
            };
            return dogDetails;
        }

        public List<DogInfoDTO> getByUserId(int userId)
        {
            List<Dog> dogs = context.Dog.Where(d => d.OwnerId == userId).ToList();
            if (dogs.Count < 1)
                throw new AppException("Nie odnaleziono psów");
            List<DogInfoDTO> dogInfo = new List<DogInfoDTO>();
            foreach(Dog dog in dogs)
            {
                DogBreed breed = context.DogBreed.Where(db => db.BreedId == dog.BreedId).FirstOrDefault();
                DogClass dogClass = context.DogClass.Where(dc => dc.ClassId == dog.ClassId).FirstOrDefault();

                dogInfo.Add(new DogInfoDTO
                {
                    dogId = dog.DogId,
                    name = dog.Name,
                    breedName = breed.NamePolish,
                    className = dogClass.NamePolish,
                    chipNumber = dog.ChipNumber
                });
            }
            return dogInfo;
        }
    }
}
