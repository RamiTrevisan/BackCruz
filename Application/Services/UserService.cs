using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

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

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            return await _userRepository.AuthenticateAsync(username, password);
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