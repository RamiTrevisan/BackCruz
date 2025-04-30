using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST: api/User
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(createUserDto.Username))
                    return BadRequest(new { message = "Username is required" });

                if (string.IsNullOrWhiteSpace(createUserDto.Password))
                    return BadRequest(new { message = "Password is required" });

                // Mapear de DTO a entidad
                var user = _mapper.Map<User>(createUserDto);

                // Crear usuario
                var createdUser = await _userService.CreateUserAsync(user, createUserDto.Password);

                // Mapear la respuesta
                var userDto = _mapper.Map<UserDto>(createdUser);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize] 
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var user = _mapper.Map<User>(userDto);

                // Actualizar usuario
                await _userService.UpdateUserAsync(user, userDto.Password);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Authenticate([FromBody] AuthenticateDto model)
        {
            var response = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (response == null)
                return BadRequest(new { message = "Usuario o contraseña incorrectos" });

            var userDto = _mapper.Map<UserDto>(response.User);

            return Ok(new
            {
                Token = response.Token,
                User = userDto
            });
        }

    }
}