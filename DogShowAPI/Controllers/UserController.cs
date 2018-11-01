using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DogShowAPI.DTOs;
using DogShowAPI.Helpers;
using DogShowAPI.Models;
using DogShowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DogShowAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private readonly SecuritySettings securitySettings;

        public UserController(IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            this.userService = userService;
            this.securitySettings = securitySettings.Value;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(userService.GetUserByID(id));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]UserDTO user)
        {
            var loggedUser = userService.Login(user.Email, user.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securitySettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loggedUser.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            int permissionLevel = userService.GetUserPermissionLevel(loggedUser.UserId);
            if (permissionLevel == -1)
                return BadRequest(new { message = "db error" });
            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = loggedUser.UserId,
                Email = loggedUser.Email,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                PermissionLevel = permissionLevel,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDTO user)
        {
            User newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email
            };

            try
            {
                // save 
                userService.Create(newUser, user.Password, 4);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("addJudge")]
        public IActionResult AddJudge([FromBody]UserDTO user)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            int userId = int.Parse(userName);
            int permissionLevel = userService.GetUserPermissionLevel(userId);
            if (permissionLevel == 1 || permissionLevel == 2)
            {
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email
                };

                try
                {
                    userService.Create(newUser, user.Password, 3);
                    return Ok();
                }
                catch (AppException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            else
                return BadRequest(new { message = "Username or password is incorrect" });
        }

        //[AllowAnonymous]
        //[HttpGet("testadmin")]
        //public IActionResult TestAdmin()
        //{
        //    User newUser = new User
        //    {
        //        FirstName = "Admin",
        //        LastName = "Admin",
        //        Address = "",
        //        Email = "administrator"
        //    };

        //    try
        //    {
        //        // save 
        //        userService.Create(newUser, "admin", 1);
        //        return Ok();
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
    }
}
