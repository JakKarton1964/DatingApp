using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;
        public AuthController (IAuthRepository repo) {
            this._repo = repo;

        }

        [HttpPost("repository")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _repo.UserExist(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}