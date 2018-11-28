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
}