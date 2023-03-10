using Authentication.Shared.Dtos.Request;
using Authentication.Shared.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> EntrarAsync(LoginRequest usuario)
        {
            var result = await _service.EntrarAsync(usuario);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegistrarAsync(CreateUserRequest usuario)
        {
            var result = await _service.RegistrarAsync(usuario);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
