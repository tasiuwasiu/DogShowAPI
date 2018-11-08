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
    }


    public class DogService : IDogService
    {
        private DogShowContext context;

        public DogService(DogShowContext context)
        {
            this.context = context;
        }


        public List<BreedGroup> getGroups ()
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
    }
}
