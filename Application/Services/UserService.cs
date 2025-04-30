using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration; 
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<AuthenticateResponseDto> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _userRepository.AuthenticateAsync(username, password);
            if (user == null)
                return null;

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthenticateResponseDto
            {
                Token = tokenString,
                User = user
            };
        }


        public async Task<User> CreateUserAsync(User user, string password)
        {
            // Validaciones básicas
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required", nameof(password));

            // En una implementación real, deberías hashear la contraseña antes de guardarla
            // user.Password = HashPassword(password);
            user.Password = password;
            user.CreatedAt = DateTime.Now;

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User userParam, string password = null)
        {
            var user = await _userRepository.GetByIdAsync(userParam.Id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Actualizar propiedades
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // Verificar si el nombre de usuario ya existe
                var existingUser = await _userRepository.GetByUsernameAsync(userParam.Username);
                if (existingUser != null && existingUser.Id != user.Id)
                    throw new ApplicationException($"Username {userParam.Username} is already taken");

                user.Username = userParam.Username;
            }

            // Actualizar contraseña si fue proporcionada
            if (!string.IsNullOrWhiteSpace(password))
            {
                // En una implementación real, deberías hashear la contraseña antes de guardarla
                // user.Password = HashPassword(password);
                user.Password = password;
            }

            user.RoleId = userParam.RoleId;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            await _userRepository.DeleteAsync(id);
        }

        // Método auxiliar para hashear contraseña (implementación simplificada)
        // private string HashPassword(string password)
        // {
        //     // En una aplicación real, deberías usar una librería como BCrypt.Net
        //     // return BCrypt.Net.BCrypt.HashPassword(password);
        //     return password; // Esta es solo una implementación temporal
        // }
    }
}