using Microsoft.AspNetCore.Mvc;
using _08_VuTriDung_Assignment01.DTOs;
using _08_VuTriDung_Assignment01.Repositories;

namespace _08_VuTriDung_Assignment01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthController(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _accountRepository.Authenticate(request.Email, request.Password, _configuration);
            if (result == null)
            {
                return Unauthorized(new { message = "You do not have permission or invalid credentials!" });
            }

            return Ok(result);
        }
    }
}
