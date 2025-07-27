using LSTM.Models;
using LSTM.Models.DTOs;

namespace LSTM.Business.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(LoginDto loginDto);
        Task<User?> RegisterAsync(RegisterDto registerDto);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
    }
} 