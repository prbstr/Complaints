using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Complaints.Core.User;
using Complaints.Data.DataModels;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Complaints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [Route("register")]
        [HttpPost]
        public IActionResult CreateUser([FromBody]RegisterDataModel userRegistrationModel)
        {
            try
            {
                var userEntity = UserEntity.MapToEntity(userRegistrationModel);
                var user = _userService.Create(userEntity, userRegistrationModel.Password);
                return Ok();
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("login")]
        [HttpPost]
        public IActionResult AuthenticateUser([FromBody]AuthenticateDataModel userAuthenticationModel)
        {
            try
            {
                var user = _userService.Authenticate(userAuthenticationModel.Username, userAuthenticationModel.Password);
                var tokenString = _userService.GenerateToken(user);
                return Ok(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Token = tokenString
                });
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}