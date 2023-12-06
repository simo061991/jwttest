using UserMicroservice.Model;
using UserMicroservice.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AuthorizeAttribute = FoodUsers.Authorization.AuthorizeAttribute;
using UserMicroservice.DataService;

namespace UserMicroservice.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class User : ControllerBase
    {

        private IUserData _userService;

        public User(
        IUserData userService)
        {
            _userService = userService;
   
        }
        [AllowAnonymous]
        [HttpPost ("login")]
        public IActionResult Login (LoginRequest login)
        {
            var response = _userService.Authenticate(login);
            if(response.rspns.rspEnum== ResponseEnum.OK)
            {
                //return Ok(response.rspns.Message);
                return Ok(response);
            }

            return BadRequest(response.rspns.Message);
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register (RegisterRequest regModel)
        {
            var response = _userService.Register(regModel);
            if(response.rspEnum == ResponseEnum.OK)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
            
        }

        [HttpPost("update")]
        public IActionResult Update(UpdateRequest update)
        {
            return Ok();
        }
    }
}
