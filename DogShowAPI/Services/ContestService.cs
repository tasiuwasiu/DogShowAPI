using DogShowAPI.DTOs;
using DogShowAPI.Helpers;
using DogShowAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.Services
{
    public interface IContestService
    {
        ContestDetailsDTO getContest(int id);
        ContestType addContest(ContestType newContest);
        void addAllowedBreeds(List<AllowedBreedsContest> allowedBreeds);
        List<ContestInfoDTO> getAll();
    }

    public class ContestService : IContestService
    {
        private DogShowContext context;

        public ContestService(DogShowContext context)
        {
            this.context = context;
        }

        public ContestDetailsDTO getContest(int id)
        {
            ContestDetailsDTO contestDetails;
            ContestType contestType = context.ContestType.Where(ct => ct.ContestTypeId == id).FirstOrDefault();
            if (contestType == null)
                throw new AppException("Nie odnaleziono konkursu o podanym id");

            List<BreedInfoDTO> allowedBreeds = new List<BreedInfoDTO>();
            foreach (AllowedBreedsContest abc in contestType.AllowedBreedsContest)
            {
                allowedBreeds.Add(new BreedInfoDTO
                {
                    breedId = abc.BreedTypeId,
                    name = abc.BreedType.NamePolish
                });
            }

            List<ParticipationInfoDTO> participations = new List<ParticipationInfoDTO>();
            foreach (Participation p in contestType.Participation)
            {
                participations.Add(new ParticipationInfoDTO
                {
                    dogId = p.DogId,
                    name = p.Dog.Name,
                    breedName = p.Dog.Breed.NamePolish
                });
            }

            Contest contest = context.Contest.Where(c => c.ContestTypeId == id).FirstOrDefault();
            if (contest == null)
            {
                contestDetails = new ContestDetailsDTO
                {
                    contestTypeId = contestType.ContestTypeId,
                    contestId = -1,
                    name = contestType.NamePolish,
                    isEnterable = Convert.ToBoolean(contestType.Enterable),
                    placeId = -1,
                    placeName = null,
                    startDate = new DateTime(),
                    endDate = new DateTime(),
                    allowedBreeds = allowedBreeds,
                    participants = participations
                };
            }
            else
            {
                contestDetails = new ContestDetailsDTO
                {
                    contestTypeId = contestType.ContestTypeId,
                    contestId = contest.ContestId,
                    name = contestType.NamePolish,
                    isEnterable = Convert.ToBoolean(contestType.Enterable),
                    placeId = contest.PlaceId,
                    placeName = contest.Place.Name,
                    startDate = contest.StartDate,
                    endDate = contest.EndDate,
                    allowedBreeds = allowedBreeds,
                    participants = participations
                };
            }
            return contestDetails;
        }

        public ContestType addContest(ContestType newContest)
        {
            context.ContestType.Add(newContest);
            context.SaveChanges();
            return context.ContestType.Where(ct => ct.ContestTypeId == newContest.ContestTypeId).FirstOrDefault();
        }

        public void addAllowedBreeds(List<AllowedBreedsContest> allowedBreeds)
        {
            if (allowedBreeds.Count > 0)
            {
                context.AllowedBreedsContest.AddRange(allowedBreeds);
                context.SaveChanges();
            }
        }

        public List<ContestInfoDTO> getAll()
        {
            List<ContestType> contests = context.ContestType.ToList();
            if (contests == null || contests.Count == 0)
                throw new AppException("Brak konkursów w bazie");
            List<ContestInfoDTO> response = new List<ContestInfoDTO>();
            foreach (ContestType contest in contests )
            {
                if (contest.Contest.Count > 0)
                {
                    response.Add(new ContestInfoDTO
                    {
                        contestTypeId = contest.ContestTypeId,
                        contestId = contest.Contest.First().ContestId,
                        name = contest.NamePolish,
                        placeName = contest.Contest.First().Place.Name,
                        startDate = contest.Contest.First().StartDate,
                        endDate = contest.Contest.First().EndDate,
                    });
                }
                else
                {
                    response.Add(new ContestInfoDTO
                    {
                        contestTypeId = contest.ContestTypeId,
                        contestId = -1,
                        name = contest.NamePolish,
                        placeName = null,
                        startDate = new DateTime(),
                        endDate = new DateTime(),
                    });
                }
            }

            return response;
        }

    }
}
