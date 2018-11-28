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
    public class DogController : ControllerBase
    {
        private IUserService userService;
        private IDogService dogService;
        private readonly SecuritySettings securitySettings;


        public DogController(IDogService dogService, IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            this.userService = userService;
            this.dogService = dogService;
            this.securitySettings = securitySettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("getGroups")]
        public IActionResult getGroups()
        {
            return Ok(dogService.getGroups());
        }

        [AllowAnonymous]
        [HttpGet("getSectionsInGroup/{groupID}")]
        public IActionResult getSectionsInGroup(int groupID)
        {
            return Ok(dogService.getSectionsInGroup(groupID));
        }

        [AllowAnonymous]
        [HttpGet("getBreedsInSection/{sectionID}")]
        public IActionResult getBreedsInSection(int sectionID)
        {
            return Ok(dogService.getBreedsInSection(sectionID));
        }

        [AllowAnonymous]
        [HttpGet("getClasses")]
        public IActionResult getClasses()
        {
            return Ok(dogService.getClasses());
        }

        [HttpPost("add")]
        public IActionResult addDog([FromBody]Dog newDog)
        {
            newDog.Owner = null;
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int userId = int.Parse(userName);
            if (userId != newDog.OwnerId)
            {
                return Unauthorized();
            }

            try
            {
                Dog addedDog = dogService.addDog(newDog);
                if (addedDog == null)
                {
                    return BadRequest(new { message = "Błąd dodawania psa!" });
                }
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteDog (int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.CanUserAccessDog(claimsIdentity, id);
                dogService.deleteDog(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult getDogById (int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.CanUserAccessDog(claimsIdentity, id);
                Dog response = dogService.getDogById(id);
                if (response == null)
                    throw new AppException("Nie odnaleziono psa o podanym ID");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("getByUserId/{userId}")]
        public IActionResult getByUserId (int userId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int user = int.Parse(userName);
            if (userId != user)
            {
                return Unauthorized();
            }
            try
            {
                List<DogInfoDTO> response = dogService.getByUserId(userId);
                if (response == null)
                    throw new AppException("Nie odnaleziono psa o podanym ID");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("edit/{dogId}")]
        public IActionResult editDog (int dogId, [FromBody]Dog newDog)
        {

            return NotFound();
        }

        [HttpGet("details/{dogId}")]
        public IActionResult getDogDetails(int dogId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.CanUserAccessDog(claimsIdentity, dogId);
                DogDetailsDTO response = dogService.getDogDetailsById(dogId);
                if (response == null)
                    throw new AppException("Nie odnaleziono psa o podanym ID");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("getGroupSectionFromBreed/{breedId}")]
        public IActionResult getGroupSectionFromBreed(int breedId)
        {

            return NotFound();
        }



    }
}