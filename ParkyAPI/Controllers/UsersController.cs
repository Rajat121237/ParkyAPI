using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository) => _userRepository = userRepository;


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authentiate([FromBody] Authentication model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);
            if(user == null)
                return BadRequest(new { message="Username or password is incorrect" } );

            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Authentication userObj)
        {
            bool isUserNameUnique = _userRepository.IsUniqueUser(userObj.Username);
            if (!isUserNameUnique)
                return BadRequest(new { message = "Username already exists" });

           var user = _userRepository.Register(userObj.Username, userObj.Password);
            if(user == null)
                return BadRequest(new { message = "Error while registering" });

            return Ok(user);
        }

    }
}
