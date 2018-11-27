using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class PlaceController : ControllerBase
    {
        private IPlaceService placeService;
        private IUserService userService;
        private readonly SecuritySettings securitySettings;

        public PlaceController(IPlaceService placeService, IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            this.placeService = placeService;
            this.userService = userService;
            this.securitySettings = securitySettings.Value;
        }
        
        [HttpPost("create")]
        public IActionResult AddPlace([FromBody]Place newPlace)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                Place response = placeService.addPlace(newPlace);
                if (response == null)
                    throw new AppException("Błąd dodawania miejsca");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPlace(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                Place response = placeService.getPlace(id);
                if (response == null)
                    throw new AppException("Brak miejsca o podanym id w bazie");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllPlaces()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                return Ok(placeService.getAllPlaces());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut ("{id}")]
        public IActionResult EditPlace (int id, [FromBody]Place newPlace)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                var response = placeService.editPlace(id, newPlace);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete ("{id}")]
        public IActionResult DeletePlace (int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            try
            {
                userService.IsUserAnOrganizator(claimsIdentity);
                placeService.deletePlace(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}