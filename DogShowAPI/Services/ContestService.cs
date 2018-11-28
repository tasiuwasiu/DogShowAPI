﻿using DogShowAPI.DTOs;
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
        List<ContestInfoDTO> getContestsByBreed(int breedId);
        List<ContestInfoDTO> getContestsByDog(int dogId);
        List<ContestInfoDTO> getNotPlanned();
        void deleteContest(int contestId);
        Contest planContest(Contest newContest);
        void participate(Participation participation);
        void deleteParticipation(Participation participation);
        Participation getParticipationById(int id);
        List<DogParticipationDTO> getDogParticipation(int dogId);
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
            List<AllowedBreedsContest> allowed = context.AllowedBreedsContest.Where(abc => abc.ContestTypeId == id).ToList();
            foreach (AllowedBreedsContest abc in allowed)
            {
                DogBreed breed = context.DogBreed.Where(db => db.BreedId == abc.BreedTypeId).FirstOrDefault();
                allowedBreeds.Add(new BreedInfoDTO
                {
                    breedId = abc.BreedTypeId,
                    name = breed.NamePolish
                });
            }

            List<ParticipationInfoDTO> participations = new List<ParticipationInfoDTO>();
            List<Participation> part = context.Participation.Where(p => p.ContestId == id).ToList();
            foreach (Participation p in part)
            {
                Dog dog = context.Dog.Where(d => d.DogId == p.DogId).FirstOrDefault();
                DogBreed breed = context.DogBreed.Where(db => db.BreedId == dog.BreedId).FirstOrDefault();
                Grade grade = context.Grade.Where(g => g.GradeId == p.GradeId).FirstOrDefault();
                string place = (p.Place == null) ? "Nie przyznano" : p.Place.ToString();
                if (grade == null)
                {
                    participations.Add(new ParticipationInfoDTO
                    {
                        dogId = p.DogId,
                        name = dog.Name,
                        breedName = breed.NamePolish,
                        chipNumber = dog.ChipNumber,
                        grade = "Nie oceniono",
                        place = place
                    });
                }
                else
                {
                    participations.Add(new ParticipationInfoDTO
                    {
                        dogId = p.DogId,
                        name = dog.Name,
                        breedName = breed.NamePolish,
                        chipNumber = dog.ChipNumber,
                        grade = grade.NamePolish,
                        place = place
                    });
                }
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
                Place place = context.Place.Where(p => p.PlaceId == contest.PlaceId).FirstOrDefault();
                contestDetails = new ContestDetailsDTO
                {
                    contestTypeId = contestType.ContestTypeId,
                    contestId = contest.ContestId,
                    name = contestType.NamePolish,
                    isEnterable = Convert.ToBoolean(contestType.Enterable),
                    placeId = contest.PlaceId,
                    placeName = place.Name,
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
                Contest plannedContest = context.Contest.Where(c => c.ContestTypeId == contest.ContestTypeId).FirstOrDefault();

                if (plannedContest != null)
                {
                    Place place = context.Place.Where(p => p.PlaceId == plannedContest.PlaceId).FirstOrDefault();
                    if (place == null)
                    {
                        response.Add(new ContestInfoDTO
                        {
                            contestTypeId = contest.ContestTypeId,
                            contestId = plannedContest.ContestId,
                            name = contest.NamePolish,
                            placeName = "Nie ustawiono",
                            startDate = plannedContest.StartDate,
                            endDate = plannedContest.EndDate,
                        });
                    }
                    else
                    {
                        response.Add(new ContestInfoDTO
                        {
                            contestTypeId = contest.ContestTypeId,
                            contestId = plannedContest.ContestId,
                            name = contest.NamePolish,
                            placeName = plannedContest.Place.Name,
                            startDate = plannedContest.StartDate,
                            endDate = plannedContest.EndDate,
                        });
                    }
                    
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

        public List<ContestInfoDTO> getContestsByBreed(int breedId)
        {
            List<ContestInfoDTO> contests = new List<ContestInfoDTO>();
            List<AllowedBreedsContest> allowed = context.AllowedBreedsContest.Where(abc => abc.BreedTypeId == breedId).ToList();
            foreach (AllowedBreedsContest allowedBreedsContest in allowed)
            {
                ContestType contestType = context.ContestType.Where(ct => ct.ContestTypeId == allowedBreedsContest.ContestTypeId).FirstOrDefault();

                if (contestType != null && contestType.Enterable)
                {
                    Contest plannedContest = context.Contest.Where(c => c.ContestTypeId == allowedBreedsContest.ContestTypeId).FirstOrDefault();

                    if (plannedContest != null)
                    {
                        Place place = context.Place.Where(p => p.PlaceId == plannedContest.PlaceId).FirstOrDefault();
                        if (place != null)
                        {
                            contests.Add(new ContestInfoDTO
                            {
                                contestId = plannedContest.ContestId,
                                contestTypeId = allowedBreedsContest.ContestTypeId,
                                name = allowedBreedsContest.ContestType.NamePolish,
                                placeName = place.Name,
                                startDate = plannedContest.StartDate,
                                endDate = plannedContest.EndDate
                            });
                        }
                        else
                        {
                            contests.Add(new ContestInfoDTO
                            {
                                contestId = plannedContest.ContestId,
                                contestTypeId = allowedBreedsContest.ContestTypeId,
                                name = allowedBreedsContest.ContestType.NamePolish,
                                placeName = "Nie ustawiono",
                                startDate = plannedContest.StartDate,
                                endDate = plannedContest.EndDate
                            });
                        }
                        
                    }
                    else
                    {
                        contests.Add(new ContestInfoDTO
                        {
                            contestId = -1,
                            contestTypeId = allowedBreedsContest.ContestTypeId,
                            name = allowedBreedsContest.ContestType.NamePolish,
                            placeName = null,
                            startDate = new DateTime(),
                            endDate = new DateTime()
                        });
                    }
                }
            }
            if (contests.Count < 1)
                throw new AppException("Nie odnaleziono konkursów dla danej rasy");
            return contests;
        }

        public List<ContestInfoDTO> getNotPlanned()
        {
            List<ContestInfoDTO> notPlannedContests = new List<ContestInfoDTO>();
            List<ContestType> contests = context.ContestType.Where(ct => ct.Contest.Count == 0).ToList();
            foreach (ContestType contest in contests)
            {
                notPlannedContests.Add(new ContestInfoDTO
                {
                    contestId = -1,
                    contestTypeId = contest.ContestTypeId,
                    name = contest.NamePolish,
                    placeName = null,
                    startDate = new DateTime(),
                    endDate = new DateTime()
                });
            }
            if (notPlannedContests.Count < 1)
                throw new AppException("Brak niezaplanowanych konkursów");
            return notPlannedContests;
        }

        public void deleteContest(int contestId)
        {
            ContestType contest = context.ContestType.Where(ct => ct.ContestTypeId == contestId).FirstOrDefault();
            if (contest == null)
                throw new AppException("Nie odnaleziono podanego konkursu");
            List<Participation> participations = context.Participation.Where(p => p.ContestId == contestId).ToList();
            context.Participation.RemoveRange(participations);

            List<AllowedBreedsContest> allowedBreeds = context.AllowedBreedsContest.Where(p => p.ContestTypeId == contestId).ToList();
            if (allowedBreeds.Count > 0)
                context.AllowedBreedsContest.RemoveRange(allowedBreeds);

            List<Contest> contests = context.Contest.Where(c => c.ContestTypeId == contestId).ToList();
            if (contests.Count > 0)
            {
                context.Contest.RemoveRange(contests);
            }
            context.Remove(contest);
            context.SaveChanges();
        }

        public Contest planContest(Contest newContest)
        {
            ContestType contestType = context.ContestType.Where(ct => ct.ContestTypeId == newContest.ContestTypeId).FirstOrDefault();
            if (contestType == null)
                throw new AppException("Błędne id konkursu");
            if (contestType.Contest.Count > 0)
                throw new AppException("Konkurs jest już zaplanowany");
            context.Contest.Add(newContest);
            context.SaveChanges();
            return context.Contest.Where(c => c.ContestId == newContest.ContestId).FirstOrDefault();
        }

        public List<ContestInfoDTO> getContestsByDog(int dogId)
        {
            Dog dog = context.Dog.Where(d => d.DogId == dogId).FirstOrDefault();
            if (dog == null)
                throw new AppException("Błędny id psa");
            return getContestsByBreed(dog.BreedId);
        }

        public void participate(Participation participation)
        {
            participation.GradeId = null;
            participation.Place = null;
            context.Participation.Add(participation);
            context.SaveChanges();
        }

        public void deleteParticipation(Participation participation)
        {
            context.Participation.Remove(participation);
            context.SaveChanges();
        }

        public Participation getParticipationById(int id)
        {
            return context.Participation.Where(p => p.ParticipationId == id).FirstOrDefault();
        }

        public List<DogParticipationDTO> getDogParticipation(int dogId)
        {
            List<Participation> participations = context.Participation.Where(p => p.DogId == dogId).ToList();
            if (participations == null)
                return null;
            List<DogParticipationDTO> dogParticipations = new List<DogParticipationDTO>();
            foreach (Participation participation in participations)
            {
                ContestType contest = context.ContestType.Where(ct => ct.ContestTypeId == participation.ContestId).FirstOrDefault();
                Grade grade = context.Grade.Where(g => g.GradeId == participation.GradeId).FirstOrDefault();
                string place = (participation.Place == null) ? "Nie przyznano" : participation.Place.ToString();
                if (grade == null)
                {
                    dogParticipations.Add(new DogParticipationDTO
                    {
                        participationId = participation.ParticipationId,
                        dogId = participation.DogId,
                        contestName = contest.NamePolish,
                        grade = "Nie oceniono",
                        place = place
                   });
                }
                else
                {
                    dogParticipations.Add(new DogParticipationDTO
                    {
                        participationId = participation.ParticipationId,
                        dogId = participation.DogId,
                        contestName = contest.NamePolish,
                        grade = grade.NamePolish,
                        place = place
                });
                }
                
            }
            return dogParticipations;
        }
    }
}
