using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogShowAPI.Helpers;
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

        private IDogService dogService;
        private readonly SecuritySettings securitySettings;

        public DogController(IDogService dogService, IOptions<SecuritySettings> securitySettings)
        {
            this.dogService = dogService;
            this.securitySettings = securitySettings.Value;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet ("getGroups")]
        public IActionResult getGroups()
        {
            return Ok(dogService.getGroups());
        }

        [AllowAnonymous]
        [HttpGet ("getSectionsInGroup/{groupID}")]
        public IActionResult getSectionsInGroup (int groupID)
        {
            return Ok(dogService.getSectionsInGroup(groupID));
        }

        [AllowAnonymous]
        [HttpGet("getBreedsInSection/{sectionID}")]
        public IActionResult getBreedsInSection(int sectionID)
        {
            return Ok(dogService.getBreedsInSection(sectionID));
        }

    }
}