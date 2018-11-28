using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogShowAPI.DTOs;
using DogShowAPI.Helpers;
using DogShowAPI.Models;
using DogShowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DogShowAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController : ControllerBase
    {
        private IContestService contestService;
        private IUserService userService;
        private readonly SecuritySettings securitySettings;

        public ContestController(IContestService contestService, IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            this.contestService = contestService;
            this.userService = userService;
            this.securitySettings = securitySettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult getContest(int id)
        {
            try
            {
                var response = contestService.getContest(id);
                if (response == null)
                    throw new AppException("Brak konkursu o podanym id w bazie");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public IActionResult getAllContests()
        {
            try
            {
                List<ContestInfoDTO> response = contestService.getAll();
                if (response.Count == 0 || response == null)
                    throw new AppException("Brak konkursów w bazie");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult addContest([FromBody]ContestTypeDTO newContestType)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                List<AllowedBreedsContest> allowedBreeds = new List<AllowedBreedsContest>();
                foreach (int breedId in newContestType.breedIds)
                {
                    allowedBreeds.Add(new AllowedBreedsContest
                    {
                        BreedTypeId = breedId
                    });
                }
                ContestType contestType = new ContestType
                {
                    Enterable = newContestType.isEnterable,
                    NamePolish = newContestType.name,
                    AllowedBreedsContest = allowedBreeds
                };
                ContestType savedContestType = contestService.addContest(contestType);
                if (savedContestType == null)
                {
                    throw new AppException("Błąd tworzenia konkursu");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("getAvailableByBreed/{breedId}")]
        public IActionResult getAvailableByBreed(int breedId)
        {
            try
            {
                var response = contestService.getContestsByBreed(breedId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpGet("getAvailableByDog/{dogId}")]
        public IActionResult getAvailableByDog(int dogId)
        {
            try
            {
                var response = contestService.getContestsByDog(dogId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpGet("getNotPlanned")]
        public IActionResult getNotPlanned()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                var response = contestService.getNotPlanned();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteContest(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                contestService.deleteContest(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("plan")]
        public IActionResult planContest([FromBody]Contest contest)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                var response = contestService.planContest(contest);
                if (response == null)
                    throw new AppException("Błąd planowania konkursu");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("participate")]
        public IActionResult participate([FromBody]Participation participation)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.CanUserAccessDog(claimsIdentity, participation.DogId);
                contestService.participate(participation);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("participation/{id}")]
        public IActionResult deleteParticipation(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                Participation participation = contestService.getParticipationById(id);
                if (participation == null)
                    throw new AppException("Nie odnaleziono danego połączenia");
                userService.CanUserAccessDog(claimsIdentity, participation.DogId);
                contestService.deleteParticipation(participation);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("getDogParticipation/{dogId}")]
        public IActionResult getDogParticipation(int dogId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.CanUserAccessDog(claimsIdentity, dogId);
                List<DogParticipationDTO> response = contestService.getDogParticipation(dogId);
                if (response.Count < 1)
                    throw new AppException("Nie odnaleziono przypisanych konkursów");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}