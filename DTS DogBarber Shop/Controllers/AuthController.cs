using System.Collections.Generic;
using System.Threading.Tasks;
using DTS_DogBarber_Shop.Auth;
using DTS_DogBarber_Shop.Data.Models.Dtos.Requests;
using DTS_DogBarber_Shop.Data.Models.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DTS_DogBarber_Shop.Controllers
{
    [Route("api/[controller]")] // api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepo _repository;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthController(IAuthRepo repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                RegistrationResponse response = await _repository.Register(user);
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            log.Error("Register() - Model isn't valid");
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                RegistrationResponse response = await _repository.Login(user);
                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }

            log.Error("Login() - Model isn't valid");
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

    }
}
