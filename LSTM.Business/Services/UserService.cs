using LSTM.Business.Interfaces;
using LSTM.Data.Interfaces;
using LSTM.Models;
using LSTM.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace LSTM.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<User?> AuthenticateAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);
                
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: User {Username} not found", loginDto.Username);
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Authentication failed: Invalid password for user {Username}", loginDto.Username);
                    return null;
                }

                // 更新最后登录时间
                await UpdateLastLoginAsync(user.Id);

                _logger.LogInformation("User {Username} authenticated successfully", loginDto.Username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user {Username}", loginDto.Username);
                return null;
            }
        }

        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // 检查用户名是否已存在
                if (await IsUsernameExistsAsync(registerDto.Username))
                {
                    _logger.LogWarning("Registration failed: Username {Username} already exists", registerDto.Username);
                    return null;
                }

                // 检查邮箱是否已存在
                if (await IsEmailExistsAsync(registerDto.Email))
                {
                    _logger.LogWarning("Registration failed: Email {Email} already exists", registerDto.Email);
                    return null;
                }

                // 创建新用户
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {Username} registered successfully", registerDto.Username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user {Username}", registerDto.Username);
                return null;
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Users.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID {UserId}", id);
                return null;
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                return await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username {Username}", username);
                return null;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                return null;
            }
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.Now;
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last login for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if username exists {Username}", username);
                return false;
            }
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists {Email}", email);
                return false;
            }
        }
    }
} 