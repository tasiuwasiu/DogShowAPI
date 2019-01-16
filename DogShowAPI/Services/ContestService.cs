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
        List<ContestInfoDTO> getContestsByBreed(int breedId);
        List<ContestInfoDTO> getContestsByDog(int dogId);
        List<ContestInfoDTO> getNotPlanned();
        void deleteContest(int contestId);
        Contest planContest(Contest newContest);
        void participate(Participation participation);
        void deleteParticipation(Participation participation);
        Participation getParticipationById(int id);
        List<DogParticipationDTO> getDogParticipation(int dogId);
        List<PlanInfoDTO> getPlan();
        List<GradeDTO> getAllGrades();
        void saveGrade(SavedGradeDTO grade);
        ContestDetailsDTO editContest(int id, ContestDetailsDTO newContest);
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
                DogClass classD = context.DogClass.Where(c => c.ClassId == dog.ClassId).FirstOrDefault();
                string place = (p.Place == null) ? "Nie przyznano" : p.Place.ToString();
                if (grade == null)
                {
                    participations.Add(new ParticipationInfoDTO
                    {
                        participationId = p.ParticipationId,
                        dogId = p.DogId,
                        name = dog.Name,
                        breedName = breed.NamePolish,
                        className = classD.NamePolish,
                        chipNumber = dog.ChipNumber,
                        gradeId = 0,
                        grade = "Nie oceniono",
                        place = place,
                        description = p.Description
                    });
                }
                else
                {
                    participations.Add(new ParticipationInfoDTO
                    {
                        participationId = p.ParticipationId,
                        dogId = p.DogId,
                        name = dog.Name,
                        breedName = breed.NamePolish,
                        className = classD.NamePolish,
                        chipNumber = dog.ChipNumber,
                        gradeId = grade.GradeId,
                        grade = grade.NamePolish,
                        place = place,
                        description = p.Description
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

        public ContestDetailsDTO editContest(int id, ContestDetailsDTO newContest)
        {
            ContestType contestType = context.ContestType.Where(ct => ct.ContestTypeId == id).FirstOrDefault();
            if (contestType == null)
                throw new AppException("Nie odnaleziono konkursu w bazie");
            Contest contest = context.Contest.Where(c => c.ContestTypeId == id).FirstOrDefault();
            contestType.NamePolish = newContest.name;
            contestType.Enterable = newContest.isEnterable;
            if (contest != null)
            {
                contest.PlaceId = newContest.placeId;
                contest.StartDate = newContest.startDate;
                contest.EndDate = newContest.endDate;
            }
            context.SaveChanges();
            return getContest(id);
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
                        place = place,
                        description = participation.Description
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
                        place = place,
                        description = participation.Description
                    });
                }
                
            }
            return dogParticipations;
        }

        public List<PlanInfoDTO> getPlan()
        {
            List<PlanInfoDTO> plans = new List<PlanInfoDTO>();
            List<Contest> contests = context.Contest.ToList();
            if (contests.Count < 1)
                throw new AppException("Brak zaplanowanych konkursów");
            var groupedContests = contests.GroupBy(c => c.StartDate.Date);
            foreach (var gContest in groupedContests)
            {
                List<ContestInfoDTO> contestInfoList = new List<ContestInfoDTO>();
                foreach( Contest contest in gContest)
                {
                    ContestType contestType = context.ContestType.Where(ct => ct.ContestTypeId == contest.ContestTypeId).FirstOrDefault();
                    Place place = context.Place.Where(p => p.PlaceId == contest.PlaceId).FirstOrDefault();

                    ContestInfoDTO contestInfo = new ContestInfoDTO
                    {
                        contestTypeId = contestType.ContestTypeId,
                        contestId = contest.ContestId,
                        name = contestType.NamePolish,
                        placeName = place.Name,
                        startDate = contest.StartDate,
                        endDate = contest.EndDate
                    };

                    contestInfoList.Add(contestInfo);
                }
                
                PlanInfoDTO planInfo = new PlanInfoDTO
                {
                    date = gContest.Key.ToShortDateString(),
                    contests = contestInfoList
                };

                plans.Add(planInfo);
            }
            return plans;
        }

        public List<GradeDTO> getAllGrades()
        {
            List<Grade> grades = context.Grade.ToList();
            List<GradeDTO> gradesDTO = new List<GradeDTO>();
            foreach (Grade g in grades)
            {
                gradesDTO.Add(new GradeDTO
                {
                    gradeId = g.GradeId,
                    gradeLevel = g.GradeLevel,
                    namePolish = g.NamePolish,
                    nameEnglish = g.NameEnglish,
                    forPuppies = g.ForPuppies
                });
            }
            if (gradesDTO.Count < 1)
                throw new AppException("Nie odnaleziono ocen");
            return gradesDTO;
        }

        public void saveGrade(SavedGradeDTO grade)
        {
            Participation participation = context.Participation.Where(p => p.ParticipationId == grade.participationId).FirstOrDefault();
            if (participation == null)
                throw new AppException("Nie odnaleziono uczestnika");
            participation.GradeId = grade.gradeId;
            participation.Description = grade.description;
            if (grade.isFinalist)
            {
                if (context.Participation.Where(p => p.ContestId == grade.gradeId && p.Place == grade.place && p.ParticipationId != grade.participationId).Count() > 0)
                    throw new AppException("Istnieje już finalista o podanym miejscu");
                participation.Place = grade.place;
            }
            context.SaveChanges();
        }
    }
}
