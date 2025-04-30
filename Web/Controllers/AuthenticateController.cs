using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Autentica un usuario y devuelve el token JWT
        /// </summary>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] AuthenticateDto model)
        {
            var response = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (response == null)
                return BadRequest(new { message = "Usuario o contraseña inválidos." });

            return Ok(response.Token); // 🔥 Solo devuelve el token
        }
    }
}
