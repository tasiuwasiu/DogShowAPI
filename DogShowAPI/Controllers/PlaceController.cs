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
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int userId = int.Parse(userName);
            int userPermissions = userService.GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                return Unauthorized();
            }

            Place response = placeService.addPlace(newPlace);
            if (response == null)
            {
                return BadRequest(new { message = "Błąd dodawania miejsca!" });
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetPlace(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int userId = int.Parse(userName);
            int userPermissions = userService.GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                return Unauthorized();
            }

            Place response = placeService.getPlace(id);
            if (response == null)
            {
                return BadRequest(new { message = "Nie odnaleziono miejsca!" });
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllPlaces()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int userId = int.Parse(userName);
            int userPermissions = userService.GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                return Unauthorized();
            }

            return Ok(placeService.getAllPlaces());
        }

        [HttpPut ("{id}")]
        public IActionResult EditPlace (int id, [FromBody]Place newPlace)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Błąd autoryzacji" });
            }
            int userId = int.Parse(userName);
            int userPermissions = userService.GetUserPermissionLevel(userId);
            if (userPermissions != 1 && userPermissions != 2)
            {
                return Unauthorized();
            }

            Place response = placeService.editPlace(id, newPlace);

            if (response == null)
            {
                return BadRequest(new { message = "Nie odnaleziono miejsca!" });
            }
            return Ok(response);
        }


    }
}