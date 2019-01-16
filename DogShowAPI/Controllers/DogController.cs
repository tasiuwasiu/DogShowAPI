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
        private IAppSettingsService appSettingsService;
        private readonly SecuritySettings securitySettings;


        public DogController(IDogService dogService, IUserService userService, IAppSettingsService appSettingsService, IOptions<SecuritySettings> securitySettings)
        {
            this.userService = userService;
            this.dogService = dogService;
            this.appSettingsService = appSettingsService;
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
                if (!appSettingsService.canEnter())
                    throw new AppException("Akcja obecnie niedozwolona");
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
                if (!appSettingsService.canEnter())
                    throw new AppException("Akcja obecnie niedozwolona");
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
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                if (!appSettingsService.canEnter())
                    throw new AppException("Akcja obecnie niedozwolona");
                userService.CanUserAccessDog(claimsIdentity, dogId);
                dogService.editDog(dogId, newDog);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
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


        [AllowAnonymous]
        [HttpGet("getBreed/{breedId}")]
        public IActionResult getBreed (int breedId)
        {
            try
            {
                var response = dogService.getBreed(breedId);
                if (response == null)
                    throw new AppException("Nie odnaleziono rasy");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}