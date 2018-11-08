using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogShowAPI.Helpers;
using DogShowAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DogShowAPI.Controllers
{
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


    }
}