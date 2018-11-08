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
    public class AppSettingsController : ControllerBase
    {
        private IAppSettingsService appSettingsService;
        private readonly SecuritySettings securitySettings;

        public AppSettingsController(IAppSettingsService appSettingsService, IOptions<SecuritySettings> securitySettings)
        {
            this.appSettingsService = appSettingsService;
            this.securitySettings = securitySettings.Value;
        }

    }
}