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
using Newtonsoft.Json;

namespace DogShowAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private IAppSettingsService appSettingsService;
        private IUserService userService;
        private readonly SecuritySettings securitySettings;

        public AppSettingsController(IAppSettingsService appSettingsService, IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            this.appSettingsService = appSettingsService;
            this.userService = userService;
            this.securitySettings = securitySettings.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult getAppSettings()
        {
            AppSetting title = appSettingsService.getTitle();
            if (title == null)
            {
                return BadRequest(new { message = "Nie odnaleziono tytułu w bazie!" });
            }
            TitleSetting t = JsonConvert.DeserializeObject<TitleSetting>(title.SettingValue);

            AppSetting appState = appSettingsService.getAppState();
            if (appState == null)
            {
                return BadRequest(new { message = "Nie odnaleziono stanu aplikacji w bazie! Ustaw stan w ustawieniach!" });
            }
            AppStateSetting a = JsonConvert.DeserializeObject<AppStateSetting>(appState.SettingValue);

            AppSettingsDTO response = new AppSettingsDTO
            {
                title = t.title,
                appState = a.appState
            };

            if (response == null)
            {
                return BadRequest(new { message = "Błąd edycji tytułu!" });
            }
            return Ok(response);
        }


        [HttpPost("title")]
        public IActionResult setTitle([FromBody]TitleSetting title)
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

            string jsonObject = JsonConvert.SerializeObject(title);

            var response = appSettingsService.setTitle(jsonObject);

            if (response == null)
            {
                return BadRequest(new { message = "Błąd edycji tytułu!" });
            }
            return Ok();
        }

        [HttpPost("appState")]
        public IActionResult setAppState([FromBody]AppStateSetting appState)
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

            if (!Enum.IsDefined(typeof(AppStates), appState.appState))

            {
                return BadRequest(new { message = "Błędny kod stanu" });
            }

            string jsonObject = JsonConvert.SerializeObject(appState);

            var response = appSettingsService.setAppState(jsonObject);

            if (response == null)
            {
                return BadRequest(new { message = "Błąd edycji tytułu!" });
            }
            return Ok();
        }


        [AllowAnonymous]
        [HttpGet("title")]
        public IActionResult getTitle()
        {
            AppSetting response = appSettingsService.getTitle();
            if (response == null)
            {
                return BadRequest(new { message = "Nie odnaleziono tytułu w bazie!" });
            }

            TitleSetting jsonTitle = JsonConvert.DeserializeObject<TitleSetting>(response.SettingValue);

            return Ok(jsonTitle);
        }

        [AllowAnonymous]
        [HttpGet("appState")]
        public IActionResult getAppState()
        {
            AppSetting response = appSettingsService.getAppState();
            if (response == null)
            {
                return BadRequest(new { message = "Nie odnaleziono tytułu w bazie!" });
            }

            AppStateSetting jsonObject = JsonConvert.DeserializeObject<AppStateSetting>(response.SettingValue);

            return Ok(jsonObject);
        }

    }
}